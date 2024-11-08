using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modular.Framework.Infrastructure.Dependencies;

namespace Modular.Framework.Module.Config;

public class CompositionRoot(Func<IContainer> container): ICompositionRoot
{
    public IScope BeginScope()
    {
        return new Scope(container().BeginLifetimeScope());
    }
}

public class Scope(ILifetimeScope autofacScope): IScope
{
    
    public ILoggerFactory GetLoggerFactory()
    {
        return autofacScope.Resolve<ILoggerFactory>();
    }
    
    public DbContext GetDbContext()
    {
        return autofacScope.Resolve<DbContext>();
    }

    public async ValueTask DisposeAsync()
    {
        await autofacScope.DisposeAsync();
    }

    
    public object GetMessageHandler(Type messageHandlerType)
    {
        return autofacScope.Resolve(messageHandlerType);
    }
    
    public void Dispose()
    {
        autofacScope.Dispose();
    }

}