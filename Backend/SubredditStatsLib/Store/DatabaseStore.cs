using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public class DatabaseStore : ISubredditPostsStatsStore
    {
        // e.g. use Entity Framework to store and fetch the data from a database

        public MostPosterInfo[] MostPosters => throw new NotImplementedException();

        public TopPostInfo[] TopPosts => throw new NotImplementedException();

        public void AddMostPosters(IEnumerable<MostPosterInfo> mostPosters)
        {
            throw new NotImplementedException();
        }

        public void AddTopPosts(IEnumerable<TopPostInfo> topPosts)
        {
            throw new NotImplementedException();
        }
    }
}
