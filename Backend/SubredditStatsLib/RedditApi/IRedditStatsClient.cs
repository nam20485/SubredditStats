using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public interface IRedditStatsClient
    {
        Task<RedditPostListing?> FetchSubredditPostListingSlice(string subreddit,
                                                                RedditStatsApiClient.PostListingSortType sort = RedditStatsApiClient.PostListingSortType.none,
                                                                int limit = 25,
                                                                string? after = "",
                                                                int count = 0);
        
        Task<RedditPostListing?> GetSubredditPostsSortedByNew(string subreddit, int limit = 25, string? after = null, int count = 0);
        Task<RedditPostListing?> GetSubredditPostsSortedByTop(string subreddit, int limit = 25, string? after = null, int count = 0);

        //Task<string> GetAboutContributors(string subreddit);
    }
}