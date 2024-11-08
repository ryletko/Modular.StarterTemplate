using Modular.Framework.Application.Commands;
using Modular.Framework.Application.Context;
using Modular.Framework.Application.Message;
using Modular.Framework.Domain.DomainContext;

namespace Modular.Framework.Infrastructure.Decorators;

public class DomainContextDecorator<T>(IMessageHandler<T> innerHandler,
                                       IAppContext appContext) : IMessageHandler<T> where T : class, ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        DomainContextAccessor.Current = new DomainContext(appContext.UserId, appContext.StartedAt);
        await innerHandler.Handle(command, cancellationToken);
    }
}

public class DomainContextDecorator<T, TR>(IMessageHandler<T, TR> innerHandler,
                                           IAppContext appContext) : IMessageHandler<T, TR> where T : class, ICommand<TR>
                                                                                            where TR : class
{
    public async Task<TR> Handle(T command, CancellationToken cancellationToken)
    {
        DomainContextAccessor.Current = new DomainContext(appContext.UserId, appContext.StartedAt);
        var result = await innerHandler.Handle(command, cancellationToken);
        return result;
    }
}