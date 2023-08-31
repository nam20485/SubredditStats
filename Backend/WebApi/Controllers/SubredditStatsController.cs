using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubredditStats.Backend.Lib;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubredditStatsController : ControllerBase
    {
        private readonly RedditStatsApiClient _redditApiClient;

        public SubredditStatsController()
        {
            _redditApiClient = new RedditStatsApiClient();
        }

        [HttpGet("top_posts/{subreddit}")]
        public async Task<ActionResult<RedditPostListing?>> GetSubredditTopPosts([FromRoute] string subreddit)
        {
            return await _redditApiClient.GetSubredditTopPosts(subreddit);
        }

        [HttpGet("new_posts/{subreddit}")]
        public async Task<ActionResult<RedditPostListing?>> GetSubredditNewPosts([FromRoute] string subreddit)
        {
            return await _redditApiClient.GetSubredditNewPosts(subreddit);
        }

        [HttpGet("posts/{subreddit}/{sort}")]
        public async Task<ActionResult<RedditPostListing?>> GetSubredditNewPosts([FromRoute] string subreddit, [FromRoute] RedditStatsApiClient.PostListingSortType sort)
        {
            return await _redditApiClient.FetchSubredditPostListing(subreddit, sort);
        }
    }
}
