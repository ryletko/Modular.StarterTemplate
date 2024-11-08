namespace Example.WebApp.Shared.Projects;

public class GetProjectsRq
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public decimal Status { get; set; }
}