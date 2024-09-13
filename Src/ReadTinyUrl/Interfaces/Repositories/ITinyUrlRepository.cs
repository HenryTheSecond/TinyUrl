using Shared.Interfaces;
using Shared.Models.MongoDb;

namespace ReadTinyUrl.Interfaces.Repositories
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {

    }
}
