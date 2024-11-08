namespace Modular.Framework.Domain.DomainContext;

public static class DomainContextAccessor
{
    private static readonly AsyncLocal<DomainContextHolder?> _current = new();

    private sealed class DomainContextHolder
    {
        public DomainContext? DomainContext;
    }

    public static DomainContext? Current
    {
        get => _current.Value?.DomainContext;
        set
        {
            var holder = _current.Value;
            if (holder != null)
            {
                holder.DomainContext = null;
            }

            if (value != null)
            {
                _current.Value = new DomainContextHolder() {DomainContext = value};
            }
        }
    }
}