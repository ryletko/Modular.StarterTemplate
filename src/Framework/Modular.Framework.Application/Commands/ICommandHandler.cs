using Modular.Framework.Application.Message;

namespace Modular.Framework.Application.Commands;

public interface ICommandHandler<T> : IMessageHandler<T> where T : class, ICommand
{
}

public interface ICommandHandler<T, TR> : IMessageHandler<T, TR> where T : class, ICommand<TR>
                                                                 where TR : class
{
}