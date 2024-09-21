using StackExchange.Redis;

namespace ReadTinyUrl.Constants
{
    public static class RedisConstants
    {
        public static readonly RedisKey TinyUrlKey = new RedisKey("TinyUrl");
    }
}
