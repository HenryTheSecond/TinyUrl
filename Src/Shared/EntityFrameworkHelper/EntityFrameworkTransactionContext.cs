using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Interfaces;

namespace Shared.EntityFrameworkHelper
{
    public class EntityFrameworkTransactionContext(DbContext context) : ITransactionContext, IAsyncDisposable, IDisposable
    {
        private IDbContextTransaction? transaction;
        public virtual async Task BeginTransactionAsync()
        {
            if (transaction != null)
            {
                // TODO: handle if one transaction already started
                throw new Exception("Cannot begin more than one transaction");
            }
            transaction = await context.Database.BeginTransactionAsync();
        }

        public virtual async Task CommitTransactionAsync()
        {
            // TODO: handle no transaction
            if (transaction == null)
                throw new Exception("No transaction to commit");
            await transaction.CommitAsync();
        }

        public virtual void Dispose()
        {
            transaction?.Dispose();
        }

        public virtual ValueTask DisposeAsync()
        {
            return transaction != null ? transaction.DisposeAsync() : ValueTask.CompletedTask;
        }
    }
}
