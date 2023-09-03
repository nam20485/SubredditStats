using SubredditStats.Shared.Client;

namespace SubredditStats.Frontend.ConsoleClient
{
    internal class Program
    {
        static int Main(string[] args)
        {
            return (int) new PostStatsApp(args).Run();            
        }
    }
}