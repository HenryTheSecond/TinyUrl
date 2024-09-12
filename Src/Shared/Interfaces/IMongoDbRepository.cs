using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Shared.Interfaces
{
    public interface IMongoDbRepository<TDocument> where TDocument : IMongoDbDocument
    {
        Task<List<TDocument>> FindAsync(Expression<Func<TDocument, bool>> predicate, IClientSessionHandle? session = null);
        Task InsertAsync(TDocument document, IClientSessionHandle? session = null);
        Task ReplaceAsync(TDocument document, IClientSessionHandle? session = null);
        Task DeleteAsync(ObjectId id, IClientSessionHandle? session = null);
    }
}
