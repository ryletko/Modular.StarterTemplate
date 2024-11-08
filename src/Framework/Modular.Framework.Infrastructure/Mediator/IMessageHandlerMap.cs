using Modular.Framework.Application.Message;

namespace Modular.Framework.Infrastructure.Mediator;

public interface IMessageHandlerMap
{
    MessageHandlerResolutionParameters GetResolutionParams<T>() where T : class, IMessageBase;
}