using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Frontend.ConsoleClient.Utils;
using SubredditStats.Shared.Client;

namespace SubredditStats.Frontend.ConsoleClient
{
    internal partial class PostStatsApp
    {
        private const string DefaultApiUrl = "https://localhost:7199";        

        private static readonly Logger Logger = new();

        private readonly ISubredditPostStatsClient _apiClient;
        private readonly PostStatsAppCliArguments _psaArgs;        

        private readonly string _apiUrl = DefaultApiUrl;

        public PostStatsApp(string[] args)
        {
            _psaArgs = new PostStatsAppCliArguments(args);  

            if (!string.IsNullOrWhiteSpace(_psaArgs.ApiUrl))
            {
                _apiUrl = _psaArgs.ApiUrl;
            }

            _apiClient = new SubRedditPostStatsClient(new HttpClient()
            {
                BaseAddress = new Uri(_apiUrl)
            });
        }

        internal ExitCode Run()
        {
            try
            {
                Console.WriteLine($"PostStatsApp Client v0.9 - (api: {_apiUrl})");               
                Console.WriteLine();

                if (! _apiClient.VerifyConnection())
                {
                    Console.WriteLine("Cannot reach api! Verify API server is running and reachable at the above url.");
                    return ExitCode.CantReachApi;
                }

                using (var consoleColors = new ConsoleColors(ConsoleColor.DarkGray))
                {
                    Console.WriteLine("(Press CTRL+P to quit)");
                }

                Console.WriteLine();

                using (var postsAnimation = new ConsoleAnimation(GetPostsFrame))
                {
                    postsAnimation.Start();
                    WaitForExitChar.Wait(ConsoleKey.P, ConsoleModifiers.Control);
                }                
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
          
            return ExitCode.UnknownError;
        }   
        
        private string GetPostsFrame(uint frameNumber)
        {
            var sb = new StringBuilder();

            sb.Append("top post 1\ntop post 2\ntop post 3\n");

            return sb.ToString();
        }
    }
}
