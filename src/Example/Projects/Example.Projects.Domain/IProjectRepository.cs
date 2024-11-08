using Example.Projects.Domain.Projects;

namespace Example.Projects.Domain;

public interface IProjectRepository
{
    Task<Project?> GetById(ProjectId projectId);

    void Add(Project project);
}