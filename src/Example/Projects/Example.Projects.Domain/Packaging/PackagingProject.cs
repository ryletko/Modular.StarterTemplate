using Example.Projects.Domain.Packaging.Artworks;
using Example.Projects.Domain.Projects;
using Example.Shared;

namespace Example.Projects.Domain.Packaging;

public class PackagingProject : Project
{
    protected virtual List<PackagingArtwork> ArtworksSelf { get; private set; } = new();
    public IReadOnlyCollection<PackagingArtwork> Artworks => ArtworksSelf.AsReadOnly();

    protected PackagingProject()
    {
        
    }
    
    private PackagingProject(ProjectName projectName, string description) :
        base(ProjectTypeEnum.Packaging, projectName, description)
    {
    }

    public static PackagingProject CreateNew(ProjectName projectName, string description)
    {
        return new PackagingProject(projectName, description);
    }
}