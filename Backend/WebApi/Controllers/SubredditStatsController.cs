using System.Reflection.Metadata.Ecma335;

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
        private readonly ISubredditPostStatsStore _store;

        public SubredditStatsController(ISubredditPostStatsStore store)
        {
            _store = store;
        }

        [HttpGet("top_posts/{count:int}")]
        public ActionResult<IEnumerable<PostInfo>> GetTopPosts([FromRoute] RequestData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_store.TopPosts.Take(data.Count));
        }

        [HttpGet("top_posts")]
        public ActionResult<IEnumerable<PostInfo>> GetTopPosts()
        {
            return _store.TopPosts;
        }

        [HttpGet("most_posters/{count}")]
        public ActionResult<IEnumerable<MostPosterInfo>> GetMostPosters([FromRoute] RequestData data)
        {
            if (! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_store.MostPosters.Take(data.Count));
        }

        [HttpGet("most_posters")]
        public IEnumerable<MostPosterInfo> GetMostPosters()
        {
            return _store.MostPosters;
        }

        [HttpGet("all_posts/{count}")]
        public ActionResult<IEnumerable<PostInfo>> GetAllPosts([FromRoute] RequestData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_store.AllPostInfos.Take(data.Count));
        }

        [HttpGet("all_posts")]
        public IEnumerable<PostInfo> GetAllPosts()
        {
            return _store.AllPostInfos;
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
