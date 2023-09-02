using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Shared.Model
{
    public class PostInfo
    {
        public string? PostTitle { get; set; }
        public int UpVotes { get; set; }
        public string? PostUrl { get; set; }        
        public string? Subreddit { get; set; }
        public string? Author { get; set; }
        public string? ApiName { get; set; }

        public PostInfo()
        {
            UpVotes = -1;
        }

        public class StringDictionary : Dictionary<string, PostInfo>
        {
            public StringDictionary() : base() { }
            public StringDictionary(StringDictionary collection) : base(collection) { }
        }

        public class List : List<PostInfo>
        {
            public List() : base() { }
            public List(IEnumerable<PostInfo> collection) : base(collection) { }            
        }        
    }
}
