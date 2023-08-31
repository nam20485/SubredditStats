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

        [HttpGet("mostupvotedposts/{subreddit}")]
        public async Task<ActionResult<RedditPostListing?>> GetMostUpvotedPostsAsync([FromRoute] string subreddit)
        {
            return await _redditApiClient.GetMostUpvotedPosts(subreddit);
        }
    }
}
