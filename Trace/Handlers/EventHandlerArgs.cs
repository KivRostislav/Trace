using Trace.Events;
using Trace.Listeners;

namespace Trace.Handlers;

/// <summary>
/// The class of arguments that are required for event processing.
/// </summary>
internal sealed class EventHandlerArgs
{
    /// <summary>
    /// Event to be processed.
    /// </summary>
    public IEvent Event { get; init; }

    /// <summary>
    /// Listeners to which processing of event <see cref="Event"/> will be handed over
    /// </summary>
    public IEnumerable<IEventListener> Listeners { get; init; }

    public EventHandlerArgs(IEvent @event, IEnumerable<IEventListener> listeners)
    {
        Event = @event;
        Listeners = listeners;
    }
}
