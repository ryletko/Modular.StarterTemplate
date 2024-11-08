using Example.Module;
using Example.Projects.Application.CommandHandlers;
using Example.Projects.Infrastructure;
using Example.Projects.Infrastructure.DataAccess;
using Modular.Framework.Module;
using Serilog;

namespace Example.Projects.Module;

public class ProjectsModuleStartup(string connectionString,
                                   ILogger logger) : IExampleAppModule
{
    private ModuleStartup _module { get; set; }

    public IExampleAppModule Configure()
    {
        _module = ModuleStartup.Configure(connectionString,
                                          logger,
                                          Schema.Name,
                                          "Projects",
                                          typeof(IProjectsInfrastructure).Assembly,
                                          typeof(IProjectsApplication).Assembly);

        return this;
    }

    public async Task<IExampleAppModule> Start()
    {
        await _module.Start();
        return this;
    }

    public async Task Stop()
    {
        await _module.Stop();
    }
}

