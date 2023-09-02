using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Shared
{
    public interface ISubredditPostStatsSource
    {
        delegate void PostListUpdatedHandler(ISubredditPostStatsSource sender);

        public event PostListUpdatedHandler? PostListUpdated;        

        MostPosterInfo[] MostPosters { get; }
        PostInfo[] TopPosts { get; }
        PostInfo[] AllPostInfos { get; }

        DateTime? Started { get; set; }
        string? Subreddit { get; set; }
    }
}
