using System.Text.Json.Serialization;

using SubredditStats.Backend.Lib.RedditApi;
using SubredditStats.Backend.Lib.Store;
using SubredditStats.Backend.WebApi.Services;

namespace SubredditStats.Backend.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);           

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // add ability to directly use & parse enums in the endpoint parameters
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });       
            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // add our custom services to the DI container (separate functionality is implemented
            // in its own service to reduce coupling and increase cohesion & encapsulation)
            builder.Services.AddHttpClient<IRedditApiTokenService, RedditApiTokenService>();
            builder.Services.AddHttpClient<IRedditStatsClient, RedditStatsApiClient>();
            builder.Services.AddSingleton<ISubredditPostsStatsStore, MemoryStore>();            
            builder.Services.AddHostedService<SubredditPostsStatsCalculator>();

            var app = builder.Build();   
            
            // confire HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}