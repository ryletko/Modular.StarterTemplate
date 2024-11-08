using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modular.Framework.Application.Context;
using Modular.Framework.Infrastructure.AppContext;
using Modular.Framework.Infrastructure.DataAccess;
using Modular.Framework.Module.DataAccess;
using Modular.Framework.Module.DomainEvents;
using Modular.Framework.Module.Logging;
using Modular.Framework.Module.Mediator;
using Modular.Utils;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Modular.Framework.Module.Config;

internal interface IServicesModuleConfigurator
{
    IAutoFacBuilderConfigurator ToAutofac();
}

internal interface IAutoFacBuilderConfigurator
{
    IAutoFacBuilderConfiguratorWithLoggerFactory RegisterLoggerFactory();
    IAutoFacBuilderConfigurator RegisterAssembliesServices();
    IFinalizedConfig FinalizeConfig();
    IAutoFacBuilderConfigurator RegisterAppServices();
    IAutoFacBuilderConfigurator RegisterDomainEventsHandling();
    
}

internal interface IAutoFacBuilderConfiguratorWithLoggerFactory : IAutoFacBuilderConfigurator
{
    IAutoFacBuilderConfiguratorWithDataAccess RegisterDataAccess();
}

internal interface IAutoFacBuilderConfiguratorWithDataAccess : IAutoFacBuilderConfiguratorWithLoggerFactory
{
    IAutoFacBuilderConfiguratorWithDataAccess RegisterMediator();
}

internal interface IFinalizedConfig
{
    IContainer Container { get; }
    ILogger ModuleLogger { get; }
    ModuleContext ModuleContext { get; }
    IModuleStartupConfiguration Config { get; }
}

internal class ModuleConfigurator : IServicesModuleConfigurator, IAutoFacBuilderConfigurator, IAutoFacBuilderConfiguratorWithLoggerFactory, IAutoFacBuilderConfiguratorWithDataAccess, IFinalizedConfig
{
    private readonly ServiceCollection _services;
    private readonly IModuleStartupConfiguration _config;
    private readonly ILogger _moduleLogger;
    private readonly ModuleContext _moduleContext;
    private readonly Type _dbContextType;
    private readonly string _connectionString;
    private ContainerBuilder _builder;
    private ILoggerFactory _loggerFactory;
    private IContainer _container;

    private ModuleConfigurator(ServiceCollection services, IModuleStartupConfiguration config, ILogger moduleLogger, ModuleContext moduleContext, Type dbContextType, string connectionString)
    {
        _services         = services;
        _config           = config;
        _moduleLogger     = moduleLogger;
        _moduleContext    = moduleContext;
        _dbContextType    = dbContextType;
        _connectionString = connectionString;
    }

    public IAutoFacBuilderConfiguratorWithLoggerFactory RegisterLoggerFactory()
    {
        _builder.RegisterModule(new LoggingModule(_moduleLogger));
        _loggerFactory = new SerilogLoggerFactory(_moduleLogger);

        _builder.RegisterInstance(_loggerFactory).As<ILoggerFactory>().SingleInstance();

        return this;
    }

    public IAutoFacBuilderConfiguratorWithDataAccess RegisterDataAccess()
    {
        var dataAccessModuleType = typeof(DataAccessModule<>).MakeGenericType(_dbContextType);
        var dataAccessModule = (IModule) Activator.CreateInstance(dataAccessModuleType, new DataAccessModuleParameters(_connectionString, _loggerFactory, _moduleContext))!;
        _builder.RegisterModule(dataAccessModule);

        return this;
    }

    public IAutoFacBuilderConfiguratorWithDataAccess RegisterMediator()
    {
        var mediatorModule = new MediatorModule(_moduleContext, new CompositionRoot(() => _container));
        _builder.RegisterModule(mediatorModule);
        return this;
    }

    public IAutoFacBuilderConfigurator RegisterAppServices()
    {
        _builder.Register(c => AppContextAccessor.Current)
                .As<IAppContext>()
                .InstancePerLifetimeScope();

        return this;
    }

    public IAutoFacBuilderConfigurator RegisterDomainEventsHandling()
    {
        _builder.RegisterModule(new DomainEventsModule());
        return this;
    }

    public IAutoFacBuilderConfigurator RegisterAssembliesServices()
    {
        _builder.RegisterAssemblyTypes([_moduleContext.InfrastructureAssembly, _moduleContext.ApplicationAssembly]).AsImplementedInterfaces().InstancePerLifetimeScope();
        return this;
    }


    public IFinalizedConfig FinalizeConfig()
    {
        _builder.RegisterInstance(_moduleContext).SingleInstance();
        _config.RegisterServicesAction?.Apply(publicServiceRegistration => publicServiceRegistration(_builder));
        _container = _builder.Build();
        return this;
    }

    public IAutoFacBuilderConfigurator ToAutofac()
    {
        var factory = new AutofacServiceProviderFactory();
        _builder = factory.CreateBuilder(_services);
        return this;
    }

    public static IServicesModuleConfigurator CreateConfigurator(string connectionString,
                                                                 ILogger logger,
                                                                 string schema,
                                                                 string moduleName,
                                                                 Assembly infrastructureAssembly,
                                                                 Assembly applicationAssembly,
                                                                 Action<IModuleStartupConfigurationBuilder>? config = null)
    {
        var moduleStartupConfig = new ModuleStartupConfiguration().Build(config);
        var moduleLogger = logger.ForContext("Module", moduleName);
        var moduleContext = new ModuleContext()
                            {
                                ModuleName             = moduleName,
                                SchemaName             = schema,
                                ApplicationAssembly    = applicationAssembly,
                                InfrastructureAssembly = infrastructureAssembly,
                            };
        var dbContextType = infrastructureAssembly.GetTypes().First(t => t.IsSubclassOf(typeof(BaseDbContext)));
        var services = new ServiceCollection();
        return new ModuleConfigurator(services, moduleStartupConfig, moduleLogger, moduleContext, dbContextType, connectionString);
    }

    public IContainer Container => _container;
    public ILogger ModuleLogger => _moduleLogger;
    public ModuleContext ModuleContext => _moduleContext;
    public IModuleStartupConfiguration Config => _config;
}