namespace Infrastracture
{
    public class RedisKeyProvider
    {
        public static string GetTransient(string key)
        {
            return "urn:Transient:" + key;
        }
    }
}
