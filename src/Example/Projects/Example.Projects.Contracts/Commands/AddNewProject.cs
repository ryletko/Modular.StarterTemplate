using Example.Shared;
using Modular.Framework.Application.Commands;

namespace Example.Projects.Contracts.Commands;

public record AddNewProject(string ProjectName, string Description, ProjectTypeEnum ProjectType): CommandBase<GuidResult>;
