namespace Modular.Framework.Domain.DomainContext;

public class DomainContext(string? userId, DateTimeOffset startedAt)
{
    public string? UserId { get; } = userId;
    public DateTimeOffset StartedAt { get; } = startedAt;
}