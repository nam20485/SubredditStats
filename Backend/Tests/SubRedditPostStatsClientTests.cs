using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using SubredditStats.Backend.WebApi;
using SubredditStats.Shared.Client;
using SubredditStats.Shared.Model;

using Xunit;

namespace Tests
{
    public class SubRedditPostStatsClientTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const int ResponseObjectCount = 100;

        private readonly WebApplicationFactory<Program> _webApplicationFactory;

        public SubRedditPostStatsClientTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        //[Fact]
        //public void ApiClient_AllPostInfos_ShouldResponseSuccessfully()
        //{
        //    var subredditStatsClient = TestUtils.CreateSubRedditPostStatsApiClient(_webApplicationFactory, ResponseObjectCount);
        //    var allPosts = subredditStatsClient.AllPostInfos;
        //    allPosts.Should().NotBeNull();
        //    allPosts.Should().NotBeEmpty();
        //    allPosts.Should().HaveCount(ResponseObjectCount);
        //    allPosts.Should().BeOfType(typeof(PostInfo[]));
        //}         
    }
}
