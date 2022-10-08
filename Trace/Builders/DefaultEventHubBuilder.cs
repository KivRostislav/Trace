using Trace.Hub;
using Trace.Listeners;

namespace Trace.Builders;

/// <summary>
/// Builder class for class <see cref="DefaultEventHub"/>.
/// </summary>
public sealed class DefaultEventHubBuilder : IEventHubBuilder<DefaultEventHub>
{
    /// <summary>
    /// List of all registered listeners.
    /// </summary>
    private readonly List<IEventListener> _eventListeners = new();

    /// <summary>
    /// Registers the listener.
    /// </summary>
    /// <param name="listener">Which listener needs to be registered.</param>
    public void Register(IEventListener listener) => _eventListeners.Add(listener);

    /// <summary>
    /// Unregisters the listener.
    /// </summary>
    /// <param name="listener">Which listener needs to be unregistered.</param>
    public void Unregister(IEventListener listener) => _eventListeners.Remove(listener);

    /// <summary>
    /// Creates an instance of a <see cref="DefaultEventHub"/> class with registered listeners.
    /// </summary>
    /// <returns>Returns a created instance of a <see cref="DefaultEventHub"/> class with registered listeners.</returns>
    public DefaultEventHub Build() => new(_eventListeners);
}
