namespace Example.Module;

public interface IExampleAppModule
{
    IExampleAppModule Configure();
    Task<IExampleAppModule> Start();
    Task Stop();
}