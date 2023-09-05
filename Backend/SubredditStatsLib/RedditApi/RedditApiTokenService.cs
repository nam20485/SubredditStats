using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

using Microsoft.Extensions.Logging;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public class RedditApiTokenService : IRedditApiTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RedditApiTokenService> _logger;

        private RedditApiToken? _currentAccessToken;

        public RedditApiTokenService(HttpClient httpClient, ILogger<RedditApiTokenService> logger)
        {
            _currentAccessToken = null;
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(RedditApi.TokenUrl);
        }

        public async Task<RedditApiToken?> GetRedditApiAccessToken()
        {
            // try from environment first             
            if (_currentAccessToken is null || _currentAccessToken.IsExpired)
            {
                _currentAccessToken = GetApiAccessTokenFromEnvironment();                
            }

            // fetch from server
            if (_currentAccessToken is null || _currentAccessToken.IsExpired)
            {
                _currentAccessToken = await FetchApiAccessToken();
            }

            if (_currentAccessToken is null)
            {
                throw new NoApiAccessTokenException("Failed to fetch API access token");
            }

            return _currentAccessToken;
        }

        private static RedditApiToken? GetApiAccessTokenFromEnvironment()
        {
            if (Environment.GetEnvironmentVariable("REDDIT_API_ACCESS_TOKEN") is string tokenValue &&
                !string.IsNullOrWhiteSpace(tokenValue))
            {
                return new RedditApiToken()
                {
                    AccessToken = tokenValue,
                    ExpiresInS = 60 * 60        // 1 hour for "artifically" inserted token values                    
                };
            }

            return null;

        }

        public async Task<RedditApiToken?> FetchApiAccessToken()
        {
            RedditApiToken? apiToken = null;

            if (string.IsNullOrWhiteSpace(RedditApi.ClientId ) || string.IsNullOrWhiteSpace(RedditApi.ClientSecret))
            {
                _logger.LogError("Reddit API client ID and/or secret not set. Environment variables REDDIT_API_CLIENT_ID and REDDIT_API_CLIENT_SECRET must be set.");
                return null;
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, RedditApi.TokenUrl);
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{RedditApi.ClientId}:{RedditApi.ClientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            request.Headers.UserAgent.Add(RedditApi.MakeUserAgentHeader());
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            });

            using var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                apiToken = await response.Content.ReadFromJsonAsync<RedditApiToken>();

                _logger.LogInformation("Fetched new access token: expires in {ApiTokenDuration}", apiToken.Duration);
            }

            return apiToken;
        }
    }
}
