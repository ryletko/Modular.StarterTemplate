using Modular.Framework.Application.Message;
using Modular.Framework.Infrastructure.DataAccess;
using Modular.Framework.Infrastructure.DomainEvents;

namespace Modular.Framework.Infrastructure.Decorators;

public class UnitOfWorkDecorator<T>(IMessageHandler<T> decorated,
                                    IDomainEventsProvider domainEventsProvider,
                                    IUnitOfWork unitOfWork,
                                    IDomainEventsDispatcher domainEventsDispatcher) : IMessageHandler<T> where T : class, IMessage
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await decorated.Handle(command, cancellationToken);

        var domainEvents = domainEventsProvider.GetAllDomainEvents();
        domainEventsProvider.ClearAllDomainEvents();

        await unitOfWork.CommitAsync(cancellationToken);

        await domainEventsDispatcher.Dispatch(domainEvents, cancellationToken);
    }
}

public class UnitOfWorkDecorator<T, TR>(IMessageHandler<T, TR> decorated,
                                        IDomainEventsProvider domainEventsProvider,
                                        IUnitOfWork unitOfWork,
                                        IDomainEventsDispatcher domainEventsDispatcher) : IMessageHandler<T, TR> where T : class, IMessage<TR>
                                                                                                                 where TR : class
{
    public async Task<TR> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await decorated.Handle(command, cancellationToken);

        var domainEvents = domainEventsProvider.GetAllDomainEvents();
        domainEventsProvider.ClearAllDomainEvents();

        await unitOfWork.CommitAsync(cancellationToken);

        await domainEventsDispatcher.Dispatch(domainEvents, cancellationToken);

        return result;
    }
}