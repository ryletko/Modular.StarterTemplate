using Example.Projects.Domain.Projects;
using Example.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Projects.Infrastructure.DataAccess.Projects;

public class ProjectsMapping : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.UseTphMappingStrategy();
        builder.ToTable("Projects", Schema.Name);
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.Name,
                        x => { x.Property(y => y.Text).HasColumnName(nameof(ProjectName)); });

        builder.Property(x => x.Status)
               .HasConversion(p => p.Value,
                              p => ProjectStatusCode.FromValue(p));

        builder.HasMany("StatusesSelf")
               .WithOne()
               .HasForeignKey("ProjectId");
        builder.Ignore(x => x.Statuses);
    }
}