using System.Text;

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

        public static MostPosterInfo CreateRandom()
        {
            var random = new Random();            

            var username = $"username{random.Next(0, 1000)}";
            var postCount = random.Next(0, 1000);
            var subreddit = $"subreddit{random.Next(0, 1000)}";

            return new MostPosterInfo(username, postCount, subreddit);
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
            
            public static List CreateRandom(int count)
            {
                var list = new List();

                for (var i = 0; i < count; i++)
                {
                    list.Add(MostPosterInfo.CreateRandom());
                }

                return list;
            }
        }           
    }
}
