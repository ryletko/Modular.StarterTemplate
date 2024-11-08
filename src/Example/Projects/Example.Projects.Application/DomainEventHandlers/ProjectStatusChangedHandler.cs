using Example.Projects.Domain.Projects.Statuses.Events;
using Modular.Framework.Application.Events;
using Modular.Framework.Application.Message;

namespace Example.Projects.Application.DomainEventHandlers;

public class ProjectStatusChangedHandler: IMessageHandler<DomainEvent<ProjectStatusChanged>> 
{
    public async Task Handle(DomainEvent<ProjectStatusChanged> command, CancellationToken cancellationToken)
    {
        
    }
}