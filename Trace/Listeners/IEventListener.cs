using Trace.Events;

namespace Trace.Listeners;

/// <summary>
/// Basic event listener interface.
/// </summary>
public interface IEventListener
{
    /// <summary>
    /// The type of event handled by the listener.
    /// </summary>
    Type EventType { get; }

    /// <summary>
    /// Process event type <see cref="IEvent"/> asynchronously.
    /// </summary>
    /// <param name="event">An <see cref="IEvent"/> event to be processed.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel this asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Will be called when an error occurs in method <see cref="HandleAsync(IEvent, CancellationToken)"/>.
    /// </summary>
    /// <param name="exception">An error occurred while processing the event.</param>
    void OnError(Exception exception);

    /// <summary>
    /// Will be called when method <see cref="HandleAsync(IEvent, CancellationToken)"/> processes the event.
    /// </summary>
    /// <param name="event">An event that is processed</param>
    void OnComplete(IEvent @event);
}