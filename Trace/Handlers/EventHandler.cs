using System.Collections.Specialized;
using Trace.Events;
using Trace.Listeners;
using Trace.Queues;

namespace Trace.Handlers;

/// <summary>
/// A class that manages a <see cref="Task"/> that processes events.
/// </summary>
public sealed class EventHandler
{
    /// <summary>
    /// Indicates whether a worker <see cref="Task"/> is currently processing a message. 
    /// true if it handles the event and false if it doesn't.
    /// </summary>
    private bool _inProcessing = false;

    /// <summary>
    /// Prevents access to method <see cref="StartHandlingFirstEvent"/>at the same time from two or more threads.
    /// </summary>
    private readonly object _lockObject = new();

    /// <summary>
    /// A <see cref="ObservableConcurrentQueue{T}"/> queue of events awaiting processing.
    /// </summary>
    private readonly ObservableConcurrentQueue<IEvent> _queueEvents;

    /// <summary>
    /// Listeners that will receive events for processing.
    /// </summary>
    private readonly IReadOnlyList<IEventListener> _eventListeners;

    /// <summary>
    /// <see cref="TaskCompletionSource{TResult}"/> which blocks the eternal loop in method <see cref="Handle"/> when the <see cref="_queueEvents"/> is empty.
    /// </summary>
    private TaskCompletionSource<EventHandlerArgs> _startProcessing;

    public EventHandler(ObservableConcurrentQueue<IEvent> queueEvents, IReadOnlyList<IEventListener> eventListeners)
    {
        _startProcessing = new();
        _queueEvents = queueEvents;
        _eventListeners = eventListeners;

        queueEvents.CollectionChanged += OnCollectionChanged;

        Start();
    }

    /// <summary>
    /// Runs method <see cref="Handle"/> in a separate <see cref="Task"/>.
    /// </summary>
    private void Start() => Task.Run(Handle);

    /// <summary>
    /// Receives the first event from <see cref="_queueEvents"/> and sends it for processing 
    /// if the processing <see cref="Task"/> is not busy processing another event.
    /// </summary>
    private void StartHandlingFirstEvent()
    {
        lock (_lockObject)
        {
            if (_inProcessing || _queueEvents.Count == 0)
            {
                return;
            }

            _queueEvents.TryDequeue(out IEvent? @event);

            if (@event == null)
            {
                return;
            }

            IEventListener[] listeners = _eventListeners.Where(x => x.EventType == @event.GetType()).ToArray();
            EventHandlerArgs args = new(@event, listeners);

            _startProcessing.SetResult(args);
            _inProcessing = true;
        }
    }

    /// <summary>
    /// Starts a forever loop that waits for the <see cref="TaskCompletionSource{TResult}.SetResult(TResult)"/> call on 
    /// the <see cref="_startProcessing"/> property and starts delegating event handling.
    /// </summary>
    private void Handle()
    {
        while (true)
        {
            EventHandlerArgs args = _startProcessing.Task.Result;
            _startProcessing = new();

            foreach (IEventListener listener in args.Listeners)
            {
                listener.HandleAsync(args.Event).Wait();
                listener.OnComplete(args.Event);
            }

            _inProcessing = false;
            StartHandlingFirstEvent();
        }
    }

    /// <summary>
    /// Handles event <see cref="ObservableConcurrentQueue{T}.CollectionChanged"/>.
    /// </summary>
    /// <param name="sender">The object in which the event occurred.</param>
    /// <param name="e">A <see cref="NotifyCollectionChangedEventArgs"/> event args.</param>
    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            StartHandlingFirstEvent();
        }
    }
}
