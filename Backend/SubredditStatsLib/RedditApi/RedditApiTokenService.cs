using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public class RedditApiTokenService : IRedditApiTokenService
    {
        private readonly HttpClient _httpClient;

        private RedditApiToken? _currentAccessToken;

        public RedditApiTokenService(HttpClient httpClient)
        {
            _currentAccessToken = null;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(RedditApi.TokenUrl);
        }

        public async Task<RedditApiToken?> GetRedditApiAccessToken()
        {
            if (_currentAccessToken is null || _currentAccessToken.IsExpired)
            {
                _currentAccessToken = await FetchRedditApiAccessToken();
            }

            return _currentAccessToken;
        }

        public async Task<RedditApiToken?> FetchRedditApiAccessToken()
        {
            RedditApiToken? apiToken = null;

            var request = new HttpRequestMessage(HttpMethod.Post, RedditApi.TokenUrl);
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{RedditApi.ClientId}:{RedditApi.ClientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(RedditApi.UserAgentName, RedditApi.UserAgentVersion));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"}
            });

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                apiToken = await response.Content.ReadFromJsonAsync<RedditApiToken>();                
            }

            return apiToken;
        }
    }
}
