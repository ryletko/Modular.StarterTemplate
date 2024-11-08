namespace Modular.Framework.Application.Message;

public interface IMessageBase
{
    Guid Id { get; }
}

public interface IMessage : IMessageBase;

public interface IMessage<out TR> : IMessageBase;
