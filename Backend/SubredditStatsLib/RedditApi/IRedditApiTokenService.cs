namespace SubredditStats.Backend.Lib.RedditApi
{
    public interface IRedditApiTokenService
    {
        Task<RedditApiToken?> GetRedditApiAccessToken();
    }
}