using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public interface IRedditStatsClient
    {
        Task<RedditPostListing?> FetchSubredditPostListing(string subreddit,
                                                           RedditStatsApiClient.PostListingSortType sort = RedditStatsApiClient.PostListingSortType.unspecified,
                                                           int limit = 25,
                                                           string? after = "",
                                                           int count = 0);

        //Task<string> GetAboutContributors(string subreddit);

        Task<RedditPostListing?> GetSubredditPostsSortedByNew(string subreddit, int limit = 25, string? after = null, int count = 0);
        Task<RedditPostListing?> GetSubredditPostsSortedByTop(string subreddit, int limit = 25, string? after = null, int count = 0);
    }
}