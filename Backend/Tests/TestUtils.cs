using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using SubredditStats.Backend.WebApi.Controllers;
using SubredditStats.Shared.Model;
using SubredditStats.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using SubredditStats.Shared.Client;
using SubredditStats.Backend.WebApi;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    internal class TestUtils
    {
        internal static SubredditStatsController CreateControllerWithMockSource(int responseItemCount)
        {
            var mockSource = CreateMockSubredditPostStatsSource(responseItemCount);
            return new SubredditStatsController(mockSource.Object);
        }

        internal static Mock<ISubredditPostStatsSource> CreateMockSubredditPostStatsSource(int responseItemCount)
        {
            var mockRepository = new Mock<ISubredditPostStatsSource>();

            mockRepository.Setup(x => x.AllPostInfos)
                .Returns(PostInfo.List.CreateRandom(responseItemCount).ToArray());
            mockRepository.Setup(x => x.TopPosts)
                .Returns(PostInfo.List.CreateRandom(responseItemCount).ToArray());
            mockRepository.Setup(x => x.MostPosters)
                .Returns(MostPosterInfo.List.CreateRandom(responseItemCount).ToArray());

            mockRepository.Setup(x => x.GetNumberOfAllPostInfos(responseItemCount))
                .Returns(PostInfo.List.CreateRandom(responseItemCount).ToArray());
            mockRepository.Setup(x => x.GetNumberOfTopPosts(responseItemCount))
                .Returns(PostInfo.List.CreateRandom(responseItemCount).ToArray());
            mockRepository.Setup(x => x.GetNumberOfMostPosters(responseItemCount))
                .Returns(MostPosterInfo.List.CreateRandom(responseItemCount).ToArray());

            return mockRepository;
        }

        internal static ISubredditPostStatsClient CreateSubRedditPostStatsApiClient(HttpClient httpClient)
        {
            return new SubRedditPostStatsApiClient(httpClient);
        }

        internal static ISubredditPostStatsClient CreateSubRedditPostStatsApiClient(WebApplicationFactory<Program> webAppFactory, int responseObjectCount)
        {
            var httpClient = webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<ISubredditPostStatsSource>(CreateMockSubredditPostStatsSource(100).Object);
                });

            }).CreateClient();
            return CreateSubRedditPostStatsApiClient(httpClient);
        }       
    }
}
