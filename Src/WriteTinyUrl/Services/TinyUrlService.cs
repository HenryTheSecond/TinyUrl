using Shared.Attributes;
using Shared.Interfaces;
using WriteTinyUrl.Interfaces.Repositories;
using WriteTinyUrl.Interfaces.Services;

namespace WriteTinyUrl.Services
{
    [Export(LifeCycle = LifeCycle.SCOPE)]
    public class TinyUrlService(IUrlRangeRepository urlRangeRepository, ITransactionContext transactionContext) : ITinyUrlService
    {
        public async Task<string> CreateTinyUrlAsync(string originalUrl)
        {
            await transactionContext.BeginTransactionAsync();
            
            // TODO: define exception handling
            var tinyUrl = await urlRangeRepository.GetTinyUrlAsync() ?? throw new Exception("Out of tiny url");

            tinyUrl.IsUsed = true;
            await urlRangeRepository.SaveChangeAsync();

            await transactionContext.CommitTransactionAsync();

            return tinyUrl.Url;
        }
    }
}
