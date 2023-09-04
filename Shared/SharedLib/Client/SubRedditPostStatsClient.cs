using System.Net.Http.Json;
using System.Text.Json;

using SubredditStats.Shared.Model;

namespace SubredditStats.Shared.Client
{
    public class SubRedditPostStatsClient : ISubredditPostStatsClient, IDisposable
    {
        //https://localhost:7199/api/SubredditStats/all_posts/{count}
        
        private const string AllPostsEndpoint = "api/SubredditStats/all_posts";
        private const string TopPostsEndpoint = "api/SubredditStats/top_posts";
        private const string MostPostersEndpoint = "api/SubredditStats/most_posters";
        private const string VerifyEndpoint = AllPostsEndpoint + "/0";

        private readonly HttpClient _httpClient;

        public SubRedditPostStatsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public MostPosterInfo[] MostPosters => FetchRequest<MostPosterInfo>(MostPostersEndpoint);       
        public PostInfo[] TopPosts => FetchRequest<PostInfo>(TopPostsEndpoint);
        public PostInfo[] AllPostInfos => FetchRequest<PostInfo>(AllPostsEndpoint);

        public PostInfo[] GetNumberOfAllPostInfos(int count) => FetchRequest<PostInfo>($"{AllPostsEndpoint}/{count}");
        public MostPosterInfo[] GetNumberOfMostPosters(int count) => FetchRequest<MostPosterInfo>($"{MostPostersEndpoint}/{count}");
        public PostInfo[] GetNumberOfTopPosts(int count) => FetchRequest<PostInfo>($"{TopPostsEndpoint}/{count}");

        public bool VerifyConnection()
        {
            return _httpClient.Send(new HttpRequestMessage()
            {
                RequestUri = new Uri(VerifyEndpoint, UriKind.Relative),
                Method = HttpMethod.Get

            }).IsSuccessStatusCode;
        }

        private TResponse[] FetchRequest<TResponse>(string uri)
        {
            var response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, new Uri(uri, UriKind.Relative)));
            if (response.IsSuccessStatusCode)
            {
                if (JsonSerializer.Deserialize<TResponse[]>(response.Content.ReadAsStream(), new JsonSerializerOptions(JsonSerializerDefaults.Web)) is TResponse[] responseValues)
                {
                    return responseValues;
                }
            }

            return Array.Empty<TResponse>();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
