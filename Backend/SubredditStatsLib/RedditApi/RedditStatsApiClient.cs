using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Extensions.Logging;

using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public class RedditStatsApiClient : IRedditStatsClient
    {
        public enum PostListingSortType
        {
            unspecified,
            top,
            @new
        }
        
        private readonly IRedditApiTokenService _apiTokenService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RedditStatsApiClient> _logger;        

        public double LastRateLimitUsed { get; private set; }
        public double LastRateLimitRemaining { get; private set; }
        // number of seconds remaining until new period starts
        public int LastRateLimitPeriodReset { get; private set; }
        public int RateLimitPeriodPassed => 600 - LastRateLimitPeriodReset;
        public double SecondsPerRequestRate => RateLimitPeriodPassed / LastRateLimitUsed;
        public double RequestsPerSecondRate => LastRateLimitUsed / RateLimitPeriodPassed;

        public RedditStatsApiClient(HttpClient httpClient, ILogger<RedditStatsApiClient> logger, IRedditApiTokenService apiTokenService)
        {           
            _logger = logger;            
            _apiTokenService = apiTokenService;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(RedditApi.ApiUri);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(RedditApi.MakeUserAgentHeader());
        }

        public async Task<RedditPostListing?> FetchSubredditPostListingSlice(string subreddit,
                                                                             PostListingSortType sort = PostListingSortType.unspecified,
                                                                             int limit = 25,
                                                                             string? after = "",
                                                                             int count = 0)
        {
            LogRateLimitValues();

            var accessToken = await _apiTokenService.GetRedditApiAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

            var uri = BuildUri(subreddit, sort, limit, after, count);
            var response = await _httpClient.GetAsync(uri);
            SaveRateLimitValues(response);
            if (response.IsSuccessStatusCode)
            {
                var s = await response.Content.ReadAsStringAsync();
                var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s);
                return redditPostListing;
            }

            return null;
        }

        private void LogRateLimitValues()
        {
            _logger.LogInformation("Last Request's Rate Limit Header Values\n\tTotal Used: {LastRateLimitUsed}\n\tTotal Remaining: {LastRateLimitRemaining}\n\tPeriod Remaining (s): {LastRateLimitPeriodReset}\n\tPeriod Passed (s): {RateLimitPeriodPassed}\n\tRate (s/request): {SecondsPerRequestRate:0.0}\n\tRate (requests/s): {RequestsPerSecondRate:0.0}",
                                   LastRateLimitUsed,
                                   LastRateLimitRemaining,
                                   LastRateLimitPeriodReset,
                                   RateLimitPeriodPassed,
                                   SecondsPerRequestRate,
                                   RequestsPerSecondRate);
        }

        private static Uri BuildUri(string subreddit, PostListingSortType sort, int limit, string after, int count)
        {                      
            var query = HttpUtility.ParseQueryString(""); 
            query["count"] = count.ToString();
            query["limit"] = limit.ToString();
            if (!string.IsNullOrWhiteSpace(after))
            {
                query["after"] = after;
            }

            var uri = $"{RedditApi.ApiUri}/r/{subreddit}";            
            if (sort != PostListingSortType.unspecified)
            {
                uri += $"/{Enum.GetName<PostListingSortType>(sort)}";
            }

            var uriBuilder = new UriBuilder(uri)
            {
                Query = query.ToString()
            };
            return uriBuilder.Uri;
        }

        public async Task<RedditPostListing?> GetSubredditPostsSortedByTop(string subreddit, int limit = 25, string? after = "", int count = 0)
        {
            return await FetchSubredditPostListingSlice(subreddit, PostListingSortType.top, limit, after, count);
        }

        public async Task<RedditPostListing?> GetSubredditPostsSortedByNew(string subreddit, int limit = 25, string? after = "", int count = 0)
        {
            return await FetchSubredditPostListingSlice(subreddit, PostListingSortType.@new, limit, after, count);
        }

        private void SaveRateLimitValues(HttpResponseMessage response)
        {
            var rateLimitUsedHeader = response.Headers.GetValues("X-Ratelimit-Used");
            LastRateLimitUsed = double.Parse(rateLimitUsedHeader.First());
            var rateLimitRemainingdHeader = response.Headers.GetValues("X-Ratelimit-Remaining");
            LastRateLimitRemaining = double.Parse(rateLimitRemainingdHeader.First());
            var rateLimitResetHeader = response.Headers.GetValues("X-Ratelimit-Reset");
            LastRateLimitPeriodReset = int.Parse(rateLimitResetHeader.First());
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
