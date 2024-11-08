using Modular.Framework.Application.Context;

namespace Modular.Framework.Infrastructure.AppContext;

public class AppContext(string? userId,
                        DateTimeOffset startedAt) : IAppContext
{
    public string? UserId { get; } = userId;
    public DateTimeOffset StartedAt { get; } = startedAt;
}