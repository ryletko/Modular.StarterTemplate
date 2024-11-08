using Autofac;
using Modular.Framework.Infrastructure.DomainEvents;

namespace Modular.Framework.Module.DomainEvents;

internal class DomainEventsModule() : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventsProvider>()
               .As<IDomainEventsProvider>()
               .InstancePerLifetimeScope();
        
        builder.RegisterType<DomainEventsDispatcher>()
               .As<IDomainEventsDispatcher>()
               .InstancePerLifetimeScope();
    }
}