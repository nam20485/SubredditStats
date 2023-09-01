using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Shared.Model
{
    public class TopPostInfo
    {
        public string PostTitle { get; set; }
        public int UpVotes { get; set; }
        public string PostUrl { get; set; }        
        public string Subreddit { get; set; }
        public string Author { get; set; }
        public string ApiName { get; set; }

        public class StringDictionary : Dictionary<string, TopPostInfo>
        {
            public StringDictionary() : base() { }
            public StringDictionary(StringDictionary collection) : base(collection) { }
        }

        public class List : List<TopPostInfo>
        {
            public List() : base() { }
            public List(IEnumerable<TopPostInfo> collection) : base(collection) { }            
        }        
    }
}
