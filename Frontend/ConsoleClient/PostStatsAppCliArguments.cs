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
        internal bool AllPosts => GetArgumentValueForCallableName<bool>();
        internal bool TopPosts => GetArgumentValueForCallableName<bool>();
        internal bool MostPosters => GetArgumentValueForCallableName<bool>();
        internal int NumberOfPosts => GetArgumentValueForCallableName<int>();
        internal string? ApiUrl => GetArgumentValueForPropertyName<string>();

        public PostStatsAppCliArguments(string[] args)
            : base(args)
        {
        }
    }
}
