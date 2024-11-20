using WriteTinyUrl.Models;

namespace WriteTinyUrl.Interfaces.Services
{
    public interface ITinyUrlService
    {
        Task<string> CreateTinyUrlAsync(UserInfo claimsInfo, string originalUrl);
    }
}
