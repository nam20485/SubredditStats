using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public class DatabaseStore : ISubredditPostStatsStore
    {
        // e.g. use Entity Framework to store and fetch the data from a database

        public DateTime? Started { get; set; }

        public MostPosterInfo[] MostPosters => throw new NotImplementedException();

        public PostInfo[] TopPosts => throw new NotImplementedException();

        public PostInfo[] AllPostInfos => throw new NotImplementedException();

        public void AddMostPosters(IEnumerable<MostPosterInfo> mostPosters)
        {
            throw new NotImplementedException();
        }     

        public void AddPostInfos(IEnumerable<PostInfo> postInfos)
        {
            throw new NotImplementedException();
        }

        public void AddTopPosters(IEnumerable<PostInfo> topPostInfos)
        {
            throw new NotImplementedException();
        }
    }
}
