using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Trace.Queues;

/// <summary>
/// A <see cref="ConcurrentQueue{T}"/> implementation of a queue that tracks changes to itself
/// and notifies about it via the <see cref="CollectionChanged"/> event.
/// </summary>
/// <typeparam name="T">The type of the elements contained in the queue</typeparam>
public class ObservableConcurrentQueue<T> : ConcurrentQueue<T>, INotifyCollectionChanged
{
    /// <summary>
    /// The event that is called when the queue changes.
    /// </summary>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    /// <summary>
    /// Adds an object to the end of the.
    /// </summary>
    /// <param name="item">The object to add to the end of the <see cref="ObservableConcurrentQueue{T}."/></param>
    public new void Enqueue(T item)
    {
        base.Enqueue(item);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Add, item));
    }

    /// <summary>
    /// Tries to remove and return the object at the beginning of the concurrent queue.
    /// </summary>
    /// <param name="item">When this method returns, if the operation was successful, the result contains the
    /// object removed. If no object was available to be removed, the value is unspecified.</param>
    /// <returns> true if an element was removed and returned from the beginning of the <see cref="ObservableConcurrentQueue{T}"/> successfully; otherwise, false.</returns>
    public new bool TryDequeue([MaybeNullWhen(false)] out T item)
    {
        bool result = base.TryDequeue(out item);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, item));
        return result;
    }

    /// <summary>
    /// Triggers event <see cref="CollectionChanged"./>
    /// </summary>
    /// <param name="e">A <see cref="NotifyCollectionChangedEventArgs"/> event args.</param>
    private void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);
}