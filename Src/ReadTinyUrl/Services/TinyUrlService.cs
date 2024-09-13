using ReadTinyUrl.Interfaces.Repositories;
using ReadTinyUrl.Interfaces.Services;
using Shared.Attributes;

namespace ReadTinyUrl.Services
{
    [Export(LifeCycle = LifeCycle.SINGLETON)]
    public class TinyUrlService(ITinyUrlRepository tinyUrlRepository) : ITinyUrlService
    {
        public async Task<string> ReadUrlAsync(string tinyUrl)
        {
            return (await tinyUrlRepository.FindOneAsync(x => x.ShortUrl == tinyUrl)).OriginalUrl;
        }
    }
}
