using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public class DatabaseBackingStore : ISubredditPostsStatsStore
    {
        // use Entity Framework to store the data in a database

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
