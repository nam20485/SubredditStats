using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public interface ISubredditPostsStatsStore : ISubredditPostStatsSource
    {
        void AddTopPosts(IEnumerable<TopPostInfo> topPosts);
        void AddMostPosters(IEnumerable<MostPosterInfo> mostPosters);
    }
}
