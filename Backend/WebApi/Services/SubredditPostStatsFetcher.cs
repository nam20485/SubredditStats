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
    public class SubredditPostStatsFetcher : BackgroundService
    {
        private const int PostListingSliceLimit = 100;

        private readonly IRedditStatsClient _apiClient;
        private readonly ISubredditPostStatsStore _backingStore;
        private readonly IConfiguration? _config;
        private readonly ILogger<SubredditPostStatsFetcher> _logger;

        public string Subreddit { get; }

        public SubredditPostStatsFetcher(ISubredditPostStatsStore store,
                                            IRedditStatsClient apiClient,
                                            IConfiguration config,
                                            ILogger<SubredditPostStatsFetcher> logger)
        {
            _backingStore = store;
            _apiClient = apiClient;
            _config = config;
            _logger = logger;

            Subreddit = _config?["SubredditName"];
            if (string.IsNullOrWhiteSpace(Subreddit))
            {
                throw new InvalidOperationException("Subreddit cannot be null or whitespace (value not found in appsettings.json?)");
            }

            _backingStore.Started = DateTime.UtcNow;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await FetchSubredditPosts(stoppingToken);
        }

        private async Task FetchSubredditPosts(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // wrap in general try/catch to prevent service from stopping
                // if any type of exception is encountered on any given iteration 
                // of fetching subreddit posts/slices
                try
                {
                    var postInfos = new PostInfo.List();

                    var after = "";
                    var count = 0;
                    do
                    {
                        var postListingSlice = await _apiClient.FetchSubredditPostListingSlice(Subreddit,
                                                                                          RedditStatsApiClient.PostListingSortType.unspecified,
                                                                                          PostListingSliceLimit,
                                                                                          after,
                                                                                          count);
                        if (postListingSlice is not null)
                        {
                            foreach (var post in postListingSlice.data.children)
                            {
                                var epoch = Convert.ToInt32(post.data.created_utc);
                                var createdDT = DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime.ToUniversalTime();

                                postInfos.Add(new PostInfo(
                                    post.data.title,
                                    post.data.ups,
                                    post.data.downs,
                                    post.data.score,
                                    post.data.url,
                                    post.data.subreddit,
                                    post.data.author,
                                    post.data.name,
                                    createdDT,
                                    DateTime.UtcNow));
                            }

                            after = postListingSlice.data.after;
                            count += postListingSlice.data.dist;
                        }
                    } while (!string.IsNullOrWhiteSpace(after) &&
                             !stoppingToken.IsCancellationRequested);

                    _backingStore.AddPostInfos(postInfos);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "{ExceptionMessage}", "Exception while fetching subreddit posts");
                }
            }
        }        
    }
}