using Autofac;
using Modular.Framework.Application.Message;
using Modular.Framework.Infrastructure.DataAccess;
using Modular.Framework.Infrastructure.Decorators;
using Modular.Framework.Infrastructure.Dependencies;
using Modular.Framework.Infrastructure.Mediator;
using Modular.Framework.Module.Config;

namespace Modular.Framework.Module.Mediator;

internal class MediatorModule(ModuleContext moduleContext,
                              ICompositionRoot compositionRoot) : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGenericDecorator(typeof(DomainContextDecorator<>), typeof(IMessageHandler<>));
        builder.RegisterGenericDecorator(typeof(UnitOfWorkDecorator<>), typeof(IMessageHandler<>));
        builder.RegisterGenericDecorator(typeof(TransactionDecorator<>), typeof(IMessageHandler<>));

        builder.RegisterGenericDecorator(typeof(DomainContextDecorator<,>), typeof(IMessageHandler<,>));
        builder.RegisterGenericDecorator(typeof(UnitOfWorkDecorator<,>), typeof(IMessageHandler<,>));
        builder.RegisterGenericDecorator(typeof(TransactionDecorator<,>), typeof(IMessageHandler<,>));

        builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

        builder.RegisterType<Infrastructure.Mediator.Mediator>()
               .As<IMediator>()
               .SingleInstance();

        var messageHandlerMap = RegisterMessageHandlers(builder);

        builder.RegisterInstance(messageHandlerMap)
               .As<IMessageHandlerMap>()
               .SingleInstance();

        builder.RegisterType<MessageHandlerResolver>()
               .As<IMessageHandlerResolver>()
               .SingleInstance();
     
    }

    private IMessageHandlerMap RegisterMessageHandlers(ContainerBuilder builder)
    {
        var messageHandlerTypes = moduleContext.ApplicationAssembly
                                               .GetTypes()
                                               .SelectMany(type => type.GetInterfaces()
                                                                       .Select(i => new
                                                                                    {
                                                                                        MessageHandlerType = type,
                                                                                        Interface          = i
                                                                                    }))
                                               .Where(t => t.Interface.IsGenericType &&
                                                           (t.Interface.GetGenericTypeDefinition() == typeof(IMessageHandler<>) ||
                                                            t.Interface.GetGenericTypeDefinition() == typeof(IMessageHandler<,>)))
                                               .Select(x => new
                                                            {
                                                                MessageType             = x.Interface.GenericTypeArguments[0],
                                                                MessageHandlerType      = x.MessageHandlerType,
                                                                MessageHandlerInterface = x.Interface
                                                            });
        var messageHandlerMap = new MessageHandlerMap();
        foreach (var mt in messageHandlerTypes)
        {
            builder.RegisterType(mt.MessageHandlerType)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            messageHandlerMap.Add(compositionRoot, mt.MessageType, mt.MessageHandlerInterface);
        }

        return messageHandlerMap;
    }
}