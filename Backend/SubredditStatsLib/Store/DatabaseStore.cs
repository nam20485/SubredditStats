using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared;
using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public class DatabaseStore : ISubredditPostStatsStore
    {
        // e.g. use Entity Framework to store and fetch the data from a database

        public event ISubredditPostStatsStore.PostListUpdatedHandler? PostListUpdated;

        public DateTime? Started { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string? Subreddit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MostPosterInfo[] MostPosters => throw new NotImplementedException();

        public PostInfo[] TopPosts => throw new NotImplementedException();

        public PostInfo[] AllPostInfos => throw new NotImplementedException();

        public void SetMostPosters(IEnumerable<MostPosterInfo> mostPosters) => throw new NotImplementedException();

        public void AddPostInfos(IEnumerable<PostInfo> postInfos) => throw new NotImplementedException();

        public void SetTopPosters(IEnumerable<PostInfo> topPostInfos) => throw new NotImplementedException();

        public MostPosterInfo[] GetNumberOfMostPosters(int count) => throw new NotImplementedException();

        public PostInfo[] GetNumberOfTopPosts(int count) => throw new NotImplementedException();

        public PostInfo[] GetNumberOfAllPostInfos(int count) => throw new NotImplementedException();
    }
}
