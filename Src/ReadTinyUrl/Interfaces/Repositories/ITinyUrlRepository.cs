using ReadTinyUrl.Models.MongoModels;
using Shared.Interfaces;

namespace ReadTinyUrl.Interfaces.Repositories
{
    public interface ITinyUrlRepository : IMongoDbRepository<TinyUrl>
    {

    }
}
