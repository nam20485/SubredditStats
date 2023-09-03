using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Client;
using SubredditStats.Shared.Utils;

namespace SubredditStats.Frontend.ConsoleClient
{
    internal class PostStatsApp
    {
        private const string DefaultApiUrl = "https://localhost:7199";        

        public enum ExitCode
        {
            Success = 0,
            InvalidArguments,
            UnknownError
        }

        private static readonly Logger Logger = new();

        private readonly ISubredditPostStatsClient _apiClient;
        private readonly PostStatsAppCliArguments _psaArgs;        

        public PostStatsApp(string[] args)
        {
            _psaArgs = new PostStatsAppCliArguments(args);           

            _apiClient = new SubRedditPostStatsClient(new HttpClient()
            {
                BaseAddress = new Uri(_psaArgs.ApiUrl ?? DefaultApiUrl)
            });
        }

        internal ExitCode Run()
        {
            try
            {
                Console.WriteLine("PostStatsApp Client v0.9");
                Console.WriteLine();

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

        //private readonly ConsoleAnimation.GetFrameTextFunc _getPostsFrameFunc = (frameNumber) =>
        //{
        //    return GetPostsFrame(frameNumber);
        //};    
        
        private string GetPostsFrame(uint frameNumber)
        {
            var sb = new StringBuilder();

            sb.Append("top post 1\ntop post 2\ntop post 3\n");

            return sb.ToString();
        }
    }
}
