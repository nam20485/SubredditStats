using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Shared.Model
{
    public class PostInfo
    {
        public string PostTitle { get; }
        public int UpVotes { get; }
        public int DownVotes { get; }
        public int Score { get; }
        public string PostUrl { get; }
        public string Subreddit { get; }
        public string Author { get; }
        public string ApiName { get; }
        public DateTime Created { get; }
        public DateTime Fetched { get; }

        /// <summary>
        /// Fuzzing is applied to the up and down vote totals, so the 
        /// real "vote value" is the difference between the two totals.
        /// <para>
        /// Thus, &lt; 0 means a negative, or overall downvote, &gt; 0 means a 
        /// positive or overall upvote, and 0 means its even (or note votes at all).
        /// </para>
        /// </summary>
        public int VoteDifference => UpVotes - DownVotes;

        public PostInfo(string postTitle, int upVotes, int downVotes, int score, string postUrl, string subreddit, string author, string apiName, DateTime created, DateTime fetched)
        {
            PostTitle = postTitle;
            UpVotes = upVotes;
            DownVotes = downVotes;
            Score = score;
            PostUrl = postUrl;
            Subreddit = subreddit;
            Author = author;
            ApiName = apiName;
            Created = created;
            Fetched = fetched;
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
