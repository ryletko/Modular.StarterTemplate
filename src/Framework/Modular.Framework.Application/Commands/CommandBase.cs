namespace Modular.Framework.Application.Commands;

public abstract record CommandBase(Guid Id) : ICommand
{
    protected CommandBase() : this(Guid.NewGuid())
    {
    }
}

public abstract record CommandBase<TResult>(Guid Id) : ICommand<TResult>
    where TResult : class
{
    protected CommandBase() : this(Guid.NewGuid())
    {
    }
}