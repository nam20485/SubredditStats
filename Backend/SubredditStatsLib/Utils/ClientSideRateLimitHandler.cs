using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;


namespace SubredditStats.Backend.Lib.Utils
{
    // taken from: https://learn.microsoft.com/en-us/dotnet/core/extensions/http-ratelimiter

    public class ClientSideRateLimitedHandler : DelegatingHandler, IAsyncDisposable
    {
        public const string CustomRasteLimiterHeaderName = "X-Rate-Limiter";

        private readonly RateLimiter _rateLimiter;

        public ClientSideRateLimitedHandler(RateLimiter limiter)
            : base(new HttpClientHandler())
        {
            _rateLimiter = limiter;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {
            using RateLimitLease lease = await _rateLimiter.AcquireAsync(permitCount: 1, cancellationToken);

            if (lease.IsAcquired)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            if (lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
            {
                response.Headers.Add("Retry-After",
                                     Convert.ToInt32(retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo));
                response.Headers.Add(CustomRasteLimiterHeaderName, nameof(ClientSideRateLimitedHandler));
            }

            return response;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _rateLimiter.DisposeAsync().ConfigureAwait(false);

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _rateLimiter.Dispose();
            }
        }
    }
}
