using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public class RedditStatsApiClient : IRedditStatsClient
    {
        public enum PostListingSortType
        {
            top,
            @new
        }

        public const string HttpClientName = "RedditStatsApiClient";

        private const string SubredditPostListingUriFormat = RedditApiAuth.ApiUri + "/r/{0}/{1}";

        private readonly IHttpClientFactory? _httpClientFactory;
        //private readonly ILogger<RedditStatsApiClient> _logger;

        private readonly HttpClient _httpClient;

        private RedditApiToken? _currentAccessToken;

        public double RateLimitUsed { get; private set; }
        public double RateLimitRemaining { get; private set; }
        // number of seconds remaining until new period starts
        public int RateLimitReset { get; private set; }

        public RedditStatsApiClient(HttpClient httpClient)
        {
            //_logger = logger;
            _httpClient = httpClient;
            //_httpClientFactory = httpClientFactory;
            _currentAccessToken = null;
        }

        public async Task<RedditPostListing?> FetchSubredditPostListing(string subreddit, PostListingSortType sort)
        {
            var httpClient = await CreateHttpClientAsync();
            if (httpClient is not null)
            {
                var uri = string.Format(SubredditPostListingUriFormat, subreddit, sort.ToString());
                var response = await httpClient.GetAsync(uri);
                GetRateLimitValues(response);
                if (response.IsSuccessStatusCode)
                {
                    var s = await response.Content.ReadAsStringAsync();
                    var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s);
                    return redditPostListing;

                }
            }
            return null;
        }

        private void GetRateLimitValues(HttpResponseMessage response)
        {
            var rateLimitUsedHeader = response.Headers.GetValues("X-Ratelimit-Used");
            RateLimitUsed = double.Parse(rateLimitUsedHeader.First());
            var rateLimitRemainingdHeader = response.Headers.GetValues("X-Ratelimit-Remaining");
            RateLimitRemaining = double.Parse(rateLimitRemainingdHeader.First());
            var rateLimitResetHeader = response.Headers.GetValues("X-Ratelimit-Reset");
            RateLimitReset = int.Parse(rateLimitResetHeader.First());
        }

        public async Task<RedditPostListing?> GetSubredditTopPosts(string subreddit)
        {
            return await FetchSubredditPostListing(subreddit, PostListingSortType.top);
        }

        public async Task<RedditPostListing?> GetSubredditNewPosts(string subreddit)
        {
            return await FetchSubredditPostListing(subreddit, PostListingSortType.@new);
        }

        // returning HTTP 403 Forbidden (probably requires mod persmission)
        public async Task<string> GetAboutContributors(string subreddit)
        {
            using var httpClient = await CreateHttpClientAsync();
            if (httpClient is not null)
            {
                //var redditPostListing = await httpClient.GetFromJsonAsync<RedditPostListing>($"https://oauth.reddit.com/r/{subreddit}/top");
                //return redditPostListing;
                var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/about/contributors");
                if (response.IsSuccessStatusCode)
                {
                    var s = await response.Content.ReadAsStringAsync();
                    //var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s, GlobalJsonSerializerOptions.Options);
                    return s;

                }
            }
            return null;
        }

        private async Task<HttpClient?> CreateHttpClientAsync()
        {
            if (_currentAccessToken == null || _currentAccessToken.IsExpired)
            {
                _currentAccessToken = await RedditApiAuth.FetchAccessToken();
            }

            if (_currentAccessToken is null) return null;

            var client = _httpClient;
            //_httpClientFactory.CreateClient(HttpClientName);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(RedditApiAuth.UserAgent, "0.9"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentAccessToken.AccessToken);
            return client;
        }
    }
}
