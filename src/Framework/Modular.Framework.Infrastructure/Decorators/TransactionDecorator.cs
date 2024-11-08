using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Modular.Framework.Application.Message;
using Modular.Framework.Infrastructure.DataAccess;
using Modular.Utils;

namespace Modular.Framework.Infrastructure.Decorators;

public class TransactionDecorator<T>(IMessageHandler<T> innerHandler,
                                     ITransactionAccessor transactionAccessor,
                                     IDbConnectionAccessor dbConnectionAccessor,
                                     DbContext dbContext) : IMessageHandler<T> where T : class, IMessage
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        DbTransaction? transaction = null;
        if (transactionAccessor.Current == null)
            transaction = transactionAccessor.Begin(dbConnectionAccessor.Current?.ApplyIf(x => x.State != ConnectionState.Open, x => x.Open()) ?? dbConnectionAccessor.Open());

        try
        {
            await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
            await innerHandler.Handle(command, cancellationToken);

            if (transaction != null)
                await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            if (transaction != null)
                await transaction.RollbackAsync(cancellationToken);

            throw;
        }
        finally
        {
            if (transaction != null)
                await transaction.DisposeAsync();
        }
    }
}

public class TransactionDecorator<T, TR>(IMessageHandler<T, TR> innerHandler,
                                         ITransactionAccessor transactionAccessor,
                                         IDbConnectionAccessor dbConnectionAccessor,
                                         DbContext dbContext) : IMessageHandler<T, TR> where T : class, IMessage<TR>
                                                                                       where TR : class
{
    public async Task<TR> Handle(T command, CancellationToken cancellationToken)
    {
        DbTransaction? transaction = null;
        if (transactionAccessor.Current == null)
            transaction = transactionAccessor.Begin(dbConnectionAccessor.Current?.ApplyIf(x => x.State != ConnectionState.Open, x => x.Open()) ?? dbConnectionAccessor.Open());

        try
        {
            await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
            var result = await innerHandler.Handle(command, cancellationToken);

            if (transaction != null)
                await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            if (transaction != null)
                await transaction.RollbackAsync(cancellationToken);

            throw;
        }
        finally
        {
            if (transaction != null)
                await transaction.DisposeAsync();
        }
    }
}