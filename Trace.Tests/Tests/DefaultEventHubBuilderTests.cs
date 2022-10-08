using Trace.Builders;
using Trace.Hub;
using Trace.Listeners;
using Trace.Tests.Listeners;

namespace Trace.Tests.Tests;

/// <summary>
/// A class that contains test methods for class <see cref="DefaultEventHubBuilder"/>.
/// </summary>
public sealed class DefaultEventHubBuilderTests
{
    [Fact]
    public void Build_RegisterListeners_GetEventHubWithRegisteredListeners()
    {
        IEventListener listener = new TestEventListener();

        DefaultEventHubBuilder builder = new();

        builder.Register(listener);


        DefaultEventHub eventHub = builder.Build();


        Assert.NotNull(eventHub);
        Assert.Contains(listener, eventHub?.EventListeners);
    }
}
