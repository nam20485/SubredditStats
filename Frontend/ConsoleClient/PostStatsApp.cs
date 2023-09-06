using System.Text;

using SubredditStats.Frontend.ConsoleClient.Utils;
using SubredditStats.Shared.Client;

namespace SubredditStats.Frontend.ConsoleClient
{
    internal partial class PostStatsApp
    {
          

        private static readonly Logger Logger = new();

        private readonly ISubredditPostStatsClient _apiClient;
        private readonly PostStatsAppCliArguments _psaArgs;
        
        public PostStatsApp(string[] args)
        {
            _psaArgs = new PostStatsAppCliArguments(args);            
            _apiClient = new SubRedditPostStatsApiClient(new HttpClient()
            {
                BaseAddress = new Uri(_psaArgs.ApiUrl)
            });
        }

        internal ExitCode Run()
        {
            try
            {
                Console.WriteLine($"PostStatsApp Client v0.9 - [api: {_psaArgs.ApiUrl}]\n");

                if (_apiClient.VerifyConnection())
                {
                    using (var consoleColors = new ConsoleColors(ConsoleColor.DarkGray))
                    {
                        Console.WriteLine("(Press CTRL+P to quit)\n");
                    }

                    using (var postsAnimation = new ConsoleAnimation(GetPostsFrame))
                    {
                        postsAnimation.Start();
                        WaitForExitChar.Wait(ConsoleKey.P, ConsoleModifiers.Control);
                    }

                    return ExitCode.Success;
                }
                else
                {
                    Console.WriteLine("Cannot reach api: verify API server is running and accessible at the above url!");
                    return ExitCode.ApiUnreachable;
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
            
            var topPosts = _apiClient.GetNumberOfTopPosts(_psaArgs.PostCount);
            if (topPosts.Any())
            {
                var strTitle = $"{_psaArgs.PostCount} Top Posts (/r/{topPosts.First().Subreddit}):";
                sb.AppendLine(strTitle);
                //sb.AppendLine();
                sb.AppendLine(new string('-', strTitle.Length));
                foreach (var topPost in topPosts)
                {
                    sb.AppendLine(topPost.ToString());
                }
                sb.AppendLine();
            }

            var mostPosters = _apiClient.GetNumberOfMostPosters(_psaArgs.PostCount);
            if (mostPosters.Any())
            {
                var strTitle = $"{_psaArgs.PostCount} Most Posters (/r/{mostPosters.First().Subreddit}):";
                sb.AppendLine(strTitle);
                //sb.AppendLine(new string('-', strTitle.Length));
                //sb.AppendLine();            
                foreach (var mostPoster in mostPosters)
                {
                    sb.AppendLine(mostPoster.ToString());
                }
            }

            return sb.ToString();
        }        

        private static void ThrowA<TException>() where TException : Exception, new()
        {
            throw new TException();            
        }
    }
}
