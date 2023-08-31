using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib
{
    public class RedditStatsApiClient : ISubredditStatsSource
    {
        private RedditApiToken? _currentAccessToken;

        public RedditStatsApiClient()
        {
            _currentAccessToken = null;
        }  

        public async Task<RedditPostListing?> GetMostUpvotedPosts(string subreddit)
        {
            using var httpClient = await CreateHttpClientAsync();
            if (httpClient is not null)
            {
                //var redditPostListing = await httpClient.GetFromJsonAsync<RedditPostListing>($"https://oauth.reddit.com/r/{subreddit}/top");
                //return redditPostListing;
                var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/top");
                if (response.IsSuccessStatusCode)
                {
                    var s = await response.Content.ReadAsStringAsync();
                    var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s, GlobalJsonSerializerOptions.Options);
                    return redditPostListing;

                }
            }
            return null;
        }

        //public async Task<List<string>> GetMostPostingUsers(string subreddit)
        //{
        //    using var httpClient = await CreateHttpClientAsync();

        //}

        private async Task<HttpClient?> CreateHttpClientAsync()
        {
            if (_currentAccessToken == null || _currentAccessToken.IsExpired)
            {
                _currentAccessToken = await RedditApiAuth.FetchAccessToken();
            }

            if (_currentAccessToken is null) return null;

            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(RedditApiAuth.UserAgent, "0.9"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentAccessToken.AccessToken);               
            return client;
        }
    }
}
