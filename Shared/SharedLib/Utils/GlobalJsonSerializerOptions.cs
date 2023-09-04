using System.Text.Json;

namespace SubredditStats.Shared.Utils
{
    public class GlobalJsonSerializerOptions
    {
        public static JsonSerializerOptions Options { get; }

        static GlobalJsonSerializerOptions()
        {
            Options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {

            };
        }
    }
}