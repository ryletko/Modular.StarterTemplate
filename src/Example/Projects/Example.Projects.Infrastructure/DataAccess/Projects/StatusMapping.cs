using Example.Projects.Domain.Projects.Statuses;
using Example.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Projects.Infrastructure.DataAccess.Projects;

public class StatusMapping : IEntityTypeConfiguration<ProjectStatus>
{
    public void Configure(EntityTypeBuilder<ProjectStatus> builder)
    {
        builder.ToTable("Statuses", Schema.Name);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StatusCode)
               .HasConversion(p => p.Value,
                              p => ProjectStatusCode.FromValue(p));
    }
}