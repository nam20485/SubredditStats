using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using static System.Net.WebRequestMethods;

namespace SubredditStats.Backend.WebApi
{

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    public static class SwaggerConfigurationExtensions
    {
        private const bool SyntaxHighlightingEnabled = true;

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new ()
                {
                    Version = "v1",
                    Title = "Subreddit Post Stats API",                    
                    Description = $"Web API for Subreddit Post Stats",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nathan Miller",
                        Url = new Uri("https://www.github.com/nam20485")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/nam20485/SubredditStats/blob/7ce860f1e9e1a40747abb0a1229a352cc4449b7c/LICENSE")
                    }                    
                });

                // add generated Xml comments
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }        

        public static IApplicationBuilder ConfigureSwaggerUI(this IApplicationBuilder app)
        {
            return app.UseSwaggerUI(options =>
            {
                options.EnableTryItOutByDefault();
                options.EnableDeepLinking();
                options.DisplayRequestDuration();
                options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>()
                {
                    ["activated"] = SyntaxHighlightingEnabled
                };
                options.ConfigObject.AdditionalItems["requestSnippetsEnabled"] = true;
                options.ConfigObject.AdditionalItems["requestSnippets"] = new Dictionary<string, object>()
                {
                    ["defaultExpanded"] = false
                };
            });
        }
    }
}
