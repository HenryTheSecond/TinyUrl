using WriteTinyUrl.Interfaces.Repositories;
using WriteTinyUrl.Interfaces.Services;

namespace WriteTinyUrl.Services
{
    public class TinyUrlService(IUrlRangeRepository urlRangeRepository) : ITinyUrlService
    {
        public async Task<string> CreateTinyUrlAsync(string originalUrl)
        {
            // TODO: define exception handling
            var tinyUrl = await urlRangeRepository.GetTinyUrlAsync() ?? throw new Exception("Out of tiny url");

            tinyUrl.IsUsed = true;
            await urlRangeRepository.SaveChangeAsync();

            return "aaa";
        }
    }
}
