using Trace.Events;
using Trace.Listeners;
using Trace.Tests.Events;

namespace Trace.Tests.Listeners;

/// <summary>
/// A class that implements interface <see cref="IEventListener"/> and is needed for testing.
/// </summary>
internal sealed class TestEventListener : IEventListener
{
    public Type EventType => typeof(TestEvent);

    public event Action EventHandled = delegate { }; 

    public int HandledEventConut { get; private set; }

    public Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public void OnComplete(IEvent @event)
    {
        HandledEventConut++;
        EventHandled.Invoke();
    }

    public void OnError(Exception exception) { }
}
