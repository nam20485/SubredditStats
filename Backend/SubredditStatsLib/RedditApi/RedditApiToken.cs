using System.Text.Json.Serialization;

namespace SubredditStats.Backend.Lib.RedditApi
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
        public DateTime RetrievedAt { get; }

        [JsonIgnore]
        public TimeSpan Duration => new(0, 0, ExpiresInS);

        [JsonIgnore]
        public DateTime ExpiresAt => RetrievedAt.Add(Duration);

        [JsonIgnore]
        public bool IsExpired => ExpiresAt <= DateTime.UtcNow;

        public RedditApiToken()
        {
            RetrievedAt = DateTime.UtcNow;
        }
    }
}
