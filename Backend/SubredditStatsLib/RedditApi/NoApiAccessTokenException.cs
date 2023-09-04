using System.Runtime.Serialization;

namespace SubredditStats.Backend.Lib.RedditApi
{
    [Serializable]
    public class NoApiAccessTokenException : Exception
    {
        public NoApiAccessTokenException()
        {
        }

        public NoApiAccessTokenException(string? message) : base(message)
        {
        }

        public NoApiAccessTokenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoApiAccessTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}