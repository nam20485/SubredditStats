﻿using System;
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
        void AddPostInfos(IEnumerable<PostInfo> postInfos);
        void SetMostPosters(IEnumerable<MostPosterInfo> mostPosters);        
        void SetTopPosters(IEnumerable<PostInfo> topPostInfos);
    }
}
