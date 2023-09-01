﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Shared
{
    public interface ISubredditPostStatsSource
    {
        MostPosterInfo[] MostPosters { get; }
        TopPostInfo[] TopPosts { get; }
    }
}