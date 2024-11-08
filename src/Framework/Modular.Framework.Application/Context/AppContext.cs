namespace Modular.Framework.Application.Context;

public interface IAppContext
{
    public string? UserId { get; }
    public DateTimeOffset StartedAt { get; }
}

public static class AppContextExt
{
    public static string GetUserId(this IAppContext appContext)
    {
        return appContext.UserId!;
    }
}
