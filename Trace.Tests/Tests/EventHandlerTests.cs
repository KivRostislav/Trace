using Trace.Events;
using Trace.Listeners;
using Trace.Queues;
using Trace.Tests.Events;
using Trace.Tests.Listeners;

namespace Trace.Tests.Tests;

/// <summary>
/// A class that contains test methods for class <see cref="EventHandler"/>.
/// </summary>
public sealed class EventHandlerTests
{
    [Fact]
    public async void Enqueue_Events_AllEventsHandled()
    {
        List<TestEvent> events = CreateEvents(1_000).ToList();
        TestEventListener listener = new();

        TaskCompletionSource allEventsHandled = new();

        ObservableConcurrentQueue<IEvent> queueEvents = new();
        List<IEventListener> listeners = new() { listener };

        Handlers.EventHandler eventHandler = new(queueEvents, listeners);

        listener.EventHandled += () => {
            if (listener.HandledEventConut == events.Count)
            {
                allEventsHandled.SetResult();
            }
        };


        foreach (TestEvent @event in events)
        {
            queueEvents.Enqueue(@event);
        }


        await allEventsHandled.Task;
    }

    /// <summary>
    /// Generates the specified number of empty instances of class <see cref="TestEvent"/>.
    /// </summary>
    /// <param name="count">Number of elements to be created.</param>
    /// <returns>Returns a <see cref="IEnumerable{T}"/> of empty events, the number of which is set by the parameter count/>.</returns>
    private static IEnumerable<TestEvent> CreateEvents(int count)
    {
        List<TestEvent> events = new();

        if (count <= 0)
        {
            return events;
        }

        for (int i = 0; i < count; i++)
        {
            events.Add(new());
        }

        return events;
    }
}
