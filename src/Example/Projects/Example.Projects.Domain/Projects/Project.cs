using Example.Projects.Domain.Projects.Statuses;
using Example.Projects.Domain.Projects.Statuses.Events;
using Example.Shared;
using Modular.Framework.Domain.Identifiers;
using Modular.Framework.Domain.Patterns;

namespace Example.Projects.Domain.Projects;

public class ProjectId(Guid projectId) : TypedIdValueBase(projectId);

public abstract class Project : EntityAudited<ProjectId>, IAggregateRoot
{
    public ProjectName Name { get; private set; }
    public string Description { get; private set; }
    public ProjectTypeEnum ProjectType { get; private set; }
    public ProjectStatusCode Status { get; private set; }

    protected virtual List<ProjectStatus> StatusesSelf { get; private set; } = new();
    public IReadOnlyCollection<ProjectStatus> Statuses => StatusesSelf.AsReadOnly();

    protected Project()
    {
    }

    protected Project(bool isNew) : base(isNew)
    {
    }

    protected Project(ProjectTypeEnum projectType, ProjectName projectName, string description) : this(true)
    {
        Name        = projectName;
        Description = description;
        ProjectType = projectType;
    }

    public virtual void SendProjectToStatus(ProjectStatusCode statusCode, string? notes)
    {
        var status = ProjectStatus.CreateNew(statusCode, notes);
        StatusesSelf.Add(status);
        Status = status.StatusCode;

        AddDomainEvent(new ProjectStatusChanged());
    }
}