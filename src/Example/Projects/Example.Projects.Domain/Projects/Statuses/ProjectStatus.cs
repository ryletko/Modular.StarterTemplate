using Example.Shared;
using Modular.Framework.Domain.Identifiers;
using Modular.Framework.Domain.Patterns;

namespace Example.Projects.Domain.Projects.Statuses;

public class StatusId(Guid id) : TypedIdValueBase(id);

public class ProjectStatus : EntityAudited<StatusId>
{
    public ProjectStatusCode StatusCode;
    public string? Notes;
    public bool IsCurrent;

    protected ProjectStatus()
    {
    }

    private ProjectStatus(bool isNew) : base(isNew)
    {
    }

    public static ProjectStatus CreateNew(ProjectStatusCode status, string? notes) =>
        new(true)
        {
            Notes      = notes,
            StatusCode = status,
            IsCurrent  = false
        };
}