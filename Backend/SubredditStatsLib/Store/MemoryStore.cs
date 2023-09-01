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
        private readonly MostPosterInfo.StringDictionary _mostPosterInfosByUsername;
        private readonly TopPostInfo.StringDictionary _topPostInfosByApiName;

        // basic concurrency synchronization
        private readonly object _mostPostersLock;
        private readonly object _topPostsLock;

        public MemoryStore()
        {
            _mostPostersLock = new();
            _topPostsLock = new();

            _mostPosterInfosByUsername = new();
            _topPostInfosByApiName = new();
        }

        public MostPosterInfo[] MostPosters
        {
            get
            {
                lock (_mostPostersLock)
                {
                    return _mostPosterInfosByUsername.Values.ToArray();
                }
            }
        }

        public TopPostInfo[] TopPosts
        {
            get
            {
                lock (_topPostsLock)
                {
                    return _topPostInfosByApiName.Values.ToArray();
                }
            }
        }

        public void AddMostPosters(IEnumerable<MostPosterInfo> mostPosters)
        {
            lock (_mostPostersLock)
            {
                foreach (var mostPoster in mostPosters)
                {
                    _mostPosterInfosByUsername[mostPoster.Username] = mostPoster;
                }
            }
        }

        public void AddTopPosts(IEnumerable<TopPostInfo> topPosts)
        {
            lock (_topPostsLock)
            {
                foreach (var topPost in topPosts)
                {
                    _topPostInfosByApiName[topPost.ApiName] = topPost;
                }
            }
        }
    }
}
