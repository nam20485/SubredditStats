using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using FluentAssertions;
using Xunit;
using Moq;

using SubredditStats.Backend.Lib.Store;
using SubredditStats.Backend.WebApi.Controllers;
using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace Tests
{
    public class SubredditStatsControllerTests
    {
        private const int ResponseObjectCount = 100;

        [Fact]
        public void ControllerActions_ShouldRespondSuccessfully_GetAllPosts()
        {
            var controller = TestUtils.CreateControllerWithMockSource(ResponseObjectCount);

            var allPosts = controller.GetAllPosts();
            allPosts.Should().NotBeNull();
            allPosts.Should().NotBeEmpty();
            allPosts.Should().HaveCount(ResponseObjectCount);            
            allPosts.Should().BeOfType(typeof(PostInfo[]));
        }

        [Fact]
        public void ControllerActions_ShouldRespondSuccessfully_GetTopPosts()
        {
            var controller = TestUtils.CreateControllerWithMockSource(ResponseObjectCount);

            var topPosts = controller.GetTopPosts();
            topPosts.Should().NotBeNull();
            topPosts.Should().NotBeEmpty();
            topPosts.Should().HaveCount(ResponseObjectCount);
            topPosts.Should().BeOfType(typeof(PostInfo[]));
        }

        [Fact]
        public void ControllerActions_ShouldRespondSuccessfully_GetMostPosters()
        {
            var controller = TestUtils.CreateControllerWithMockSource(ResponseObjectCount);

            var mostPosters = controller.GetMostPosters();
            mostPosters.Should().NotBeNull();
            mostPosters.Should().NotBeEmpty();
            mostPosters.Should().HaveCount(ResponseObjectCount);
            mostPosters.Should().BeOfType(typeof(MostPosterInfo[]));
        }

        [Fact]
        public void ControllerActions_ShouldRespondSuccessfully_GetNumberOfAllPosts()
        {
            var controller = TestUtils.CreateControllerWithMockSource(ResponseObjectCount);

            var actionResult = controller.GetNumberOfAllPosts(new RequestData
            {
                Count = ResponseObjectCount
            });

            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<ActionResult<PostInfo[]>>();
            actionResult.Result.Should().NotBeNull();

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var objectResult = actionResult.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.Value.Should().NotBeNull();

            objectResult!.Value.Should().BeOfType<PostInfo[]>();
            var resultValue = objectResult.Value as PostInfo[];
            resultValue.Should().NotBeNull();
            resultValue.Should().NotBeEmpty();
            resultValue.Should().HaveCount(ResponseObjectCount);
        }

        [Fact]
        public void ControllerActions_ShouldRespondSuccessfully_GetNumberOfTopPosts()
        {
            var controller = TestUtils.CreateControllerWithMockSource(ResponseObjectCount);

            var actionResult = controller.GetNumberOfTopPosts(new RequestData
            {
                Count = ResponseObjectCount
            });

            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<ActionResult<PostInfo[]>>();
            actionResult.Result.Should().NotBeNull();

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var objectResult = actionResult.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.Value.Should().NotBeNull();

            objectResult!.Value.Should().BeOfType<PostInfo[]>();
            var resultValue = objectResult.Value as PostInfo[];
            resultValue.Should().NotBeNull();
            resultValue.Should().NotBeEmpty();
            resultValue.Should().HaveCount(ResponseObjectCount);
        }

        [Fact]
        public void ControllerActions_ShouldRespondSuccessfully_GetNumberOfMostPosters()
        {
            var controller = TestUtils.CreateControllerWithMockSource(ResponseObjectCount);

            var actionResult = controller.GetNumberOfMostPosters(new RequestData
            {
                Count = ResponseObjectCount
            });

            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<ActionResult<MostPosterInfo[]>>();
            actionResult.Result.Should().NotBeNull();

            actionResult.Result.Should().BeOfType<OkObjectResult>();            
            var objectResult = actionResult.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.Value.Should().NotBeNull();

            objectResult!.Value.Should().BeOfType<MostPosterInfo[]>();            
            var resultValue = objectResult.Value as MostPosterInfo[];
            resultValue.Should().NotBeNull();
            resultValue.Should().NotBeEmpty();
            resultValue.Should().HaveCount(ResponseObjectCount);            
        }        
    }
}
