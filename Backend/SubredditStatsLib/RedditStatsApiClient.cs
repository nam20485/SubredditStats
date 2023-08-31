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
        private const string RedditApiUri = "https://oauth.reddit.com";
        private const string SubredditPostListingUriFormat = RedditApiUri + "/r/{0}/{1}";

        private RedditApiToken? _currentAccessToken;

        public enum PostListingSortType
        {
            top,
            @new
        }

        public RedditStatsApiClient()
        {
            _currentAccessToken = null;
        }  

        public async Task<RedditPostListing?> FetchSubredditPostListing(string subreddit, PostListingSortType sort)
        {
            using var httpClient = await CreateHttpClientAsync();
            if (httpClient is not null)
            {
                var uri = string.Format(SubredditPostListingUriFormat, subreddit, sort.ToString());
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var s = await response.Content.ReadAsStringAsync();
                    var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s);
                    return redditPostListing;

                }
            }
            return null;
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

            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(RedditApiAuth.UserAgent, "0.9"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentAccessToken.AccessToken);               
            return client;
        }
    }
}
