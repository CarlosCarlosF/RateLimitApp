using System;
namespace RateLimitApplication
{
    public class CounterInfo
    {
        public int Counter { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime AttempTime { get; set; }
        public DateTime LimitTime { get; set; }
    }
}
