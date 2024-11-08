namespace Modular.Framework.Application.Commands;

// for masstransit in mind
public class GuidResult(Guid guid)
{
    public Guid Guid { get; } = guid;
}