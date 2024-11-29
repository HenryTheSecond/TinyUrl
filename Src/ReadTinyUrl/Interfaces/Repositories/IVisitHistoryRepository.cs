using ReadTinyUrl.Models.MongoModels;
using ReadTinyUrl.Models.Responses;
using Shared.Interfaces;

namespace ReadTinyUrl.Interfaces.Repositories;

public interface IVisitHistoryRepository : IMongoDbRepository<VisitHistory>
{
    Task<List<VisitHistoryResponse>> GetVisitHistories(string userId, int take, DateTimeOffset? lastVistedTimeParam, string? lastId);
}
