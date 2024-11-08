using Modular.Framework.Application.Message;

namespace Modular.Framework.Infrastructure.Mediator;

public class Mediator(IMessageHandlerResolver messageHandlerResolver) : IMediator
{
    public async Task Handle<T>(T message, CancellationToken cancellationToken = default) where T : class, IMessage
    {
        var handler = messageHandlerResolver.Resolve<T>();
        await handler.Handle(message, cancellationToken);
    }

    public async Task<TR> Handle<T, TR>(T message, CancellationToken cancellationToken = default) where T : class, IMessage<TR>
                                                                                                  where TR : class
    {
        var handler = messageHandlerResolver.Resolve<T, TR>();
        return await handler.Handle(message, cancellationToken);
    }
}