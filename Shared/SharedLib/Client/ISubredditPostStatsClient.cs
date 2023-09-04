namespace SubredditStats.Shared.Client
{
    public interface ISubredditPostStatsClient : ISubredditPostStatsSource
    {
        bool VerifyConnection();
    }
}
