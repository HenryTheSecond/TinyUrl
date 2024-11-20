using ReadTinyUrl.Constants;
using ReadTinyUrl.Interfaces.Repositories;
using ReadTinyUrl.Interfaces.Services;
using Shared.Attributes;
using StackExchange.Redis;

namespace ReadTinyUrl.Services
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlService(ITinyUrlRepository tinyUrlRepository, IDatabase redisDatabase, ILogger<TinyUrlService> logger) : ITinyUrlService
    {
        public async Task<string> ReadUrlAsync(string tinyUrl)
        {
            try
            {
                var cachedUrl = await redisDatabase.HashGetAsync(RedisConstants.TinyUrlKey, tinyUrl);
                if (!cachedUrl.IsNullOrEmpty)
                {
                    return cachedUrl.ToString();
                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "Get cache from Redis server {Server} cause error", redisDatabase.Multiplexer.Configuration);
            }

            // TODO: handle exception
            var urlObj = await tinyUrlRepository.FindOneAsync(x => x.ShortUrl == tinyUrl && x.Expire > DateTimeOffset.Now) ?? throw new Exception("Not found URL");
            var originalUrl = urlObj.OriginalUrl;

            await redisDatabase.HashSetAsync(RedisConstants.TinyUrlKey, [new HashEntry(tinyUrl, originalUrl)], CommandFlags.FireAndForget);
            await redisDatabase.HashFieldExpireAsync(RedisConstants.TinyUrlKey, [new RedisValue(tinyUrl)],
                CalculateTimeToLive(urlObj.Expire), flags: CommandFlags.FireAndForget);

            return originalUrl;
        }

        private static TimeSpan CalculateTimeToLive(DateTimeOffset expire)
        {
            var timeToLive = new TimeSpan(0, 1, 0);
            var remainingTime = DateTimeOffset.Now - expire;
            return timeToLive < remainingTime ? timeToLive : remainingTime;
        }
    }
}
