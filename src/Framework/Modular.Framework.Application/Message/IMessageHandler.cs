namespace Modular.Framework.Application.Message;

public interface IMessageHandler<T> where T : class, IMessage
{
    Task Handle(T command, CancellationToken cancellationToken);
}

public interface IMessageHandler<T, TR> where T : class, IMessage<TR>
                                        where TR : class
{
    Task<TR> Handle(T command, CancellationToken cancellationToken);
}