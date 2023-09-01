using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SubredditStats.Backend.Lib;
using SubredditStats.Backend.Lib.RedditApi;
using SubredditStats.Backend.Lib.Store;
using SubredditStats.Backend.WebApi.Services;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubredditStatsController : ControllerBase
    {        
        private readonly ISubredditPostsStatsStore _store;

        public SubredditStatsController(ISubredditPostsStatsStore store)
        {
            _store = store;
        }

        [HttpGet("top_post")]
        public TopPostInfo[] GetTopPosts()
        {
            return _store.TopPosts;
        }

        [HttpGet("most_posters")]
        public MostPosterInfo[] GetMostPosters()
        {
            return _store.MostPosters;
        }

        //[HttpGet("top_posts/{subreddit}")]
        //public async Task<ActionResult<RedditPostListing?>> GetSubredditTopPosts([FromRoute] string subreddit)
        //{
        //    return await _redditApiClient.GetSubredditTopPosts(subreddit);
        //}

        //[HttpGet("new_posts/{subreddit}")]
        //public async Task<ActionResult<RedditPostListing?>> GetSubredditNewPosts([FromRoute] string subreddit)
        //{
        //    return await _redditApiClient.GetSubredditNewPosts(subreddit);
        //}

        //[HttpGet("posts/{subreddit}/{sort}")]
        //public async Task<ActionResult<RedditPostListing?>> GetSubredditNewPosts([FromRoute] string subreddit, [FromRoute] RedditStatsApiClient.PostListingSortType sort)
        //{
        //    return await _redditApiClient.FetchSubredditPostListing(subreddit, sort);
        //}
    }
}
