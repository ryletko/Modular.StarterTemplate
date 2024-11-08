using Modular.Framework.Application.Message;
using Modular.Framework.Infrastructure.Dependencies;

namespace Modular.Framework.Infrastructure.Mediator;

internal class MessageHandlerScoped<T>(ICompositionRoot compositionRoot,
                                       Type messageHandlerType) : IMessageHandler<T> where T : class, IMessage
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await using (var scope = compositionRoot.BeginScope())
        {
            var messageHandler = (IMessageHandler<T>) scope.GetMessageHandler(messageHandlerType);
            await messageHandler.Handle(command, cancellationToken);
        }
    }
}

internal class MessageHandlerScoped<T, TR>(ICompositionRoot compositionRoot,
                                           Type messageHandlerType) : IMessageHandler<T, TR> where T : class, IMessage<TR>
                                                                                             where TR : class
{
    public async Task<TR> Handle(T command, CancellationToken cancellationToken)
    {
        await using (var scope = compositionRoot.BeginScope())
        {
            var messageHandler = (IMessageHandler<T, TR>) scope.GetMessageHandler(messageHandlerType);
            var result = await messageHandler.Handle(command, cancellationToken);
            return result;
        }
    }
}