using ReadTinyUrl.Constants;
using ReadTinyUrl.Interfaces.Repositories;
using ReadTinyUrl.Interfaces.Services;
using Shared.Attributes;
using StackExchange.Redis;

namespace ReadTinyUrl.Services
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlService(ITinyUrlRepository tinyUrlRepository, IDatabase redisDatabase) : ITinyUrlService
    {
        public async Task<string> ReadUrlAsync(string tinyUrl)
        {
            var cachedUrl = await redisDatabase.HashGetAsync(RedisConstants.TinyUrlKey, tinyUrl);
            if (!cachedUrl.IsNullOrEmpty)
            {
                return cachedUrl.ToString();
            }

            var originalUrl = (await tinyUrlRepository.FindOneAsync(x => x.ShortUrl == tinyUrl)).OriginalUrl;
            await redisDatabase.HashSetAsync(RedisConstants.TinyUrlKey, [new HashEntry(tinyUrl, originalUrl)], CommandFlags.FireAndForget);
            await redisDatabase.HashFieldExpireAsync(RedisConstants.TinyUrlKey, [new RedisValue(tinyUrl)],
                new TimeSpan(0, 1, 0), flags: CommandFlags.FireAndForget);
            return originalUrl;
        }
    }
}
