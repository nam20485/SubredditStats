using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubredditStats.Backend.Lib
{
    public class RedditApiToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresInS { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonIgnore]
        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public DateTime ExpiresAt => RetrievedAt.AddSeconds(ExpiresInS);

        [JsonIgnore]
        public bool IsExpired => ExpiresAt <= DateTime.UtcNow;
    }
}
