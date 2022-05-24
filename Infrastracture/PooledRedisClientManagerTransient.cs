using ServiceStack.Caching;
using ServiceStack.Redis;

namespace Infrastracture
{
    public class PooledRedisClientManagerTransient : IRedisClientsManagerTransient
    {
        private readonly IRedisClientsManager _manager;

        public PooledRedisClientManagerTransient(IRedisClientsManager manager)
        {
            _manager = manager;
        }

        public void Dispose()
        {
            _manager.Dispose();
        }

        public IRedisClient GetClient()
        {
            return _manager.GetClient();
        }

        public IRedisClient GetReadOnlyClient()
        {
            return _manager.GetReadOnlyClient();
        }

        public ICacheClient GetCacheClient()
        {
            return _manager.GetCacheClient();
        }

        public ICacheClient GetReadOnlyCacheClient()
        {
            return _manager.GetReadOnlyCacheClient();
        }
    }
}
