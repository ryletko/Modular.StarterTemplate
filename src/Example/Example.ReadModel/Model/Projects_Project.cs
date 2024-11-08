namespace Example.ReadModel.Model;

public partial class Projects_Project
{
    public Guid Id { get; set; }

    public string ProjectName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int ProjectType { get; set; }

    public decimal Status { get; set; }

    public string Discriminator { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public virtual ICollection<Projects_PackagingArtwork> PackagingArtworks { get; set; } = new List<Projects_PackagingArtwork>();

    public virtual ICollection<Projects_Status> Statuses { get; set; } = new List<Projects_Status>();
}
