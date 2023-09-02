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
    public class SubredditPostStatsCalculator : BackgroundService
    {
        private readonly IRedditStatsClient _apiClient;
        private readonly ISubredditPostStatsStore _backingStore;
        private readonly IConfiguration? _config;

        private readonly DateTime _started;        

        public string Subreddit { get; }

        public MostPosterInfo[] MostPosters => _backingStore.MostPosters;
        public PostInfo[] TopPosts => _backingStore.TopPosts;

        public SubredditPostStatsCalculator(ISubredditPostStatsStore store,
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
        }
      
        public async Task CalculateStatsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var postInfos = new PostInfo.List();

                const int sliceLimit = 100;
                var after = "";
                var count = 0;
                do
                {
                    var postListings = await _apiClient.FetchSubredditPostListing(Subreddit,
                                                                                  RedditStatsApiClient.PostListingSortType.unspecified,
                                                                                  sliceLimit,
                                                                                  after,
                                                                                  count);
                    if (postListings is not null)
                    {                        
                        foreach (var post in postListings.data.children)
                        {
                            postInfos.Add(new PostInfo()
                            {
                                PostTitle = post.data.title,
                                Subreddit = post.data.subreddit,
                                Author = post.data.author,
                                UpVotes = post.data.ups,
                                PostUrl = post.data.url,
                                ApiName = post.data.name
                            });
                        }

                        after = postListings.data.after;
                        count = postInfos.Count;              
                    }
                } while (!string.IsNullOrWhiteSpace(after));

                _backingStore.AddPostInfos(postInfos);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CalculateStatsAsync(stoppingToken);
        }
    }
}