using System.Net.Http.Headers;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public class RedditApi
    {
        public static string? ClientId => Environment.GetEnvironmentVariable("REDDIT_API_CLIENT_ID");
        public static string? ClientSecret => Environment.GetEnvironmentVariable("REDDIT_API_CLIENT_SECRET");
        public static string AccessTokenFromEnvironment => Environment.GetEnvironmentVariable("REDDIT_API_ACCESS_TOKEN");

        public const string TokenUrl = "https://www.reddit.com/api/v1/access_token";
        public const string ApiUri = "https://oauth.reddit.com";

        public const string RateLimitUsedHeaderName = "X-Ratelimit-Used";
        public const string RateLimitRemainingHeaderName = "X-Ratelimit-Remaining";
        public const string RateLimitResetHeaderName = "X-Ratelimit-Reset";

        public const string AppName = "subrpoststats";

        public const string UserAgentName = AppName;
        public const string UserAgentVersion = "1.0";

        public static ProductInfoHeaderValue MakeUserAgentHeader() => new (RedditApi.UserAgentName, UserAgentVersion);

        public const int RateLimitPeriodS = 600; 
    }
}
