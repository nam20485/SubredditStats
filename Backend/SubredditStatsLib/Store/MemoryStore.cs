﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Shared.Model;

namespace SubredditStats.Backend.Lib.Store
{
    public class MemoryStore : ISubredditPostStatsStore
    {
        private readonly PostInfo.StringDictionary _postInfosByApiName;
        private readonly MostPosterInfo.StringDictionary _mostPosterInfosByUsername;        
        private readonly PostInfo.StringDictionary _topPostInfosByApiName;

        // basic concurrency synchronization
        private readonly object _mostPostersLock;
        private readonly object _topPostsLock;
        private readonly object _postInfosLock;

        public MemoryStore()
        {
            _mostPostersLock = new();
            _topPostsLock = new();
            _postInfosLock = new();

            _postInfosByApiName = new();
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

        public PostInfo[] TopPosts
        {
            get
            {
                lock (_topPostsLock)
                {
                    return _topPostInfosByApiName.Values.ToArray();
                }
            }
        }

        public PostInfo[] PostInfos
        {
            get
            {
                lock (_postInfosLock)
                {
                    return _postInfosByApiName.Values.ToArray();
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

        public void AddTopPosters(IEnumerable<PostInfo> topPostInfos)
        {
            lock (_topPostsLock)
            {
                foreach (var topPostInfo in topPostInfos)
                {
                    _postInfosByApiName[topPostInfo.ApiName] = topPostInfo;
                }
            }
        }

        public void AddPostInfos(IEnumerable<PostInfo> postInfos)
        {
            lock (_topPostsLock)
            {
                foreach (var postInfo in postInfos)
                {
                    _postInfosByApiName[postInfo.ApiName] = postInfo;
                }
            }
        }
    }
}
