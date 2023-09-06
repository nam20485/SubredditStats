using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SubredditStats.Frontend.ConsoleClient.Utils;

namespace SubredditStats.Frontend.ConsoleClient
{
    internal class PostStatsAppCliArguments : CliArguments
    {
        private const string DefaultApiUrl = "https://localhost:7199";
        private const int DefaultPostCount = 5;

        //internal bool AllPosts => GetArgumentValueForCallableName<bool>();
        //internal bool TopPosts => GetArgumentValueForCallableName<bool>();
        //internal bool MostPosters => GetArgumentValueForCallableName<bool>();

        internal int PostCount => GetArgumentValueForCallableName(defaultValue: DefaultPostCount);
        internal string ApiUrl => GetArgumentValueForPropertyName(defaultValue: DefaultApiUrl);

        public PostStatsAppCliArguments(string[] args)
            : base(args)
        {
        }
    }
}
