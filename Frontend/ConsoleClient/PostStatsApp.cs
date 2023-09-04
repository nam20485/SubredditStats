﻿using System;
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
          

        private static readonly Logger Logger = new();

        private readonly ISubredditPostStatsClient _apiClient;
        private readonly PostStatsAppCliArguments _psaArgs;
        
        public PostStatsApp(string[] args)
        {
            _psaArgs = new PostStatsAppCliArguments(args);            
            _apiClient = new SubRedditPostStatsClient(new HttpClient()
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

            var strTitle = $"{_psaArgs.PostCount} Top Posts:";
            sb.AppendLine(strTitle);
            //sb.AppendLine();
            sb.AppendLine(new string('-', strTitle.Length));
            var topPosts = _apiClient.GetNumberOfTopPosts(_psaArgs.PostCount);
            foreach (var topPost in topPosts)
            {
                sb.AppendLine(topPost.ToString());
            }
            sb.AppendLine();

            strTitle = $"{_psaArgs.PostCount} Most Posters:";
            sb.AppendLine(strTitle);
            //sb.AppendLine(new string('-', strTitle.Length));
            //sb.AppendLine();
            var mostPosters = _apiClient.GetNumberOfMostPosters(_psaArgs.PostCount);
            foreach (var mostPoster in mostPosters)
            {
                sb.AppendLine(mostPoster.ToString());
            }            

            return sb.ToString();
        }        

        private static void SimulateException<TException>() where TException : Exception, new()
        {
            throw new TException();            
        }
    }
}
