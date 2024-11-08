using Modular.Framework.Application.Message;
using Modular.Framework.Infrastructure.Dependencies;

namespace Modular.Framework.Infrastructure.Mediator;

public class MessageHandlerMap : IMessageHandlerMap
{
    private static Dictionary<Type, MessageHandlerResolutionParameters> _map = new();

    public void Add(ICompositionRoot compositionRoot, Type messageType, Type messageHandlerType)
    {
        _map.Add(messageType, new MessageHandlerResolutionParameters(compositionRoot, messageHandlerType));
    }
    
    public MessageHandlerResolutionParameters GetResolutionParams<T>() where T : class, IMessageBase
    {
        return _map[typeof(T)];
    }
}