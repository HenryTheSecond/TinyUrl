using Shared.Interfaces;
using Shared.Models.MongoDb;

namespace WriteTinyUrl.Interfaces.Repositories.MongoDb
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {
    }
}
