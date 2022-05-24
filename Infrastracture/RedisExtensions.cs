using ServiceStack.Redis;
using System;

namespace Infrastracture
{
    public static class RedisExtensions
    {
        public static T Exec<T>(this IRedisClientsManager redisClientsManager, Func<IRedisClient, T> func)
        {
            using (IRedisClient arg = redisClientsManager.GetClient())
            {
                return func(arg);
            }
        }
    }
}
