using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public interface IRedditStatsClient
    {
        Task<RedditPostListing?> FetchSubredditPostListing(string subreddit, RedditStatsApiClient.PostListingSortType sort);
        //Task<string> GetAboutContributors(string subreddit);
        Task<RedditPostListing?> GetSubredditPostsSortedByNew(string subreddit);
        Task<RedditPostListing?> GetSubredditPostsSortedByTop(string subreddit);
    }
}