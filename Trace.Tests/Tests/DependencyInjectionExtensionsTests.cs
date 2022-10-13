using Microsoft.Extensions.DependencyInjection;
using Trace.Hub;
using Trace.Tests.EventHubBuilders;

namespace Trace.Tests.Tests;

/// <summary>
/// A class that contains test methods for class <see cref="DependencyInjectionExtensions"/>.
/// </summary>
public sealed class DependencyInjectionExtensionsTests
{
    [Fact]
    public void AddDefaultEventHub_DefaultEventHubBuilder_GetDefaultEventHubFormServices()
    {
        IServiceCollection services = new ServiceCollection();


        services.AddDefaultEventHub();


        IServiceProvider provider = services.BuildServiceProvider();
        IEventHub hub = provider.GetRequiredService<IEventHub>();

        Assert.IsType<DefaultEventHub>(hub);
    }

    [Fact]
    public void AddEventHub_TestEventHubBuilder_GetDefaultEventHubFormServices()
    {
        IServiceCollection services = new ServiceCollection();


        services.AddEventHub<TestEventHubBuilder, DefaultEventHub>(new());


        IServiceProvider provider = services.BuildServiceProvider();
        IEventHub hub = provider.GetRequiredService<IEventHub>();

        Assert.IsType<DefaultEventHub>(hub);
    }
}
