using Shared.Interfaces;
using WriteTinyUrl.Models;

namespace WriteTinyUrl.Interfaces.Repositories.MongoDb
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {
    }
}
