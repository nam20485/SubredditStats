using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

using SubredditStats.Backend.Lib.RedditApi;
using SubredditStats.Backend.Lib.Store;
using SubredditStats.Backend.Lib.Utils;
using SubredditStats.Backend.WebApi.Services;

namespace SubredditStats.Backend.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddResponseCompression(options =>
            {
                // not safe for cookie-based token authentication, use header bearer tokens only!
                // https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-7.0#compression-with-https
                options.EnableForHttps = true;
            });
            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                // add ability to directly use & parse enums in the endpoint parameters
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });       
            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // add our custom services to the DI container (each separate functionality is 
            // implemented in its own service to reduce coupling and increase cohesion & encapsulation)
            builder.Services.AddHttpClient<IRedditApiTokenService, RedditApiTokenService>();
            builder.Services.AddHttpClient<IRedditStatsClient, RedditStatsApiClient>()
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    // use "static" client-side rate limiting (better solution would use values from rate limiting response headers)
                    return new ClientSideRateLimitedHandler(
                        new TokenBucketRateLimiter(
                            new TokenBucketRateLimiterOptions
                            {
                                // 60 requests per minute?
                                TokenLimit = 600,
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                QueueLimit = 0,
                                ReplenishmentPeriod = TimeSpan.FromSeconds(600),
                                TokensPerPeriod = 600,
                                AutoReplenishment = true
                            }));
                });
            builder.Services.AddSingleton<ISubredditPostStatsStore, MemoryStore>();            
            builder.Services.AddHostedService<SubredditPostStatsFetcher>();
            builder.Services.AddHostedService<SubredditPostStatsCalculator>();

            var app = builder.Build();   
            
            // confire HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}