using Trace.Builders;
using Trace.Hub;
using Trace.Listeners;

namespace Trace.Tests.EventHubBuilders;

/// <summary>
/// A class that implements interface <see cref="IEventHubBuilder{TEventHub}"/> and is needed for testing.
/// </summary>
internal sealed class TestEventHubBuilder : IEventHubBuilder<DefaultEventHub>
{
    public DefaultEventHub Build() => new(new List<IEventListener>().AsReadOnly());

    public void Register(IEventListener listener) { }

    public void Unregister(IEventListener listener) { }
}
