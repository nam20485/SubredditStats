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
        
        private const string SubredditPostListingUriFormat = RedditApi.ApiUri + "/r/{0}/{1}";     
        
        private readonly IRedditApiTokenService _apiTokenService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RedditStatsApiClient> _logger;        

        public double LastRateLimitUsed { get; private set; }
        public double LastRateLimitRemaining { get; private set; }
        // number of seconds remaining until new period starts
        public int LastRateLimitReset { get; private set; }

        public RedditStatsApiClient(HttpClient httpClient, ILogger<RedditStatsApiClient> logger, IRedditApiTokenService apiTokenService)
        {           
            _logger = logger;            
            _apiTokenService = apiTokenService;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(RedditApi.ApiUri);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(RedditApi.UserAgentName, RedditApi.UserAgentVersion));            
        }

        public async Task<RedditPostListing?> FetchSubredditPostListing(string subreddit, PostListingSortType sort)
        {
            var accessToken = await _apiTokenService.GetRedditApiAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

            var uri = string.Format(SubredditPostListingUriFormat, subreddit, Enum.GetName<PostListingSortType>(sort));
            var response = await _httpClient.GetAsync(uri);
            GetRateLimitValues(response);
            if (response.IsSuccessStatusCode)
            {
                var s = await response.Content.ReadAsStringAsync();
                var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s);
                return redditPostListing;
            }
            
            return null;
        }
        
        public async Task<RedditPostListing?> GetSubredditPostsSortedByTop(string subreddit)
        {
            return await FetchSubredditPostListing(subreddit, PostListingSortType.top);
        }

        public async Task<RedditPostListing?> GetSubredditPostsSortedByNew(string subreddit)
        {
            return await FetchSubredditPostListing(subreddit, PostListingSortType.@new);
        }


        private void GetRateLimitValues(HttpResponseMessage response)
        {
            var rateLimitUsedHeader = response.Headers.GetValues("X-Ratelimit-Used");
            LastRateLimitUsed = double.Parse(rateLimitUsedHeader.First());
            var rateLimitRemainingdHeader = response.Headers.GetValues("X-Ratelimit-Remaining");
            LastRateLimitRemaining = double.Parse(rateLimitRemainingdHeader.First());
            var rateLimitResetHeader = response.Headers.GetValues("X-Ratelimit-Reset");
            LastRateLimitReset = int.Parse(rateLimitResetHeader.First());
        }
        
        //// returning HTTP 403 Forbidden (probably requires mod persmission)
        //public async Task<string> GetAboutContributors(string subreddit)
        //{
        //    var accessToken = await _apiTokenService.GetRedditApiAccessToken();
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        //    if (httpClient is not null)
        //    {
        //        //var redditPostListing = await httpClient.GetFromJsonAsync<RedditPostListing>($"https://oauth.reddit.com/r/{subreddit}/top");
        //        //return redditPostListing;
        //        var response = await httpClient.GetAsync($"https://oauth.reddit.com/r/{subreddit}/about/contributors");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var s = await response.Content.ReadAsStringAsync();
        //            //var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s, GlobalJsonSerializerOptions.Options);
        //            return s;

        //        }
        //    }
        //    return "";
        //}       
    }
}
