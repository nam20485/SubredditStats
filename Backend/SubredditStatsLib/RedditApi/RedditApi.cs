using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public class RedditApi
    {
        public static string? ClientId => Environment.GetEnvironmentVariable("REDDIT_API_CLIENT_ID");
        public static string? ClientSecret => Environment.GetEnvironmentVariable("REDDIT_API_CLIENT_SECRET");
        public static string AppName => "subrpoststats";        

        public const string TokenUrl = "https://www.reddit.com/api/v1/access_token";

        public const string ApiUri = "https://oauth.reddit.com";

        public const string UserAgentName = "subrpoststats";
        public const string UserAgentVersion = "1.0";
    }
}
