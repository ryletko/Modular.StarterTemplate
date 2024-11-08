using Example.Projects.Domain;
using Example.Projects.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Example.Projects.Infrastructure.DataAccess;

public class ProjectRepository(ProjectsDbContext projectsDbContext) : IProjectRepository
{
    public async Task<Project?> GetById(ProjectId projectId)
    {
        return await projectsDbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectId);
    }

    public void Add(Project project)
    {
        projectsDbContext.Projects.Add(project);
    }
}