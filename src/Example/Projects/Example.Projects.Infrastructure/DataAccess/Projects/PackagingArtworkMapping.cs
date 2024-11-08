using Example.Projects.Domain.Packaging.Artworks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Projects.Infrastructure.DataAccess.Projects;

public class PackagingArtworkMapping : IEntityTypeConfiguration<PackagingArtwork>
{
    public void Configure(EntityTypeBuilder<PackagingArtwork> builder)
    {
        builder.ToTable("PackagingArtworks", Schema.Name);
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.Number,
                        x =>
                        {
                            x.Property(y => y.Text).HasColumnName(nameof(ArtworkNumber));
                        });
    }
}