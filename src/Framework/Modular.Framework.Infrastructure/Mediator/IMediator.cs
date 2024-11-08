using Modular.Framework.Application.Message;

namespace Modular.Framework.Infrastructure.Mediator;

public interface IMediator
{
    Task Handle<T>(T message, CancellationToken cancellationToken = default) where T : class, IMessage;

    Task<TR> Handle<T, TR>(T message, CancellationToken cancellationToken = default) where T : class, IMessage<TR>
                                                                                     where TR : class;
}