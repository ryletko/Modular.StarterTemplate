using Modular.Framework.Application.Message;

namespace Modular.Framework.Infrastructure.Mediator;

public interface IMessageHandlerResolver
{
    IMessageHandler<T> Resolve<T>() where T : class, IMessage;

    IMessageHandler<T, TR> Resolve<T, TR>() where T : class, IMessage<TR>
                                            where TR : class;
}