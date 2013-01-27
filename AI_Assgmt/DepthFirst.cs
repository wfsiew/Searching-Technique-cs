using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Depth First Search class.
/// Uses Stack as open list, and Dictionary as close list.
/// </summary>
public class DepthFirst : BaseSolver
{
    // open list (stack)
    private Stack<State> openlist;
    // close list (dictionary)
    // Key = state value, Value = State object
    private Dictionary<int, State> closelist;
    // the depth limit of the search
    // the search won't go beyond this limit
    private int depth;

    public DepthFirst(int state) : base(state)
    {
        openlist = new Stack<State>();
        closelist = new Dictionary<int, State>();
        depth = 0;
    }

    public int Depth
    {
        set { depth = value; }
    }

    public override void Solve()
    {
        State activestate = new State(initstate);
        bool solved = false;

        watch.Reset();
        watch.Start();

        // add the initial state to the open list
        openlist.Push(activestate);
        // loops until the open list is empty
        while (openlist.Count > 0)
        {
            // remove the left most state from the open list
            activestate = openlist.Pop();
            // checks whether the state is goal state
            // if yes, add the state to the close list and exit the loop
            if (Utils.IsGoalState(activestate.StateValue))
            {
                solved = true;
                closelist.Add(activestate.StateValue, activestate);
                break;
            }

            else
            {
                // add the state to the close list
                if (!closelist.ContainsKey(activestate.StateValue))
                    closelist.Add(activestate.StateValue, activestate);

                // if the state level is still below the depth limit,
                // or the depth limit is 0 (no depth limit)
                // generate all the possible next states from the current state
                if (activestate.Level < depth || depth == 0)
                {
                    int[] nextstates = Utils.ResolveNextStates(activestate.StateValue);
                    for (int i = 0; i < nextstates.Length; i++)
                    {
                        State state = new State(nextstates[i]);
                        state.Parent = activestate;

                        // if the child state already exist in the open list or close list,
                        // skip it to prevent circular
                        if (!closelist.ContainsKey(nextstates[i]) && !openlist.Contains(state))
                        {
                            // checks whether the child state is goal state
                            // if yes, add the child state to the close list and exit the loop
                            if (Utils.IsGoalState(nextstates[i]))
                            {
                                solved = true;
                                closelist.Add(nextstates[i], state);
                                break;
                            }

                            else
                            {
                                // add the child state to the left end of the open list
                                openlist.Push(state);
                            }
                        }
                    }
                }

                // if the goal state has been found,
                // exit the loop
                if (solved)
                    break;
            }
        }

        watch.Stop();
        WriteSolution(solved);
    }

    protected override void WriteSolution(bool solved)
    {
        Console.WriteLine("\rTime elapsed for Depth First Search in milliseconds = {0}", watch.ElapsedMilliseconds);
        Console.WriteLine("States generated in Depth First Search = {0}", closelist.Count + openlist.Count);
        Console.WriteLine("States generated in open list = {0}", openlist.Count);
        Console.WriteLine("States generated in close list = {0}", closelist.Count);
        Console.WriteLine("Depth = {0}", depth);

        string dir = Directory.GetCurrentDirectory();
        string file = Path.Combine(dir, Utils.DepthFirstFile);

        if (!solved)
        {
            // if the goal state could not be found,
            // show the message which says it could not find the goal state at the specified depth limit, and
            // delete the previously generated solution file
            Console.WriteLine("Unable to find the goal state at depth {0}\n", depth);

            try
            {
                File.Delete(file);
            }

            catch
            {
            }
        }

        else
        {
            // writes the goal state path to a file,
            // starts from the goal state, until the initial state
            StreamWriter sw = null;

            try
            {
                State o = closelist[Utils.goalstate];
                List<string> ls = new List<string>();
                string s = null;

                Console.WriteLine("Goal state found at level {0}", o.Level);

                while (o != null)
                {
                    s = Utils.GetStateString(o.StateValue);
                    ls.Add(s);
                    o = o.Parent;
                }

                sw = new StreamWriter(file, false);

                for (int i = ls.Count - 1; i >= 0; i--)
                    sw.WriteLine(ls[i]);

                Console.WriteLine("Solution count (excluding initial state) = {0}", ls.Count - 1);
                Console.WriteLine("Please refer to {0} file for the solutions.\n", file);
            }

            catch (Exception e)
            {
                Console.WriteLine("Error occurred in generating the solution file.\n{0}", e.ToString());
            }

            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
    }
}