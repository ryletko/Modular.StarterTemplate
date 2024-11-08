using Autofac;

namespace Modular.Framework.Module.Config;

public interface IModuleStartupConfigurationBuilder
{
    IModuleStartupConfigurationBuilder RegisterServices(Action<ContainerBuilder> configServices);
}

internal interface IModuleStartupConfiguration
{
    Action<ContainerBuilder>? RegisterServicesAction { get; }
}

internal class ModuleStartupConfiguration : IModuleStartupConfigurationBuilder, IModuleStartupConfiguration
{
    public Action<ContainerBuilder>? RegisterServicesAction { get; private set; }

    public IModuleStartupConfigurationBuilder RegisterServices(Action<ContainerBuilder> configServices)
    {
        RegisterServicesAction = configServices;
        return this;
    }

    public IModuleStartupConfiguration Build(Action<IModuleStartupConfigurationBuilder>? config)
    {
        if (config != null)
            config(this);

        return this;
    }
}