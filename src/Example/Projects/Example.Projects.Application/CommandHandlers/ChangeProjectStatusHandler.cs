using Example.Projects.Contracts.Commands;
using Example.Projects.Domain;
using Example.Projects.Domain.Projects;
using Example.Shared;
using Modular.Framework.Application.Commands;

namespace Example.Projects.Application.CommandHandlers;

internal class ChangeProjectStatusHandler(IProjectRepository projectRepository): ICommandHandler<ChangeProjectStatus>
{
    public async Task Handle(ChangeProjectStatus command, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetById(new ProjectId(command.ProjectId));
        project.SendProjectToStatus(ProjectStatusCode.FromValue(command.ProjectStatusCode), command.Notes);
    }
}