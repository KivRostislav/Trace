using Trace.Builders;
using Trace.Listeners;
using Trace.Hub;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// An extension class to add <see cref="IEventHub"/> to <see cref="IServiceCollection"/>. 
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds <see cref="DefaultEventHub"/> as the base implementation of <see cref="IEventHub"/> in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddDefaultEventHub(this IServiceCollection services)
    {
        DefaultEventHub hub = ConfigureEventHub<DefaultEventHubBuilder, DefaultEventHub>(new(), services);

        services.AddSingleton<IEventHub>(hub);

        return services;
    }

    /// <summary>
    /// Adds its own implementation of <see cref="IEventHub"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TEventHubBuilder">Builder class type for TEventHub and that implements the <see cref="IEventHubBuilder{TEventHub}"/> interface.</typeparam>
    /// <typeparam name="TEventHub">The type of event hub that implements the <see cref="IEventHub"/> interface and that you want to add to the services.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="builder">An instance of class TEventHubBuilder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddEventHub<TEventHubBuilder, TEventHub>(this IServiceCollection services, TEventHubBuilder builder) where TEventHubBuilder : class, IEventHubBuilder<TEventHub> where TEventHub: IEventHub
    {
        TEventHub hub = ConfigureEventHub<TEventHubBuilder, TEventHub>(builder, services);

        services.AddSingleton<IEventHub>(hub);

        return services;
    }

    /// <summary>
    /// Configures the event hub, registers in it all classes that are registered in <see cref="IServiceCollection"/> as <see cref="IEventListener"/>.
    /// </summary>
    /// <typeparam name="TEventHubBuilder">Builder class type for TEventHub and that implements the <see cref="IEventHubBuilder{TEventHub}"/> interface.</typeparam>
    /// <typeparam name="TEventHub">The type of event hub that implements the <see cref="IEventHub"/> interface and that you want to configure.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="builder">An instance of class TEventHubBuilder.</param>
    /// <returns>Returns the configured instance of class TEventHub.</returns>
    private static TEventHub ConfigureEventHub<TEventHubBuilder, TEventHub>(TEventHubBuilder builder, IServiceCollection services) where TEventHubBuilder : class, IEventHubBuilder<TEventHub> where TEventHub : IEventHub
    {
        IServiceProvider provider = services.BuildServiceProvider();

        IEnumerable<IEventListener> listeners = provider.GetServices<IEventListener>();

        foreach (IEventListener listener in listeners)
        {
            builder.Register(listener);
        }

        return builder.Build();
    }
}
