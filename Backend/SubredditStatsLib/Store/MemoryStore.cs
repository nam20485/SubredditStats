using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public class MemoryStore : ISubredditPostsStatsStore
    {
        private readonly MostPosterInfo.List _mostPosterInfos;
        private readonly TopPostInfo.List _topPostInfos;

        private readonly object _mostPostersLock;
        private readonly object _topPostsLock;

        public MemoryStore()
        {
            _mostPostersLock = new();
            _topPostsLock = new();

            _mostPosterInfos = new();
            _topPostInfos = new();
        }

        public MostPosterInfo[] MostPosters
        {
            get
            {
                lock (_mostPostersLock)
                {
                    return _mostPosterInfos.ToArray();
                }
            }
        }

        public TopPostInfo[] TopPosts
        {
            get
            {
                lock (_topPostsLock)
                {
                    return _topPostInfos.ToArray();
                }
            }
        }

        public void AddMostPosters(IEnumerable<MostPosterInfo> mostPosters)
        {
            lock (_mostPostersLock)
            {
                _mostPosterInfos.AddRange(mostPosters);
            }
        }

        public void AddTopPosts(IEnumerable<TopPostInfo> topPosts)
        {
            lock (_topPostsLock)
            {
                _topPostInfos.AddRange(topPosts);
            }
        }
    }
}
