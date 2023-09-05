using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using SubredditStats.Backend.Lib.Store;
using SubredditStats.Backend.WebApi.Controllers;
using SubredditStats.Shared;
using SubredditStats.Shared.Model;

using Xunit;

namespace Tests
{
    public class SubredditStatsControllerTests
    {
        private const int RandomResponseObjectCount = 100;

        [Fact]
        public void Test_ControllerActions_GetAllPosts()
        {
            var controller = CreateControllerWithMockSource();

            var allPosts = controller.GetAllPosts();
            allPosts.Should().NotBeNull();
            allPosts.Should().NotBeEmpty();
            allPosts.Should().HaveCount(RandomResponseObjectCount);            
            allPosts.Should().BeOfType(typeof(PostInfo[]));
        }

        [Fact]
        public void Test_ControllerActions_GetTopPosts()
        {
            var controller = CreateControllerWithMockSource();

            var topPosts = controller.GetTopPosts();
            topPosts.Should().NotBeNull();
            topPosts.Should().NotBeEmpty();
            topPosts.Should().HaveCount(RandomResponseObjectCount);
            topPosts.Should().BeOfType(typeof(PostInfo[]));
        }

        [Fact]
        public void Test_ControllerActions_GetMostPosters()
        {
            var controller = CreateControllerWithMockSource();

            var mostPosters = controller.GetMostPosters();
            mostPosters.Should().NotBeNull();
            mostPosters.Should().NotBeEmpty();
            mostPosters.Should().HaveCount(RandomResponseObjectCount);
            mostPosters.Should().BeOfType(typeof(MostPosterInfo[]));
        }

        [Fact]
        public void Test_ControllerActions_GetNumberOfAllPosts()
        {
            var controller = CreateControllerWithMockSource();

            var actionResult = controller.GetNumberOfAllPosts(new RequestData
            {
                Count = RandomResponseObjectCount
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
            resultValue.Should().HaveCount(RandomResponseObjectCount);
        }

        [Fact]
        public void Test_ControllerActions_GetNumberOfTopPosts()
        {
            var controller = CreateControllerWithMockSource();

            var actionResult = controller.GetNumberOfTopPosts(new RequestData
            {
                Count = RandomResponseObjectCount
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
            resultValue.Should().HaveCount(RandomResponseObjectCount);
        }

        [Fact]
        public void Test_ControllerActions_GetNumberOfMostPosters()
        {
            var controller = CreateControllerWithMockSource();

            var actionResult = controller.GetNumberOfMostPosters(new RequestData
            {
                Count = RandomResponseObjectCount
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
            resultValue.Should().HaveCount(RandomResponseObjectCount);            
        }

        private static SubredditStatsController CreateControllerWithMockSource()
        {
            var mockSource = CreateMockSubredditPostStatsSource();
            return new SubredditStatsController(mockSource.Object);
        }

        private static Mock<ISubredditPostStatsSource> CreateMockSubredditPostStatsSource()
        {
            var mockRepository = new Mock<ISubredditPostStatsSource>();

            mockRepository.Setup(x => x.AllPostInfos)
                .Returns(PostInfo.List.CreateRandom(RandomResponseObjectCount).ToArray());
            mockRepository.Setup(x => x.TopPosts)
                .Returns(PostInfo.List.CreateRandom(RandomResponseObjectCount).ToArray());
            mockRepository.Setup(x => x.MostPosters)
                .Returns(MostPosterInfo.List.CreateRandom(RandomResponseObjectCount).ToArray());

            mockRepository.Setup(x => x.GetNumberOfAllPostInfos(RandomResponseObjectCount))
                .Returns(PostInfo.List.CreateRandom(RandomResponseObjectCount).ToArray());
            mockRepository.Setup(x => x.GetNumberOfTopPosts(RandomResponseObjectCount))
                .Returns(PostInfo.List.CreateRandom(RandomResponseObjectCount).ToArray());
            mockRepository.Setup(x => x.GetNumberOfMostPosters(RandomResponseObjectCount))
                .Returns(MostPosterInfo.List.CreateRandom(RandomResponseObjectCount).ToArray());

            return mockRepository;
        }
    }
}
