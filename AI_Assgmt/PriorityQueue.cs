using System;
using System.Collections.Generic;

/// <summary>
/// PriorityQueue class.
/// </summary>
public class PriorityQueue : BasePriorityQueue<State>
{
    public PriorityQueue(bool _allowrepeat) : base(_allowrepeat)
    {
    }

    /// <summary>
    /// Overrides the method from BasePriority class.
    /// It checks whether the same state exist in the queue.
    /// If yes, it compares the state's level found from the queue with the specified state's level.
    /// If the state from the queue is deeper, it will be replaced with the specified state.
    /// </summary>
    /// <returns>True if there is repetition of the state, False otherwise.</returns>
    public override bool IsRepetitionExist(int key, State obj)
    {
        bool exist = false;
        LinkedListNode<QueueItem<State>> current = items.First;
        // checks other nodes in the open list
        while (current != null)
        {
            if (current.Value.GetObj().StateValue == obj.StateValue)
            {
                // compares the level of two states
                if (current.Value.GetObj().Level > obj.Level)
                {
                    // replaced the old one if it's deeper than new one.
                    Replace(current, new QueueItem<State>(obj.EValue, obj));
                    exist = true;
                    break;
                }
            }

            current = current.Next;
        }

        return exist;
    }
}