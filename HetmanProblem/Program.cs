using System;
using System.Diagnostics;

namespace HetmanProblem
{
    internal class Program
    {
        private static void Main()
        {
            for (int i = 0; i < 30; i++)
            {
                Game game = new Game(i);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                game.Solve();
                stopwatch.Stop();
                game.PrintSolutions();
                Console.WriteLine(i + ". " + "Execution time: " + stopwatch.ElapsedMilliseconds + "ms. ");
            }
        }
    }
}
