using Ardalis.SmartEnum;

namespace Example.Shared;

public class ProjectStatusCode : SmartEnum<ProjectStatusCode, decimal>
{
    public static ProjectStatusCode RequestedByClient = new(0.5m, "Requested by client");
    public static ProjectStatusCode ProjectSubmitted = new(1, "Project submitted");
    public static ProjectStatusCode ArtworkInProcess = new(2, "Request to designer. Artwork in process.");
    public static ProjectStatusCode ArtworkComplete = new(3, "Artwork complete.");
    public static ProjectStatusCode Complete = new(5, "Artwork finalized, project complete.");

    private ProjectStatusCode(decimal id, string name) : base(name, id)
    {
        Id   = id;
        Name = name;
    }

    public decimal Id { get; }
    public string Name { get; }

    public ProjectStatusCode? Next()
    {
        var returnNext = false; 
        foreach (var l in List)
        {
            if (returnNext)
                return l;

            if (l == this)
            {
                returnNext = true;
            }
        }

        return null;
    }
}