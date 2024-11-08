using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Modular.Framework.Infrastructure.DataAccess;

public interface IDbConnectionAccessor : IAsyncDisposable
{
    DbConnection Open();
    DbConnection? Current { get; }
}

public class DbConnectionAccessor(string connectionString) : IDbConnectionAccessor
{
    public bool IsOwner { get; private set; } = false;

    private static AsyncLocal<DbConnectionHolder?> _current = new();

    private sealed class DbConnectionHolder
    {
        public DbConnection? DbConnection;
    }

    public DbConnection? Current
    {
        get => _current.Value?.DbConnection;
        private set
        {
            var holder = _current.Value;
            if (holder != null)
            {
                holder.DbConnection = null;
            }

            if (value != null)
            {
                _current.Value = new DbConnectionHolder() {DbConnection = value};
            }
        }
    }

    // public DbConnection? Current
    // {
    //     get => DbConnection.Current;
    //     private set
    //     {
    //         DbConnection.Current = value;
    //     }
    // }


    public DbConnection Open()
    {
        if (Current != null)
            throw new DbConnectionException(DbConnectionError.AlreadyOpen);

        IsOwner = true;
        return Current = new SqlConnection(connectionString);
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

public class DbConnectionError
{
    public string ErrorText { get; }

    public static readonly DbConnectionError AlreadyOpen = new("Db connection already opened.");

    private DbConnectionError(string errorText)
    {
        ErrorText = errorText;
    }
}

public class DbConnectionException(DbConnectionError dbConnectionError) : Exception(dbConnectionError.ErrorText)
{
    public DbConnectionError ErrorCode { get; set; } = dbConnectionError;
}