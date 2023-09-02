using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public interface ISubredditPostStatsStore : ISubredditPostStatsSource
    {        
        void AddMostPosters(IEnumerable<MostPosterInfo> mostPosters);
        void AddPostInfos(IEnumerable<PostInfo> postInfos);
        void AddTopPosters(IEnumerable<PostInfo> topPostInfos);
    }
}
