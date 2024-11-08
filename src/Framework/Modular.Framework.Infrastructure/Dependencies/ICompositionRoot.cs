using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modular.Framework.Infrastructure.Mediator;

namespace Modular.Framework.Infrastructure.Dependencies;

public interface ICompositionRoot
{
    IScope BeginScope();
}

public interface IScope : IMessageHandlerScope, IDisposable, IAsyncDisposable
{
    public ILoggerFactory GetLoggerFactory();
    DbContext GetDbContext();
    
}