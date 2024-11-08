using Modular.Framework.Domain.Identifiers;
using Modular.Framework.Domain.Patterns;

namespace Example.Projects.Domain.Packaging.Artworks;

public class PackagingArtworkId(Guid artworkId) : TypedIdValueBase(artworkId);

public class PackagingArtwork : EntityAudited<PackagingArtworkId>
{
    public ArtworkNumber Number { get; private set; }
    public string Description { get; private set; }

    private PackagingArtwork() { }

    private PackagingArtwork(bool isNew) : base(isNew)
    {
    }

    public static PackagingArtwork CreateNew(ArtworkNumber number, string description)
    {
        return new PackagingArtwork(true)
               {
                   Number      = number,
                   Description = description,
               };
    }
}