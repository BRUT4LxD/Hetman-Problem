using System;
using System.Diagnostics;

namespace HetmanProblem
{
    internal class Program
    {
        private static void Main()
        {
            for (int i = 0; i < 36; i++)
            {
                Game game = new Game(i);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //game.Solve(9);
                game.Solve2();
                stopwatch.Stop();
                Console.WriteLine(i + ". " + "Execution time: " + stopwatch.ElapsedMilliseconds + "ms. ");
                // Console.WriteLine("Found " + game.Solutions.Count + " solutions after rotation refinement");
            }
            
            //Console.ReadKey();
            //game.PrintSolutions();
        }
    }
}
