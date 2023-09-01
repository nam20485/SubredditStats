using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Backend.Lib.RedditApi;
using SubredditStats.Backend.Lib.Store;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.WebApi.Services
{
    public class SubredditPostsStatsCalculator : BackgroundService
    {
        private readonly IRedditStatsClient _apiClient;
        private readonly ISubredditPostsStatsStore _backingStore;
        private readonly IConfiguration? _config;

        private readonly DateTime _started;
        private readonly CancellationToken _cancel;

        public string Subreddit { get; }

        public MostPosterInfo[] MostPosters => _backingStore.MostPosters;
        public TopPostInfo[] TopPosts => _backingStore.TopPosts;

        public SubredditPostsStatsCalculator(ISubredditPostsStatsStore store,
                                             IRedditStatsClient apiClient,
                                             IConfiguration config)
        {
            _backingStore = store;
            _apiClient = apiClient;
            _config = config;

            Subreddit = _config?["SubredditName"];
            if (string.IsNullOrWhiteSpace(Subreddit))
            {
                throw new InvalidOperationException("Subreddit cannot be null or whitespace (value not found in appsettings.json?)");
            }

            _started = DateTime.UtcNow;
            _cancel = new CancellationToken();
        }
      
        public async Task CalculateStatsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {               
                var topPostListings = await _apiClient.GetSubredditPostsSortedByTop(Subreddit);
                if (topPostListings is not null)
                {
                    var topPostInfos = new TopPostInfo.List();

                    foreach (var post in topPostListings.data.children)
                    {
                        topPostInfos.Add(new TopPostInfo()
                        {
                            PostTitle = post.data.title,
                            Subreddit = post.data.subreddit,
                            Author = post.data.author,
                            UpVotes = post.data.ups,
                            PostUrl = post.data.url
                        });
                    }

                    _backingStore.AddTopPosts(topPostInfos);
                }                
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CalculateStatsAsync(stoppingToken);
        }
    }
}