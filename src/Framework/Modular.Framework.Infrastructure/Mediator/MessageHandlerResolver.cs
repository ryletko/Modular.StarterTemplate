using Modular.Framework.Application.Message;

namespace Modular.Framework.Infrastructure.Mediator;

public class MessageHandlerResolver(IMessageHandlerMap messageHandlerMap) : IMessageHandlerResolver
{
    public IMessageHandler<T> Resolve<T>() where T : class, IMessage
    {
        var resolutionParameters = messageHandlerMap.GetResolutionParams<T>();
        return new MessageHandlerScoped<T>(resolutionParameters.CompositionRoot, resolutionParameters.MessageHandlerType);
    }

    public IMessageHandler<T, TR> Resolve<T, TR>() where T : class, IMessage<TR> where TR : class
    {
        var resolutionParameters = messageHandlerMap.GetResolutionParams<T>();
        return new MessageHandlerScoped<T, TR>(resolutionParameters.CompositionRoot, resolutionParameters.MessageHandlerType);
    }
}