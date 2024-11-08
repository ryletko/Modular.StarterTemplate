namespace Example.ReadModel.Model;

public partial class Projects_PackagingArtwork
{
    public Guid Id { get; set; }

    public string ArtworkNumber { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid? ProjectId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public virtual Projects_Project? Project { get; set; }
}
