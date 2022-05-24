using System;
using ServiceStack;

namespace Infrastracture
{
    public class CachingService : ICachingService
    {
        private const string Key = "RateLimit:{0}";
        private readonly IRedisClientsManagerTransient _redisClientsManagerTransient;

        public CachingService(IRedisClientsManagerTransient redisClientsManagerTransient)
        {
            _redisClientsManagerTransient = redisClientsManagerTransient;
        }

        public string BuildKey(string data)
        {
            return RedisKeyProvider.GetTransient(Key.FormatWith(data));
        }

        public void CacheData<T>(T data, string key, int expireTime = -1)
        {
            _redisClientsManagerTransient.Exec(redisClient => redisClient.Set(key, data, TimeSpan.FromMinutes(expireTime)));
        }

        public T GetDataFromCache<T>(string key)
        {
            using (var redisClient = _redisClientsManagerTransient.GetClient())
            {
                var cachedData = redisClient.Get<T>(key);
                return cachedData;
            }
        }
    }
}
