using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Shared.Client
{
    public class SubRedditPostStatsClient : ISubredditPostStatsClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        public SubRedditPostStatsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public MostPosterInfo[] MostPosters => throw new NotImplementedException();

        public PostInfo[] TopPosts => throw new NotImplementedException();

        public PostInfo[] AllPostInfos => throw new NotImplementedException();       

        public PostInfo[] GetNumberOfAllPostInfos(int count)
        {
            throw new NotImplementedException();
        }

        public MostPosterInfo[] GetNumberOfMostPosters(int count)
        {
            throw new NotImplementedException();
        }

        public PostInfo[] GetNumberOfTopPosts(int count)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
