using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SubredditStats.Backend.Lib
{
    public static class GlobalJsonSerializerOptions
    {
        public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            // nothing
        };
    }
}
