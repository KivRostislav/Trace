using Trace.Events;

namespace Trace.Hub;

/// <summary>
/// An interface for a class that is required for event publish.
/// </summary>
public interface IEventHub
{
    /// <summary>
    /// Publishes the event.
    /// </summary>
    /// <param name="event">Event to be published.</param>
    public void Publish(IEvent @event);
}
