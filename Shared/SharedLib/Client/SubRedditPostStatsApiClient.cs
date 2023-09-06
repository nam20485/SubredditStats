using System.Text.Json;

using SubredditStats.Shared.Model;
using SubredditStats.Shared.Utils;

namespace SubredditStats.Shared.Client
{
    public class SubRedditPostStatsApiClient : ISubredditPostStatsClient, IDisposable
    {
        //https://localhost:7199/api/SubredditStats/all_posts/{count}
        
        public const string AllPostsEndpoint = "api/SubredditStats/all_posts";
        public const string TopPostsEndpoint = "api/SubredditStats/top_posts";
        public const string MostPostersEndpoint = "api/SubredditStats/most_posters";
        private const string VerifyEndpoint = AllPostsEndpoint + "/0";

        private readonly HttpClient _httpClient;

        public SubRedditPostStatsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public MostPosterInfo[] MostPosters => FetchRequest<MostPosterInfo>(MostPostersEndpoint);       
        public PostInfo[] TopPosts => FetchRequest<PostInfo>(TopPostsEndpoint);
        public PostInfo[] AllPostInfos => FetchRequest<PostInfo>(AllPostsEndpoint);

        public PostInfo[] GetNumberOfAllPostInfos(int count) => FetchRequest<PostInfo>($"{AllPostsEndpoint}/{count}");
        public MostPosterInfo[] GetNumberOfMostPosters(int count) => FetchRequest<MostPosterInfo>($"{MostPostersEndpoint}/{count}");
        public PostInfo[] GetNumberOfTopPosts(int count) => FetchRequest<PostInfo>($"{TopPostsEndpoint}/{count}");

        public bool VerifyConnection(out string message)
        {
            try
            {
                message = "Success";

                using var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(VerifyEndpoint, UriKind.Relative),
                    Method = HttpMethod.Get

                };
                using var response = _httpClient.Send(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                message = e.ToString();
                return false;
            }
        }

        private TResponse[] FetchRequest<TResponse>(string uri)
        {
            using var response = _httpClient.Send(new HttpRequestMessage(HttpMethod.Get, new Uri(uri, UriKind.Relative)));
            if (response.IsSuccessStatusCode)
            {
                if (DeserializeValue<TResponse[]>(response) is TResponse[] responseValues)
                {
                    return responseValues;
                }
            }

            return Array.Empty<TResponse>();
        }

        private static TValue? DeserializeValue<TValue>(HttpResponseMessage response)
        {
            return JsonSerializer.Deserialize<TValue>(response.Content.ReadAsStream(), GlobalJsonSerializerOptions.Options);
        }

        public void Dispose()
        {
            _httpClient.Dispose();            

            GC.SuppressFinalize(this);
        }
    }
}
