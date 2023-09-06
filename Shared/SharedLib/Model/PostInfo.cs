using System.Text;

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

        //🜂🜄▼▲↑↓

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"\"{PostTitle}\"");
            sb.AppendLine($"\t- {Author} (↑{UpVotes} ↓{DownVotes})");
            sb.Append($"\t- {PostUrl}");

            return sb.ToString();
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
            
            public static List CreateRandom(int count)
            {
                var list = new List();
                
                for (var i = 0; i < count; i++)
                {
                    list.Add(PostInfo.CreateRandom());
                }

                return list;
            }
        }

        public static PostInfo CreateRandom()
        {
            var random = new Random();

            var subreddit = $"subreddit{random.Next(0, 1000)}";
            var postTitle = $"{subreddit} Post Title {random.Next(0, 1000)}";
            var upVotes = random.Next(0, 1000);
            var downVotes = random.Next(0, 1000);
            var score = upVotes - downVotes;
            var postUrl = $"https://www.reddit.com/r/subreddit/{subreddit}";
            var author = $"username{random.Next(0, 1000)}";
            var apiName = $"apiName{random.Next(0, 1000)}";
            var created = DateTime.UtcNow;
            var fetched = DateTime.UtcNow;

            return new SubredditStats.Shared.Model.PostInfo(postTitle, upVotes, downVotes, score, postUrl, subreddit, author, apiName, created, fetched);
        }
    }
}
