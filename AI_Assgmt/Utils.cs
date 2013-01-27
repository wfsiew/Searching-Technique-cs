using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

/// <summary>
/// Helper class which provides all the necessary static methods used in the entire application.
/// </summary>
public class Utils
{
    /// <summary>
    /// The initial state of the puzzle.
    /// It will be overwritten by the init_state specified in the config.xml file.
    /// </summary>
    public static int initstate = 250816743;

    /// <summary>
    /// The goal state of the puzzle that the program will try to find.
    /// It will be overwritten by the goal_state specified in the config.xml file.
    /// </summary>
    public static int goalstate = 765408321;

    /// <summary>
    /// The depth limit that will be used in Depth First Search to search up to
	/// a certain level.
    /// It will be overwritten by the depth_limit specified in the config.xml file
    /// </summary>
    public static int depthlimit = 41;

    /// <summary>
    /// The goal state map array. It maps each digit in the goal state to row
	/// and column.
    /// Used for heuristic value calculation.
    /// </summary>
    private static byte[,] goalstatemap = new byte[9, 2];

    /// <summary>
    /// The file name for Breadth First Search output.
    /// The output is the path of the initial state to goal state (in reverse,
	/// goal state first).
    /// </summary>
    public const string BreadthFirstFile = "breadth_first.txt";

    /// <summary>
    /// The file name for Depth First Search output.
    /// The output is the path of the initial state to goal state (in reverse,
	/// goal state first).
    /// </summary>
    public const string DepthFirstFile = "depth_first.txt";

    /// <summary>
    /// The file name for Best First Search output.
    /// The output is the path of the initial state to goal state (in reverse,
	/// goal state first).
    /// </summary>
    public const string BestFirstFile = "best_first.txt";

    /// <summary>
    /// The message to be displayed if the goal state not found.
    /// </summary>
    public const string GoalStateNotFound = "Goal state does not exist";

    /// <summary>
    /// The movement constants.
    /// </summary>
    public enum Direction
    {
		Up, Down, Left, Right
    }

    /// <summary>
    /// Checks whether the given state is the goal state.
    /// </summary>
    /// <returns>True if the state is goal state, False otherwise.</returns>
    public static bool IsGoalState(int state)
    {
        return state == goalstate;
    }

    /// <summary>
    /// Gets the index of digit 0 from a given state.
    /// </summary>
    /// <returns>The index of digit 0.</returns>
    public static byte GetBlankIndex(int state)
    {
        return GetIndexOfDigit(state, 0);
    }

    /// <summary>
    /// Gets the index of a specified digit from a given state.
    /// </summary>
    /// <returns>The index of specified digit.</returns>
    public static byte GetIndexOfDigit(int state, int digit)
    {
        byte index = 0;
        while ((state % 10) != digit)
        {
            state /= 10;
            ++index;
            if (index == 8)
                break;
        }

        return index;
    }

    /// <summary>
    /// Gets a digit from a specified index from a given state.
    /// </summary>
    /// <returns>The digit from a specified index.</returns>
    public static int GetDigitAtIndex(int state, int index)
    {
        double x = Math.Pow(10, index);
        int q = (int)(state / x);
        return (q == 0 ? 0 : q % 10);
    }

    /// <summary>
    /// Sets a digit at the specified index.
    /// </summary>
    /// <returns>A new state with the digit sets at the specified index.</returns>
    public static int SetDigitAtIndex(int state, int index, int digit)
    {
        double x = Math.Pow(10, index);
        int q = (int)(state / x);
        int r = state % (int)x;
        int a = q / 10;
        int b = a * 10;
        int val = ((b + digit) * (int)x) + r;
        return val;
    }

    /// <summary>
    /// Swaps 2 digits and returns a new state.
    /// </summary>
    /// <returns>A new state with 2 digits being exchanged.</returns>
    public static int SwapDigit(int state, int index1, int index2)
    {
        int digit1 = GetDigitAtIndex(state, index1);
        int digit2 = GetDigitAtIndex(state, index2);
        int tmpstate = SetDigitAtIndex(state, index1, digit2);
        return SetDigitAtIndex(tmpstate, index2, digit1);
    }

