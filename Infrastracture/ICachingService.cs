namespace Infrastracture
{
    public interface ICachingService
    {
        string BuildKey(string data);
        void CacheData<T>(T data, string key, int expireTime = -1); //expires never
        T GetDataFromCache<T>(string key);
    }
}
