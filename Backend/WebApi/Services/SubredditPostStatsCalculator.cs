using SubredditStats.Backend.Lib.Store;
using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.WebApi.Services
{
    public class SubredditPostStatsCalculator : BackgroundService, IDisposable
    {
        private readonly ISubredditPostStatsStore _store;
        private readonly ILogger<SubredditPostStatsCalculator> _logger;

        private readonly AutoResetEvent _postListUpdatedSignal;

        public SubredditPostStatsCalculator(ISubredditPostStatsStore store, ILogger<SubredditPostStatsCalculator> logger)
        {
            _postListUpdatedSignal = new AutoResetEvent(false);

            _logger = logger;
            _store = store;

            _store.PostListUpdated += OnStorePostListUpdated;            
        }

        private void OnStorePostListUpdated(ISubredditPostStatsSource sender)
        {
            _postListUpdatedSignal.Set();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                CalculateSubredditPostStats(stoppingToken);
            }, stoppingToken);
        }        

        private Task CalculateSubredditPostStats(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _postListUpdatedSignal.WaitOne();

                    UpdateSubredditPostsStats();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error calculating subreddit post stats");
                }
            }

            return Task.CompletedTask;
        }

        private void UpdateSubredditPostsStats()
        {
            // top posts
            // sort all posts by upvotes (or vote difference) and add to TopPosts
            _store.SetTopPosters(_store.AllPostInfos.OrderByDescending(p => p.Score).ToArray());

            // most posters
            var postCountsByUsername = new Dictionary<string, int>();
            foreach (var post in new PostInfo.List(_store.AllPostInfos))
            {
                postCountsByUsername[post.Author] = postCountsByUsername.GetValueOrDefault(post.Author, 0) + 1;
            }

            var mostPosters = new MostPosterInfo.List();
            foreach (var kvp in postCountsByUsername)
            {
                mostPosters.Add(new MostPosterInfo(kvp.Key, kvp.Value, _store.Subreddit));
            }

            _store.SetMostPosters(mostPosters.OrderByDescending(mp => mp.PostCount).ToArray());           
        }
    }
}
