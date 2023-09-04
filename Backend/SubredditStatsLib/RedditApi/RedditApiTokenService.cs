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
            if (_currentAccessToken is null || _currentAccessToken.IsExpired)
            {
                _currentAccessToken = await FetchRedditApiAccessToken();
            }

            if (_currentAccessToken is null)
            {
                throw new NoApiAccessTokenException("Failed to fetch API access token");
            }

            return _currentAccessToken;
        }

        public async Task<RedditApiToken?> FetchRedditApiAccessToken()
        {
            RedditApiToken? apiToken = null;

            if (string.IsNullOrWhiteSpace(RedditApi.ClientId ) || string.IsNullOrWhiteSpace(RedditApi.ClientSecret))
            {
                _logger.LogError("Reddit API client ID and/or secret not set. Environment variables REDDIT_API_CLIENT_ID and REDDIT_API_CLIENT_SECRET must be set.");
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, RedditApi.TokenUrl);
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{RedditApi.ClientId}:{RedditApi.ClientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            request.Headers.UserAgent.Add(RedditApi.MakeUserAgentHeader());
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            });

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                apiToken = await response.Content.ReadFromJsonAsync<RedditApiToken>();

                _logger.LogInformation("Fetched new access token: expires in {ApiTokenDuration}", apiToken.Duration);
            }

            return apiToken;
        }
    }
}
