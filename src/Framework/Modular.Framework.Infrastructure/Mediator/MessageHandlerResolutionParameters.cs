using Modular.Framework.Infrastructure.Dependencies;

namespace Modular.Framework.Infrastructure.Mediator;

public class MessageHandlerResolutionParameters(ICompositionRoot compositionRoot, Type messageHandlerType)
{
    public ICompositionRoot CompositionRoot { get; } = compositionRoot;
    public Type MessageHandlerType { get; } = messageHandlerType;
}