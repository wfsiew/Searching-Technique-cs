using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Best First Search class.
/// Uses PriorityQueue as open list, and Dictionary as close list.
/// </summary>
public class BestFirst : BaseSolver
{
    // open list
    private PriorityQueue openlist;
    // close list (dictionary)
    // Key = state value, Value = State object
    private Dictionary<int, State> closelist;

    public BestFirst(int state) : base(state)
    {
        openlist = new PriorityQueue(false);
        closelist = new Dictionary<int, State>();
    }

    public override void Solve()
    {
        State activestate = new State(initstate, Utils.GetHeuristicValue(initstate));
        bool solved = false;

        watch.Reset();
        watch.Start();

        // add the initial state to the open list
        openlist.Enqueue(activestate.EValue, activestate);
        // loops until the open list is empty
        while (openlist.Count > 0)
        {
            // since the open list is already sorted in ascending based on the EValue when a state is added,
            // so just remove the left most state from the open list
            activestate = openlist.Dequeue();
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
                // generate all the possible next states from the current state
                int[] nextstates = Utils.ResolveNextStates(activestate.StateValue);
                for (int i = 0; i < nextstates.Length; i++)
                {
                    State state = new State(nextstates[i], Utils.GetHeuristicValue(nextstates[i]));
                    state.Parent = activestate;

                    // checks whether the child state already exist in the open list
                    // if yes, the state in the open list will be replaced with the child state if
                    // the child state was reached by a shorter path
                    bool is_exist_on_openlist = openlist.IsRepetitionExist(state.EValue, state);
                    // checks whether the child state already exist in the close list
                    bool is_exist_on_closelist = closelist.ContainsKey(state.StateValue);

                    // if the child state not exist in both list, add the child state to the open list
                    if (!is_exist_on_closelist && !is_exist_on_openlist)
                        openlist.Enqueue(state.EValue, state);

                    // if the child state already exist in the close list and the child state
                    // was reached by a shorter path, then remove the state from the close list
                    // and add the child state to the open list
                    if (is_exist_on_closelist)
                    {
                        State o = closelist[state.StateValue];
                        if (o.Level > state.Level)
                        {
                            closelist.Remove(state.StateValue);
                            openlist.Enqueue(state.EValue, state);
                        }
                    }
                }

                // add the state to the close list
                if (!closelist.ContainsKey(activestate.StateValue))
                    closelist.Add(activestate.StateValue, activestate);
            }
        }

        watch.Stop();
        WriteSolution(solved);
    }

    protected override void WriteSolution(bool solved)
    {
        Console.WriteLine("\rTime elapsed for Best First Search in milliseconds = {0}", watch.ElapsedMilliseconds);
        Console.WriteLine("States generated in Best First Search = {0}", closelist.Count + openlist.Count);
        Console.WriteLine("States generated in open list = {0}", openlist.Count);
        Console.WriteLine("States generated in close list = {0}", closelist.Count);

        string dir = Directory.GetCurrentDirectory();
        string file = Path.Combine(dir, Utils.BestFirstFile);

        if (!solved)
        {
            // if the goal state does not exist from the initial state,
            // show the no solution message and
            // delete the previously generated solution file
            Utils.ShowNoSolutionMessage();

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