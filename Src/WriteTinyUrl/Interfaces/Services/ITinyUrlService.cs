using WriteTinyUrl.Models;
using WriteTinyUrl.Models.Responses;

namespace WriteTinyUrl.Interfaces.Services
{
    public interface ITinyUrlService
    {
        Task<string> CreateTinyUrlAsync(UserInfo claimsInfo, string originalUrl);
        Task<List<CreateUrlHistory>> GetCreateUrlHistories(string userId, int take, DateTimeOffset? lastCreatedDate, string? lastId);
    }
}
