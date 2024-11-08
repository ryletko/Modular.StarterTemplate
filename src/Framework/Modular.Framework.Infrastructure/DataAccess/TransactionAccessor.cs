using System.Data;
using System.Data.Common;

namespace Modular.Framework.Infrastructure.DataAccess;

public interface ITransactionAccessor : IAsyncDisposable
{
    DbTransaction Begin(DbConnection connection);
    DbTransaction? Current { get; }
}

public class TransactionAccessor : ITransactionAccessor
{
    public bool IsOwner { get; private set; } = false;

    private static readonly AsyncLocal<TransactionHolder?> _current = new();

    private sealed class TransactionHolder
    {
        public DbTransaction? Transaction;
    }

    public DbTransaction? Current
    {
        get => _current.Value?.Transaction;
        private set
        {
            var holder = _current.Value;
            if (holder != null)
            {
                holder.Transaction = null;
            }

            if (value != null)
            {
                _current.Value = new TransactionHolder() {Transaction = value};
            }
        }
    }

    public DbTransaction Begin(DbConnection connection)
    {
        if (Current != null)
            throw new TransactionException(TransactionError.AlreadyStarted);

        IsOwner = true;
        return Current = connection.BeginTransaction(IsolationLevel.Serializable);
    }

    private bool _disposed;

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (IsOwner && Current != null)
            await Current.DisposeAsync();
    }
}

public class TransactionError
{
    public string ErrorText { get; }

    public static readonly TransactionError AlreadyStarted = new("Transaction already started.");

    private TransactionError(string errorText)
    {
        ErrorText = errorText;
    }
}

public class TransactionException(TransactionError transactionError) : Exception(transactionError.ErrorText)
{
    public TransactionError ErrorCode { get; set; } = transactionError;
}