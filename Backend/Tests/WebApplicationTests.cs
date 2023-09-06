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
        [InlineData(SubRedditPostStatsApiClient.AllPostsEndpoint)]
        [InlineData(SubRedditPostStatsApiClient.TopPostsEndpoint)]
        [InlineData(SubRedditPostStatsApiClient.MostPostersEndpoint)]
        public async Task Endpoints_ShouldReturnHttp200(string uri)
        {
            using var httpClient = _webApplicationFactory.CreateClient();

            using var response = await httpClient.GetAsync(uri);

            response.Should().NotBeNull();
            response.Should().BeSuccessful();
            response.Should().Be200Ok();
        }

        [Theory]
        [InlineData(SubRedditPostStatsApiClient.AllPostsEndpoint, typeof(PostInfo[]))]
        [InlineData(SubRedditPostStatsApiClient.TopPostsEndpoint, typeof(PostInfo[]))]
        [InlineData(SubRedditPostStatsApiClient.MostPostersEndpoint, typeof(MostPosterInfo[]))]
        public async Task EndpointResponses_ShouldDeserializeIntoCorrectTypes(string uri, Type responseType)
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

        [Fact]
        public async Task Endpoints_Swagger_ShouldReturnHttp200()
        {
            var uri = "swagger/index.html";

            using var httpClient = _webApplicationFactory.CreateClient();

            using var response = await httpClient.GetAsync(uri);

            response.Should().NotBeNull();
            response.Should().BeSuccessful();
            response.Should().Be200Ok();
        }


        [Fact]
        public async Task Endpoints_Redoc_ShouldReturnHttp200()
        {
            var uri = "redoc/index.html";

            using var httpClient = _webApplicationFactory.CreateClient();

            using var response = await httpClient.GetAsync(uri);

            response.Should().NotBeNull();
            response.Should().BeSuccessful();
            response.Should().Be200Ok();
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
