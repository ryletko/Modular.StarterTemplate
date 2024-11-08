using Example.Projects.Domain.Packaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Projects.Infrastructure.DataAccess.Projects;

public class PackagingProjectMapping: IEntityTypeConfiguration<PackagingProject>
{
    public void Configure(EntityTypeBuilder<PackagingProject> builder)
    {
        builder.HasMany("ArtworksSelf")
               .WithOne()
               .HasForeignKey("ProjectId");
        builder.Ignore(x => x.Artworks);
    }
}