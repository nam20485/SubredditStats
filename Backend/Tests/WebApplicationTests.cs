using Xunit;
using FluentAssertions;
using FluentAssertions.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using SubredditStats.Backend.WebApi;
using System.Net.Http.Json;
using SubredditStats.Shared.Model;
using SubredditStats.Backend.Lib.RedditApi;
using SubredditStats.Shared.Client;
using System.Net;
using SubredditStats.Shared.Utils;

namespace Tests
{
    public class WebApplicationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _webApplicationFactory;

        public WebApplicationTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }
     
        [Theory]
        [InlineData(SubRedditPostStatsClient.AllPostsEndpoint)]
        [InlineData(SubRedditPostStatsClient.TopPostsEndpoint)]
        [InlineData(SubRedditPostStatsClient.MostPostersEndpoint)]
        public async Task Test_Endpoints_Return_Http200(string uri)
        {
            using var httpClient = _webApplicationFactory.CreateClient();

            using var response = await httpClient.GetAsync(uri);

            response.Should().NotBeNull();
            response.Should().BeSuccessful();
            response.Should().Be200Ok();
        }

        [Theory]
        [InlineData(SubRedditPostStatsClient.AllPostsEndpoint, typeof(PostInfo[]))]
        [InlineData(SubRedditPostStatsClient.TopPostsEndpoint, typeof(PostInfo[]))]
        [InlineData(SubRedditPostStatsClient.MostPostersEndpoint, typeof(MostPosterInfo[]))]
        public async Task Test_EndpointResponsesShouldDeserializeIntoCorrectTypes(string uri, Type responseType)
        {
            using var httpClient = _webApplicationFactory.CreateClient();

            using var response = await httpClient.GetAsync(uri);

            response.Should().NotBeNull();
            response.Should().BeSuccessful();
            response.Should().Be200Ok();

            var responseContent = response.Content;
            responseContent.Should().NotBeNull();

            var result = await responseContent.ReadFromJsonAsync(responseType, GlobalJsonSerializerOptions.Options);
            result.Should().NotBeNull();
            result.Should().BeOfType(responseType);
        }

        // inject a fake service into the controller that WebApplicaitonFactory uses
        //// Arrange
        //var client = _factory.WithWebHostBuilder(builder =>
        //{
        //    builder.ConfigureTestServices(services =>
        //    {
        //        services.AddScoped<IQuoteService, TestQuoteService>();
        //    });
        //})
        //    .CreateClient();

    }
}
