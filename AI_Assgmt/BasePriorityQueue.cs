using System;
using System.Collections.Generic;

/// <summary>
/// Base class of priority queue, where an element in the queue is QueueItem object.
/// </summary>
public class BasePriorityQueue<T> where T : class
{
    /// <summary>
    /// A list of QueueItem.
    /// </summary>
    protected LinkedList<QueueItem<T>> items;
    /// <summary>
    /// A flag to determine whether the queue allows repetition of the same item.
    /// </summary>
    private bool allowrepeat;

    public BasePriorityQueue() : this(true)
    {
    }

    public BasePriorityQueue(bool _allowrepeat)
    {
        items = new LinkedList<QueueItem<T>>();
        allowrepeat = _allowrepeat;
    }

    /// <summary>
    /// Adds an object to the queue, and place the object in the queue
    /// based on the key.
    /// Object with key will be at the left end in the queue.
    /// </summary>
    public void Enqueue(int key, T obj)
    {
        if (!allowrepeat)
        {
            if (IsRepetitionExist(key, obj))
                return;
        }

        LinkedListNode<QueueItem<T>> node = new LinkedListNode<QueueItem<T>>(new QueueItem<T>(key, obj));
        if (items.Count == 0)
            items.AddFirst(node);

        else
        {
            LinkedListNode<QueueItem<T>> current = items.First;
            while (current != null)
            {
                if (node.Value.GetKey() <= current.Value.GetKey())
                {
                    items.AddBefore(current, node);
                    break;
                }

                current = current.Next;
            }

            if (current == null)
                items.AddLast(node);
        }
    }

    /// <summary>
    /// Removes the left most object from the queue.
    /// </summary>
    /// <returns>The left most object in the queue.</returns>
    public T Dequeue()
    {
        if (items.Count == 0)
            return default(T);

        else
        {
            T obj = items.First.Value.GetObj();
            items.RemoveFirst();
            return obj;
        }
    }

    /// <summary>
    /// Gets the number of objects actually contained in the queue.
    /// </summary>
    public int Count
    {
        get { return items.Count; }
    }

    /// <summary>
    /// Checks whether there is repetition of the key in the queue.
    /// </summary>
    /// <returns>True if there is repetition of the key, False otherwise.</returns>
    public virtual bool IsRepetitionExist(int key, T obj)
    {
        if (items.Count == 0)
            return false;

        LinkedListNode<QueueItem<T>> current = items.First;

        int repetition = 0;
        while (current != null || (current != null && current.Value.GetKey() > key))
        {
            if (current.Value.GetKey() == key)
                ++repetition;

            if (repetition > 1)
                break;

            current = current.Next;
        }

        if (repetition < 2)
            return false;

        else
            return true;
    }

    /// <summary>
    /// Gets/Sets the allow repeat mode of the queue.
    /// </summary>
    public bool AllowRepeat
    {
        get { return allowrepeat; }
        set { allowrepeat = value; }
    }

    /// <summary>
    /// Replaces an existing object in the queue with a new object.
    /// </summary>
    public void Replace(LinkedListNode<QueueItem<T>> oldobj, QueueItem<T> newobj)
    {
        items.AddAfter(oldobj, newobj);
        items.Remove(oldobj);
    }

    /// <summary>
    /// Gets the actual queue.
    /// </summary>
    public LinkedList<QueueItem<T>> List
    {
        get { return items; }
    }
}