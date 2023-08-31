using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubredditStats.Backend.Lib
{
    public static class GlobalJsonSerializerOptions
    {
        static GlobalJsonSerializerOptions()
        {
            Options = new JsonSerializerOptions()
            {                
            };
            Options.Converters.Add(new JsonStringEnumConverter());
        }

        public static JsonSerializerOptions Options { get; }
    }
}
