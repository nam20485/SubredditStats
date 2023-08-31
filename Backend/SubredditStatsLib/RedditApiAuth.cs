using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SubredditStats.Backend.Lib
{
    public class RedditApiAuth
    {
        public static string? ClientId => Environment.GetEnvironmentVariable("REDDIT_API_CLIENT_ID");
        public static string? ClientSecret => Environment.GetEnvironmentVariable("REDDIT_API_CLIENT_SECRET");
        public static string AppName => "subrpoststats";

        private const string TokenUrl = "https://www.reddit.com/api/v1/access_token";

        public const string UserAgent = "subrpoststats";

        public static async Task<RedditApiToken?> FetchAccessToken()
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl);
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(UserAgent, "1.0"));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            });
            var response = await client.SendAsync(request);
            var token = await response.Content.ReadFromJsonAsync<RedditApiToken>();
            return token;
        }
    }
}
