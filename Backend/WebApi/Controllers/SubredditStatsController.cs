using Microsoft.AspNetCore.Mvc;

using SubredditStats.Backend.Lib.Store;
using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubredditStatsController : ControllerBase
    {        
        private readonly ISubredditPostStatsSource _source;

        public SubredditStatsController(ISubredditPostStatsSource source)
        {
            _source = source;
        }

        [HttpGet("top_posts/{Count:int}")]
        public ActionResult<PostInfo[]> GetNumberOfTopPosts([FromRoute] RequestData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_source.GetNumberOfTopPosts(data.Count));
        }

        [HttpGet("top_posts")]
        public PostInfo[] GetTopPosts()
        {
            return _source.TopPosts;
        }

        [HttpGet("most_posters/{Count:int}")]
        public ActionResult<MostPosterInfo[]> GetNumberOfMostPosters([FromRoute] RequestData data)
        {
            if (! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_source.GetNumberOfMostPosters(data.Count));
        }

        [HttpGet("most_posters")]
        public MostPosterInfo[] GetMostPosters()
        {
            return _source.MostPosters;
        }

        [HttpGet("all_posts/{Count:int}")]
        public ActionResult<PostInfo[]> GetNumberOfAllPosts([FromRoute] RequestData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_source.GetNumberOfAllPostInfos(data.Count));
        }

        [HttpGet("all_posts")]
        public PostInfo[] GetAllPosts()
        {
            return _source.AllPostInfos;
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
