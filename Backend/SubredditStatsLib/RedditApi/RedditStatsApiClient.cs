using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Extensions.Logging;

using SubredditStats.Backend.Lib.Utils;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public class RedditStatsApiClient : IRedditStatsClient
    {
        public enum PostListingSortType
        {
            none,
            top,
            @new
        }
        
        private readonly IRedditApiTokenService _apiTokenService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RedditStatsApiClient> _logger;        

        private readonly RateLimitData _rateLimitData;

        public RedditStatsApiClient(HttpClient httpClient, ILogger<RedditStatsApiClient> logger, IRedditApiTokenService apiTokenService)
        {           
            _logger = logger;            
            _apiTokenService = apiTokenService;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(RedditApi.ApiUri);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(RedditApi.MakeUserAgentHeader());

            _rateLimitData = new (_logger);
        }

        public async Task<RedditPostListing?> FetchSubredditPostListingSlice(string subreddit,
                                                                             PostListingSortType sort = PostListingSortType.none,
                                                                             int limit = 25,
                                                                             string? after = "",
                                                                             int count = 0)
        {
            _rateLimitData.LogRateLimitValues();

            var accessToken = await _apiTokenService.GetRedditApiAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

            var uri = BuildUri(subreddit, sort, limit, after, count);
            var response = await _httpClient.GetAsync(uri);
            
            _rateLimitData.SaveRateLimitValues(response);

            if (response.IsSuccessStatusCode)
            {
                var s = await response.Content.ReadAsStringAsync();
                var redditPostListing = JsonSerializer.Deserialize<RedditPostListing>(s);
                return redditPostListing;
            }
            else if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                LogRateLimiterKind(response);
            }

            return null;
        }

        private void LogRateLimiterKind(HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues(ClientSideRateLimitedHandler.CustomRasteLimiterHeaderName, out var values))
            { 
                var rateLimiterName = values.First();
                if (rateLimiterName == nameof(ClientSideRateLimitedHandler))
                {
                    _logger.LogWarning("Rate limit reached for {RateLimiterName} (limited client-side)", rateLimiterName);
                }
            }
            else if (response.Headers.TryGetValues(RedditApi.RateLimitResetHeaderName, out values))
            {
                _logger.LogWarning("Rate limit reached (from the server)");
            }
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
            if (sort != PostListingSortType.none)
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
