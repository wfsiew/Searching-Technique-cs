using System;

/// <summary>
/// A class which encapsulates a state in the 8 puzzle.
/// </summary>
public class State : IEquatable<State>
{
    /// <summary>
    /// The state value, represented in reversed order from the 9 slots in the 8 puzzle.
    /// </summary>
    private int state;
    /// <summary>
    /// The heuristic value of this state.
    /// </summary>
    private int heuristicvalue;
    /// <summary>
    /// The depth level of this state. The initial state will have level 0.
    /// </summary>
    private int level;
    /// <summary>
    /// The evaluation value of this state.
    /// EValue = depth level + heuristic value
    /// </summary>
    private int evalue;
    /// <summary>
    /// A reference of the parent state.
    /// </summary>
    private State parent;

    public State(int s) : this(s, 0)
    {
    }

    public State(int s, int hval)
    {
        state = s;
        heuristicvalue = hval;
        Parent = null;
    }

    /// <summary>
    /// Gets the state value.
    /// </summary>
    public int StateValue
    {
        get { return state; }
    }

    /// <summary>
    /// Gets/Sets the heuristic value.
    /// </summary>
    public int HeuristicValue
    {
        get { return heuristicvalue; }
        set { heuristicvalue = value; }
    }

    /// <summary>
    /// Gets the depth level.
    /// </summary>
    public int Level
    {
        get { return level; }
    }

    /// <summary>
    /// Gets the evaluation value.
    /// </summary>
    public int EValue
    {
        get { return evalue; }
    }

    /// <summary>
    /// Gets/Sets the reference of the parent state.
    /// </summary>
    public State Parent
    {
        get { return parent; }

        set 
        {
            parent = value;
            level = (value != null ? value.Level + 1 : 0);
            evalue = level + heuristicvalue;
        }
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <returns>True if the state of this object is equal to another object's state, False otherwise.</returns>
    public bool Equals(State other)
    {
        return state == other.state;
    }
}