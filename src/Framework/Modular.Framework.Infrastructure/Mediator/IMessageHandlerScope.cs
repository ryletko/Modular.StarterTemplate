namespace Modular.Framework.Infrastructure.Mediator;

public interface IMessageHandlerScope : IDisposable
{
    public object GetMessageHandler(Type messageHandlerType);
}