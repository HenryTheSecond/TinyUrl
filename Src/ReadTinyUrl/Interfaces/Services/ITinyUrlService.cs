using ReadTinyUrl.Models.Responses;

namespace ReadTinyUrl.Interfaces.Services
{
    public interface ITinyUrlService
    {
        Task<string> ReadUrlAsync(string tinyUrl);
        Task<List<VisitHistoryResponse>> GetHistory(string userId, int take, DateTimeOffset? lastVistedTimeParam, string? lastId);
    }
}
