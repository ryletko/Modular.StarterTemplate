namespace Example.ReadModel.Model;

public partial class Projects_Status
{
    public Guid Id { get; set; }

    public Guid? ProjectId { get; set; }

    public decimal StatusCode { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public virtual Projects_Project? Project { get; set; }
}
