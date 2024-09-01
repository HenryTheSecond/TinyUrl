namespace Shared.Interfaces
{
    public interface ITransactionContext
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
    }
}
