using Modular.Framework.Application.Commands;

namespace Example.Projects.Contracts.Commands;

public record ChangeProjectStatus(Guid ProjectId,
                                  decimal ProjectStatusCode,
                                  string Notes) : CommandBase;