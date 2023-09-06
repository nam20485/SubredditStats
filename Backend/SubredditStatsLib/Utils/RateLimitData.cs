using Microsoft.Extensions.Logging;

namespace SubredditStats.Backend.Lib.Utils
{
    public class RateLimitData
    {
        private readonly int _rateLimitPeriodS = 600;

        private readonly ILogger _logger;

        public RateLimitData(ILogger logger, int rateLimitPeriodS)
        {
            _logger = logger;
            _rateLimitPeriodS = rateLimitPeriodS;
        }


        public double LastRateLimitUsed { get; set; }
        public double LastRateLimitRemaining { get; set; }
        // number of seconds remaining until new period starts
        public int LastRateLimitPeriodReset { get; set; }
        public int RateLimitPeriodPassed => _rateLimitPeriodS - LastRateLimitPeriodReset;
        public double SecondsPerRequestRate => RateLimitPeriodPassed / LastRateLimitUsed;
        public double RequestsPerSecondRate => LastRateLimitUsed / RateLimitPeriodPassed;

        public void LogRateLimitValues()
        {
            _logger.LogInformation(
                "Last Request's Rate Limit Header Values\n\tTotal Used: {LastRateLimitUsed}\n\tTotal Remaining: {LastRateLimitRemaining}\n\tPeriod Remaining (s): {LastRateLimitPeriodReset}\n\tPeriod Passed (s): {RateLimitPeriodPassed}\n\tRate (s/request): {SecondsPerRequestRate:0.0}\n\tRate (requests/s): {RequestsPerSecondRate:0.0}",
                LastRateLimitUsed,
                LastRateLimitRemaining,
                LastRateLimitPeriodReset,
                RateLimitPeriodPassed,
                SecondsPerRequestRate,
                RequestsPerSecondRate);
        }

        public void SaveRateLimitValues(HttpResponseMessage response)
        {
            try
            {
                if (response.Headers.TryGetValues(RedditApi.RedditApi.RateLimitUsedHeaderName, out var values))
                {
                    LastRateLimitUsed = double.Parse(values.First());
                }                
                if (response.Headers.TryGetValues(RedditApi.RedditApi.RateLimitRemainingHeaderName, out values))
                {
                    LastRateLimitRemaining = double.Parse(values.First());
                }                     
                if (response.Headers.TryGetValues(RedditApi.RedditApi.RateLimitResetHeaderName, out values))
                {
                    LastRateLimitPeriodReset = int.Parse(values.First());
                }                
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogWarning(ioe, "{ExceptionMessage}", "Rate limit header not found (client-side rate limiting entered?)");
            }
        }
    }
}
