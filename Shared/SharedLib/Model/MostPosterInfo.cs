using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Shared.Model
{
    public class MostPosterInfo
    {
        public string Username { get; }
        public int PostCount { get; }
        public string Subreddit { get; }
        
        public MostPosterInfo(string username, int postCount, string subreddit)
        {
            Username = username;
            PostCount = postCount;
            Subreddit = subreddit;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"{Username} ({PostCount} Posts)");            

            return sb.ToString();
        }

        public class StringDictionary : Dictionary<string, MostPosterInfo>
        {
            public StringDictionary() : base() { }
            public StringDictionary(StringDictionary collection) : base(collection) { }
        }

        public class List : List<MostPosterInfo>
        {
            public List() : base()  { }
            public List(IEnumerable<MostPosterInfo> collection) : base(collection)  { }            
        }           
    }
}
