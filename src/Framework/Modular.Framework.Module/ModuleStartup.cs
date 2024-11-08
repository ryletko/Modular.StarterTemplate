using System.Reflection;
using Autofac;
using Modular.Framework.Module.Config;
using Modular.Framework.Module.DataAccess;
using Modular.Utils;
using Serilog;

namespace Modular.Framework.Module;

public class ModuleStartup
{
    private ModuleStartup(IContainer container, ModuleContext moduleContext, IModuleStartupConfiguration config, ILogger moduleLogger)
    {
        
        _container     = container;
        _moduleContext = moduleContext;
        _config        = config;
        _moduleLogger  = moduleLogger;
    }

    private readonly IContainer _container;
    private readonly ILogger _moduleLogger;
    private readonly ModuleContext _moduleContext;
    private readonly IModuleStartupConfiguration _config;

    public static ModuleStartup Configure(string connectionString,
                                          ILogger logger,
                                          string schema,
                                          string moduleName,
                                          Assembly infrastructureAssembly,
                                          Assembly applicationAssembly,
                                          Action<IModuleStartupConfigurationBuilder>? config = null)
    {
        return ModuleConfigurator.CreateConfigurator(connectionString,
                                                     logger,
                                                     schema,
                                                     moduleName,
                                                     infrastructureAssembly,
                                                     applicationAssembly,
                                                     config)
                                 .ToAutofac()
                                 .RegisterDomainEventsHandling()
                                 .RegisterAppServices()
                                 .RegisterLoggerFactory()
                                 .RegisterDataAccess()
                                 .RegisterMediator()
                                 .RegisterAssembliesServices()
                                 .FinalizeConfig()
                                 .Map(c => new ModuleStartup(c.Container,
                                                             c.ModuleContext,
                                                             c.Config,
                                                             c.ModuleLogger));
    }

    public async Task<ModuleStartup> Start()
    {
        Migrator.ApplyDbMigrations(new CompositionRoot(() => _container));
        _moduleLogger.Information("Module started");
        return this;
    }

    public async Task Stop()
    {
        _moduleLogger.Information("Module stopped");
    }
}