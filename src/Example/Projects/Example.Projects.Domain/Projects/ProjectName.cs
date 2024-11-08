using Modular.Framework.Domain.Patterns;

namespace Example.Projects.Domain.Projects;

public class ProjectName : ValueObject
{
    protected ProjectName() {}
    
    private ProjectName(string text)
    {
        // validation 

        if (String.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Project name cannot be empty", nameof(text));

        Text = text;
    }

    public string Text { get; }

    public override string ToString() => Text;

    public static ProjectName FromString(string definition) => new ProjectName(definition);
}