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
        MostPosterInfo[] MostPosters { get; }
        PostInfo[] TopPosts { get; }
        PostInfo[] AllPostInfos { get; }              

        MostPosterInfo[] GetNumberOfMostPosters(int count);
        PostInfo[] GetNumberOfTopPosts(int count);
        PostInfo[] GetNumberOfAllPostInfos(int count);
    }
}
