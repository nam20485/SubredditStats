using Xunit;
using FluentAssertions;
using FluentAssertions.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using SubredditStats.Backend.WebApi;

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
        [InlineData("https://localhost:5001/api/subredditstats/top_posts/10")]
        public void Test_Endpoints(string uri)
        {

        }
    }
}
