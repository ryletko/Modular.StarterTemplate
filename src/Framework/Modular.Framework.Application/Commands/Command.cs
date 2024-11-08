using Modular.Framework.Application.Message;

namespace Modular.Framework.Application.Commands;

public interface ICommand: IMessage;
public interface ICommand<out TR> : IMessage<TR> where TR : class;