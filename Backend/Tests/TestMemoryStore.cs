using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Backend.Lib.Store;
using SubredditStats.Shared.Model;

namespace Tests
{
    internal class TestMemoryStore : MemoryStore
    {
        internal TestMemoryStore()
        {
            AddPostInfos(PostInfo.List.CreateRandom(100));
            SetTopPosters(PostInfo.List.CreateRandom(100));
            SetMostPosters(MostPosterInfo.List.CreateRandom(100));
        }
    }
}
