using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Backend.Lib.Utils
{
    // translated from https://stackoverflow.com/a/7728872/4848067

    public class SlidingWindowEventLimiter   
    {
        private readonly Queue<DateTime> _requestTimes;
        private readonly int _maxRequestsPerWindow;
        private readonly TimeSpan _windowDuration;

        public SlidingWindowEventLimiter(int maxRequestsPerWindow, TimeSpan windowDuration)
        {
            this._maxRequestsPerWindow = maxRequestsPerWindow;
            _windowDuration = windowDuration;
            _requestTimes = new Queue<DateTime>(maxRequestsPerWindow);
        }

        private void RemovePastRequests()
        {
            // earliest existing request is expired, remove it
            while ((_requestTimes.Count > 0) && (_requestTimes.Peek().Add(_windowDuration) < DateTime.UtcNow))
            {
                _requestTimes.Dequeue();
            }
        }

        public bool CanRequestNow()
        {
            RemovePastRequests();
            return _requestTimes.Count < _maxRequestsPerWindow;
        }

        public void EnqueueRequest()
        {
            while (!CanRequestNow())
            {
                // wait until we can request again (i.e. when earliest existing request will expire)                
                var earliestRequestWillExpireIn = _requestTimes.Peek().Add(_windowDuration).Subtract(DateTime.UtcNow);
                // TODO: what happens when Queue is empty?
                // TODO: what happens when earliestRequestWillExpireIn is negative?
                Thread.Sleep(earliestRequestWillExpireIn);                
            }

            _requestTimes.Enqueue(DateTime.UtcNow);
        }
    }
}
