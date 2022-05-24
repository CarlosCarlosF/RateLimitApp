using Infrastracture;
using ServiceStack;
using System;
using System.Net;

namespace RateLimitApplication.Services
{
    public class RateLimitService : Service
    {
        private readonly ICachingService _cachingService;
        public RateLimitService(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }

        [CustomAsyncResponseFilter]
        public HttpResult Get(RateLimitRequest request)
        {
            var firstAttemp = 1;
            var limitCounter = 10;
            var ipAdress = Request.RemoteIp;
            var key = _cachingService.BuildKey($"{ipAdress}");
            var limitCount = _cachingService.GetDataFromCache<CounterInfo>(key);

            if (limitCount == null)
            {
                limitCount = new CounterInfo()
                {
                    Counter = firstAttemp,
                    StartTime = DateTime.UtcNow,
                    AttempTime = DateTime.UtcNow,
                    LimitTime = DateTime.UtcNow.AddMinutes(1)
                };   
                _cachingService.CacheData(limitCount, key);    
            }
            else if (limitCount.Counter >= limitCounter && limitCount.LimitTime >= limitCount.AttempTime)
            {
                limitCount.AttempTime = DateTime.UtcNow;
                _cachingService.CacheData(limitCount, key);
                return new HttpResult("Rate limit exceeded.")
                {
                    StatusCode = HttpStatusCode.TooManyRequests,
                    ContentType = "text/plain;charset=utf-8",
                    Headers = {
                         ["X-RateLimit-Limit"] = limitCounter.ToString(),
                         ["X-RateLimit-Remaining"] = (limitCounter - limitCount.Counter).ToString()
                    }
                };
            }
            else if (limitCount.Counter <= limitCounter && limitCount.LimitTime >= limitCount.AttempTime)
            {
                limitCount.AttempTime = DateTime.UtcNow;
                limitCount.Counter += 1;
                _cachingService.CacheData(limitCount, key);
            }
            else if (limitCount.LimitTime < limitCount.AttempTime)
            {
                ResetCounter(firstAttemp, limitCount);
                _cachingService.CacheData(limitCount, key);
            }
            return new HttpResult($"Hi, {request.Name}!")
            {
                StatusCode = HttpStatusCode.OK,
                ContentType = "text/plain;charset=utf-8",
                Headers = {
                         ["X-RateLimit-Limit"] = limitCounter.ToString(),
                         ["X-RateLimit-Remaining"] = (limitCounter - limitCount.Counter).ToString()
                }
            };
        }
        private static void ResetCounter(int firstAttemp, CounterInfo limitCount)
        {
            limitCount.Counter = firstAttemp;
            limitCount.StartTime = DateTime.UtcNow;
            limitCount.AttempTime = DateTime.UtcNow;
            limitCount.LimitTime = DateTime.UtcNow.AddMinutes(1);
        }
    }
}