    /// <summary>
    /// Moves the blank with the specified direction.
    /// </summary>
    /// <returns>A new state with a new blank position.</returns>
    public static int MoveBlank(int state, int blankindex, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return SwapDigit(state, blankindex, blankindex - 3);

            case Direction.Down:
                return SwapDigit(state, blankindex, blankindex + 3);

            case Direction.Left:
                return SwapDigit(state, blankindex, blankindex - 1);

            default:
                return SwapDigit(state, blankindex, blankindex + 1);
		}
    }

    /// <summary>
    /// Resolves the possible next states from a given state
    /// which determines by the current blank index.
    /// </summary>
    /// <returns>An array of possible next states.</returns>
    public static int[] ResolveNextStates(int state)
    {
        byte blankindex = GetBlankIndex(state);
        int[] states;

        switch (blankindex)
        {
            case 1:
                states = new int[3];
                states[0] = MoveBlank(state, blankindex, Direction.Left);
                states[1] = MoveBlank(state, blankindex, Direction.Right);
                states[2] = MoveBlank(state, blankindex, Direction.Down);
                return states;

            case 2:
                states = new int[2];
                states[0] = MoveBlank(state, blankindex, Direction.Left);
                states[1] = MoveBlank(state, blankindex, Direction.Down);
                return states;

            case 3:
	            states = new int[3];
	            states[0] = MoveBlank(state, blankindex, Direction.Up);
	            states[1] = MoveBlank(state, blankindex, Direction.Down);
	            states[2] = MoveBlank(state, blankindex, Direction.Right);
	            return states;

            case 4:
	            states = new int[4];
	            states[0] = MoveBlank(state, blankindex, Direction.Up);
	            states[1] = MoveBlank(state, blankindex, Direction.Left);
	            states[2] = MoveBlank(state, blankindex, Direction.Right);
	            states[3] = MoveBlank(state, blankindex, Direction.Down);
	            return states;

            case 5:
	            states = new int[3];
	            states[0] = MoveBlank(state, blankindex, Direction.Up);
	            states[1] = MoveBlank(state, blankindex, Direction.Down);
	            states[2] = MoveBlank(state, blankindex, Direction.Left);
	            return states;

            case 6:
	            states = new int[2];
	            states[0] = MoveBlank(state, blankindex, Direction.Up);
	            states[1] = MoveBlank(state, blankindex, Direction.Right);
	            return states;

            case 7:
	            states = new int[3];
	            states[0] = MoveBlank(state, blankindex, Direction.Up);
	            states[1] = MoveBlank(state, blankindex, Direction.Left);
	            states[2] = MoveBlank(state, blankindex, Direction.Right);
	            return states;

            case 8:
	            states = new int[2];
	            states[0] = MoveBlank(state, blankindex, Direction.Up);
	            states[1] = MoveBlank(state, blankindex, Direction.Left);
	            return states;

            default:
	            states = new int[2];
	            states[0] = MoveBlank(state, blankindex, Direction.Right);
	            states[1] = MoveBlank(state, blankindex, Direction.Down);
	            return states;
        }
    }

    /// <summary>
    /// Gets the heuristic value from a given state.
    /// It calculates the row offset and column offset from the goal state.
    /// The distance is row offset + column offset.
    /// </summary>
    /// <returns>The heuristic value from a given state.</returns>
    public static int GetHeuristicValue(int state)
    {
        int hval = 0;

        for (byte i = 0; i < 9; i++)
        {
	        int digit = GetDigitAtIndex(state, i);
	        if (digit == 0)
		        continue;

	        int row1 = i / 3;
	        int column1 = i % 3;
	        int row2 = goalstatemap[digit, 0];
	        int column2 = goalstatemap[digit, 1];

	        int rowoffset = Math.Abs(row1 - row2);
	        int columnoffset = Math.Abs(column1 - column2);
	        hval += rowoffset + columnoffset;
        }

        return hval;
    }

    /// <summary>
    /// Gets a string representation from a given state.
    /// e.g A state with 250816743 will be represented in
    ///
    /// 347
    /// 618
    ///  52
    /// </summary>
    /// <returns>The string representation from a given state.</returns>
    public static string GetStateString(int state)
    {
        StringBuilder sb = new StringBuilder();
        char[] c = new char[9];

        for (byte i = 0; i < c.Length; i++)
        {
	        int x = GetDigitAtIndex(state, i);
	        char tmp = (x == 0 ? ' ' : char.Parse(x.ToString()));
	        c[i] = tmp;
        }

        sb.AppendFormat("{0}{1}{2}" + Environment.NewLine, c[0], c[1], c[2]);
        sb.AppendFormat("{0}{1}{2}" + Environment.NewLine, c[3], c[4], c[5]);
        sb.AppendFormat("{0}{1}{2}" + Environment.NewLine, c[6], c[7], c[8]);

        return sb.ToString();
    }

    /// <summary>
    /// Displays the 8 puzzle in the console.
    /// </summary>
    public static void ShowPuzzle()
    {
        Console.WriteLine("Puzzle initial state =");
        Console.WriteLine(GetStateString(initstate));
        Console.WriteLine("Puzzle goal state =");
        Console.WriteLine(GetStateString(goalstate));
    }

    /// <summary>
    ///
    /// </summary>
    public static void ShowNoSolutionMessage()
    {
        Console.WriteLine("There is no solution exist.\n");
    }

    /// <summary>
    /// Initializes the 8 puzzle with initial state, goal state, and
	/// depth limit from config.xml file.
    /// </summary>
    public static void SetPuzzle()
    {
        try
        {
	        string dir = Directory.GetCurrentDirectory();
	        string file = Path.Combine(dir, "config.xml");
	        if (!File.Exists(file))
		        return;

	        XmlDocument doc = new XmlDocument();
	        doc.Load(file);
	        XmlNode initnode = doc.SelectSingleNode("/config/init_state");
	        XmlNode goalnode = doc.SelectSingleNode("/config/goal_state");
	        XmlNode depthnode = doc.SelectSingleNode("/config/depth_limit");

	        string init = ReverseString(initnode.InnerText);
	        string goal = ReverseString(goalnode.InnerText);
	        string depth = depthnode.InnerText;

	        int _initstate = int.Parse(init);
	        int _goalstate = int.Parse(goal);
	        int _depthlimit = int.Parse(depth);

	        initstate = _initstate;
	        goalstate = _goalstate;
	        depthlimit = _depthlimit;
        }

        catch
        {
        }

        SetGoalStateMap();
    }

    /// <summary>
    /// Initializes the goalstatemap array.
    /// It keeps the row and column for each digit in the goal state.
    /// </summary>
    private static void SetGoalStateMap()
    {
        for (byte i = 0; i < 9; i++)
        {
	        int digit = GetDigitAtIndex(goalstate, i);
	        goalstatemap[digit, 0] = (byte)(i / 3);
	        goalstatemap[digit, 1] = (byte)(i % 3);
        }
    }

    /// <summary>
    /// Reverses a string.
    /// </summary>
    /// <returns>A new string with the original string being reversed.</returns>
    private static string ReverseString(string s)
    {
        char[] c = s.ToCharArray();
        Array.Reverse(c);
        return new string(c);
    }
}