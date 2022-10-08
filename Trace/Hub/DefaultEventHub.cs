using Trace.Events;
using Trace.Listeners;
using Trace.Queues;

namespace Trace.Hub;

/// <summary>
/// Base class that implements interface <see cref="IEventHub"/>.
/// </summary>
public sealed class DefaultEventHub : IEventHub
{
    /// <summary>
    /// List of all registered listeners.
    /// </summary>
    public readonly IReadOnlyList<IEventListener> EventListeners;

    /// <summary>
    /// A <see cref="ObservableConcurrentQueue{T}"/> queue of events awaiting processing.
    /// </summary>
    private readonly ObservableConcurrentQueue<IEvent> _events;

    /// <summary>
    /// An instance of class <see cref="Handlers.EventHandler"/> that waits for an event to be added to the queue and processes it.
    /// </summary>
    private readonly Handlers.EventHandler _eventHandler;

    public DefaultEventHub(IReadOnlyList<IEventListener> eventListeners)
    {
        EventListeners = eventListeners;
        _events = new();

        _eventHandler = new(_events, eventListeners);
    }

    /// <summary>
    /// Publishes the event.
    /// </summary>
    /// <param name="event">Event to be published.</param>
    public void Publish(IEvent @event) => _events.Enqueue(@event);
}
