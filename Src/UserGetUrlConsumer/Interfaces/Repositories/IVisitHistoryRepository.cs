using Shared.Interfaces;
using UserGetUrlConsumer.Models;

namespace UserGetUrlConsumer.Interfaces.Repositories
{
    public interface IVisitHistoryRepository : IMongoDbRepository<VisitHistory>
    {
    }
}
