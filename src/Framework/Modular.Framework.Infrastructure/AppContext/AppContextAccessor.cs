namespace Modular.Framework.Infrastructure.AppContext;

public static class AppContextAccessor
{
    private static readonly AsyncLocal<AppContextHolder?> _current = new();

    private sealed class AppContextHolder
    {
        public AppContext? AppContext;
        
    }

    public static AppContext? Current
    {
        get => _current.Value?.AppContext;
        set
        {
            var holder = _current.Value;
            if (holder != null)
            {
                holder.AppContext = null;
            }

            if (value != null)
            {
                _current.Value = new AppContextHolder() {AppContext = value};
            }
        }
    }
}