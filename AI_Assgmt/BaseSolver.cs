using System.Diagnostics;

/// <summary>
/// Base class.
/// </summary>
public abstract class BaseSolver
{
    /// <summary>
    /// The initial state of the 8 puzzle.
    /// </summary>
    protected int initstate;
    /// <summary>
    /// Used to capture the time taken for the Solve method to find the goal state.
    /// </summary>
    protected Stopwatch watch;

    public BaseSolver(int state)
    {
        initstate = state;
        watch = new Stopwatch();
    }

    public abstract void Solve();
    protected abstract void WriteSolution(bool solved);
}