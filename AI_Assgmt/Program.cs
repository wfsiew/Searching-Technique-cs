using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Please note that you may set the initial state, goal state,");
        Console.WriteLine("and depth limit in the config.xml file.\n");

        Utils.SetPuzzle();
        Utils.ShowPuzzle();

        StartDepthFirst(Utils.initstate);
        StartBreadthFirst(Utils.initstate);
        StartBestFirst(Utils.initstate);

        Console.WriteLine("\nPress any key to continue . . .");
        Console.ReadKey();
    }

    static void StartDepthFirst(int initstate)
    {
        try
        {
            DepthFirst o = new DepthFirst(initstate);
            o.Depth = Utils.depthlimit;
            Console.Write("\rDepth First Search starts . . . Please wait");
            o.Solve();
        }

        catch (Exception e)
        {
            HandleException(e);
        }
    }

    static void StartBreadthFirst(int initstate)
    {
        try
        {
            BreadthFirst o = new BreadthFirst(initstate);
            Console.Write("\rBreadth First Search starts . . . Please wait");
            o.Solve();
        }

        catch (Exception e)
        {
            HandleException(e);
        }
    }

    static void StartBestFirst(int initstate)
    {
        try
        {
            BestFirst o = new BestFirst(initstate);
            Console.Write("\rBest First Search starts . . . Please wait");
            o.Solve();
        }

        catch (Exception e)
        {
            HandleException(e);
        }
    }

    static void HandleException(Exception e)
    {
        Console.WriteLine();
        Console.WriteLine(e.ToString());
    }
}