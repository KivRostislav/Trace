using Trace.Hub;
using Trace.Listeners;

namespace Trace.Builders;

/// <summary>
/// Interface for the builder class of type <see cref="TEventHub"/>.
/// </summary>
/// <typeparam name="TEventHub">The type that should be returned after the build.</typeparam>
public interface IEventHubBuilder<TEventHub> where TEventHub : IEventHub
{
    /// <summary>
    /// List of all registered listeners.
    /// </summary>
    public void Register(IEventListener listener);

    /// <summary>
    /// Registers the listener.
    /// </summary>
    /// <param name="listener">Which listener needs to be registered.</param>
    public void Unregister(IEventListener listener);

    /// <summary>
    /// Creates an instance of a class of type <see cref="TEventHub"/> with registered listeners.
    /// </summary>
    /// <returns>Returns an instance of a class of type <see cref="TEventHub"/> with registered listeners.</returns>
    public TEventHub Build();
}
