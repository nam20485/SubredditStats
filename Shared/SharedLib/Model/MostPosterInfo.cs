﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Shared.Model
{
    public class MostPosterInfo
    {
        public string Username { get; set; }
        public int PostCount { get; set; }
        public string Subreddit { get; set; }
        
        public class List : List<MostPosterInfo>
        {
            public List() : base()  { }
            public List(IEnumerable<MostPosterInfo> collection) : base(collection)  { }
            public List(int capacity) : base(capacity)  { }
        }   

    }
}