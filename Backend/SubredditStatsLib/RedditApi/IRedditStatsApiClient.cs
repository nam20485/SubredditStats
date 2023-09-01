using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.RedditApi
{
    public interface IRedditStatsApiClient
    {
        Task<RedditPostListing?> FetchSubredditPostListing(string subreddit, RedditStatsApiClient.PostListingSortType sort);
        Task<string> GetAboutContributors(string subreddit);
        Task<RedditPostListing?> GetSubredditNewPosts(string subreddit);
        Task<RedditPostListing?> GetSubredditTopPosts(string subreddit);
    }
}