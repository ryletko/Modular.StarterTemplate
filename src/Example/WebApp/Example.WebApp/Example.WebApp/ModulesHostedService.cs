using Example.Module;
using Example.Projects.Module;
using ILogger = Serilog.ILogger;

namespace Example.WebApp;

public class ModulesHostedService(ConnectionString connectionString,
                                  ILogger logger) : IHostedService
{
    private List<IExampleAppModule> ModuleStartups;

    private async Task StartModules()
    {
        foreach (var moduleStartup in ModuleStartups)
        {
            await moduleStartup.Start();
        }
    }

    private void ConfigureModules()
    {
        foreach (var moduleStartup in ModuleStartups)
        {
            moduleStartup.Configure();
        }
    }

    public ModulesHostedService Configure()
    {
        ModuleStartups =
        [
            new ProjectsModuleStartup(connectionString.Value, logger),
        ];

        ConfigureModules();
        return this;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await StartModules();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var moduleStartup in ModuleStartups)
        {
            await moduleStartup.Stop();
        }
    }
}