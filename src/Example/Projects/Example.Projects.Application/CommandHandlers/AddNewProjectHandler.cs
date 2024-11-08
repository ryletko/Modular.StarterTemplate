using Example.Projects.Contracts.Commands;
using Example.Projects.Domain;
using Example.Projects.Domain.Packaging;
using Example.Projects.Domain.Projects;
using Example.Shared;
using Modular.Framework.Application.Commands;

namespace Example.Projects.Application.CommandHandlers;

internal class AddNewProjectHandler(IProjectRepository projectRepository) : ICommandHandler<AddNewProject, GuidResult>
{
    public async Task<GuidResult> Handle(AddNewProject command, CancellationToken cancellationToken)
    {
        if (command.ProjectType == ProjectTypeEnum.Packaging)
        {
            var project = PackagingProject.CreateNew(ProjectName.FromString(command.ProjectName), command.Description);
            projectRepository.Add(project);
            return new GuidResult(project.Id.Value);
        }

        throw new InvalidOperationException("Project type is not supported");
    }
}