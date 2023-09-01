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
                // add ability to directly use & parse enums in the endpoint definitions
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient("RedditStatsApiClient", client =>
            {
                client.BaseAddress = new Uri(RedditApiAuth.ApiUri);
            });
            builder.Services.AddHttpClient<IRedditStatsApiClient, RedditStatsApiClient>("RedditStatsApiClient");

            builder.Services.AddSingleton<ISubredditPostsStatsStore, MemoryBackingStore>();            
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