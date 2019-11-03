using System;
using System.Diagnostics;

namespace HetmanProblem
{
    internal class Program
    {
        private static void Main()
        {
            Game game = new Game();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            game.Solve();
            
            stopwatch.Stop();
            Console.WriteLine("Execution time: " + stopwatch.ElapsedMilliseconds + "ms. ");
            Console.WriteLine("Found " + game.Solutions.Count + " solutions after rotation refinement");
        }
    }
}
