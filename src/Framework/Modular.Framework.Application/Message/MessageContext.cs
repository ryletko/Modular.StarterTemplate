namespace Modular.Framework.Application.Message;

public interface IMessageContext<T> where T : class, IMessage
{
    Guid UserId { get; }
    DateTimeOffset StartedAt { get; }

    T Message { get; }
}

public interface IMessageContext<T, TR> where T : class, IMessage<TR>
                                        where TR : class
{
    Guid UserId { get; }
    DateTimeOffset StartedAt { get; }

    T Message { get; }
}