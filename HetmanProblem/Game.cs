using System;
using System.Collections.Generic;
using System.Linq;

namespace HetmanProblem
{
    internal partial class Game
    {
        private int _boardSize;
        private List<Quarter> _quarters;
        private int _counter = 0;

        internal List<List<Point>> Solutions { get; set; } = new List<List<Point>>();

        public Game(int boardSize)
        {
            _boardSize = boardSize;
            _quarters = CreateQuarters();
        }

        private static List<Quarter> CreateQuarters()
        {
            return new List<Quarter>
            {
                new Quarter(0, 0, 4, 4),
                new Quarter(4, 0, 8, 4),
                new Quarter(0, 4, 4, 8),
                new Quarter(4, 4, 8, 8)
            };
        }
        private static List<Quarter> CreateColumnQuarters()
        {
            return new List<Quarter>
            {
                new Quarter( 0,0,  1, 8),
                new Quarter( 1,  0,2, 8),
                new Quarter( 2,  0,3, 8),
                new Quarter( 3,  0,4, 8),
                new Quarter( 4,  0,5, 8),
                new Quarter(5,  0,6, 8),
                new Quarter(6,  0,7, 8),
                new Quarter( 7,  0,8, 8)
            };
        }
        private void RefineSolutions()
        {
            var solutionSet = new List<List<Point>>();
            for (var i = 0; i < Solutions.Count; i++)
            {
                var counter = 0;
                for (var j = i + 1; j < Solutions.Count; j++)
                {
                    counter = 0;
                    for (var k = 0; k < _boardSize; k++)
                    {
                        for (var l = 0; l < _boardSize; l++)
                        {
                            if (Solutions[i][k].X == Solutions[j][l].X && Solutions[i][k].Y == Solutions[j][l].Y)
                            {
                                counter++;
                                break;
                            }
                        }
                    }
                    if (counter == _boardSize)
                    {
                        break;
                    }
                }
                if (counter != _boardSize)
                {
                    solutionSet.Add(Solutions[i]);
                }
            }

            Solutions = solutionSet;
        }
        private static IEnumerable<int[]> GenerateQuarterArrays(int num)
        {
            var rnd = new Random();
            int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3 };
            var quarters = new List<int[]>();
            for (var i = 0; i < num; i++)
            {
                quarters.Add(arr.OrderBy(x => rnd.Next()).ToArray());
            }

            return quarters;
        }

        public List<List<Point>> Solve2()
        {
            //_counter = 0;

            //_quarters = CreateColumnQuarters();

            var board = new BoardManager(_boardSize);
            PlaceHetman2(board, 0, new Stack<Point>());

            //RefineSolutions();
            //RotateSolutions();
            //RefineSolutions();

            return Solutions;
        }

        public void PrintSolutions()
        {
            var board = new BoardManager(_boardSize);
            Console.WriteLine("Found " + Solutions.Count + " solutions after rotation refinement");
            foreach (var item in Solutions)
            {
                board.PrintBoardWithHetmans(item.ToList());
            }
        }
        public List<List<Point>> Solve(int number, bool reset = false)
        {
            if (reset)
            {
                Solutions = new List<List<Point>>();
            }
            _counter = 0;
            var arrays = GenerateQuarterArrays(number).ToList();

            var board = new BoardManager();
            foreach (var array in arrays)
            {
                board = new BoardManager(_boardSize);
                PlaceHetman(board, 0, new Stack<Point>(), array);
                board = new BoardManager(_boardSize);
                PlaceHetmanReverse(board, 0, new Stack<Point>(), array);
            }

            //Console.WriteLine("Single hetman placements: " + _counter);

            //Console.WriteLine("Found " + _solutions.Count + " solutions before refinement");
            RefineSolutions();
            //Console.WriteLine("Found " + _solutions.Count + " solutions after refinement");
            RotateSolutions();
            //Console.WriteLine("Found " + _solutions.Count + " solutions after rotation");
            RefineSolutions();
            //Console.WriteLine("Found " + _solutions.Count + " solutions after rotation refinement");
            //foreach (var item in _solutions)
            //{
            //    board.PrintBoardWithHetmans(item.ToList());
            //}
            return Solutions;
        }

        private bool PlaceHetman2(BoardManager board, int depth, Stack<Point> currentHetmans)
        {
            if (depth == _boardSize)
            {
                var solution = currentHetmans.ToList();
                Solutions.Add(solution);
                return true;
            }

            var boardCopy = new bool[_boardSize, _boardSize];

            Array.Copy(board.Board, 0, boardCopy, 0, board.Board.Length);
            var tempBoard = new BoardManager(boardCopy);
            for (var i = depth == 0 ? 1 : 0; i < _boardSize; i++)
            {
                if (board.IsOccupied(i, depth))
                    continue;

                board.FillOccupiedPlaces(i, depth);

                _counter++;
                currentHetmans.Push(new Point { X = i, Y = depth });

                if (PlaceHetman2(board, depth + 1, currentHetmans)) return true;

                currentHetmans.Pop();

                // board.PrintBoardWithHetmans(currentHetmans.ToList());

                Array.Copy(tempBoard.Board, 0, board.Board, 0, tempBoard.Board.Length);
            }

            return false;
        }
        private void PlaceHetman(BoardManager board, int depth, Stack<Point> currentHetmans, IReadOnlyList<int> quarterArray)
        {
            if (depth == 8)
            {
                var solution = currentHetmans.ToList();
                Solutions.Add(solution);
                return;
            }

            Quarter currentQuarter = _quarters[quarterArray[depth]];
            var boardCopy = new bool[8, 8];

            Array.Copy(board.Board, 0, boardCopy, 0, board.Board.Length);
            var tempBoard = new BoardManager(boardCopy);
            for (var i = currentQuarter.StartX; i < currentQuarter.EndX; i++)
            {
                for (var j = currentQuarter.StartY; j < currentQuarter.EndY; j++)
                {
                    if (board.IsOccupied(j, i))
                        continue;

                    board.FillOccupiedPlaces(j, i);

                    _counter++;
                    currentHetmans.Push(new Point { X = j, Y = i });

                    board.PrintBoardWithHetmans(currentHetmans.ToList());

                    Console.ReadKey();

                    PlaceHetman(board, depth + 1, currentHetmans, quarterArray);

                    currentHetmans.Pop();
                    board.Board = tempBoard.Board;
                }
            }

        }
        private void PlaceHetmanReverse(BoardManager board, int depth, Stack<Point> currentHetmans, IReadOnlyList<int> quarterArray)
        {
            if (depth == 8)
            {
                var solution = currentHetmans.ToList();
                Solutions.Add(solution);
                return;
            };

            Quarter currentQuarter = _quarters[quarterArray[depth]];
            var boardCopy = new bool[8, 8];

            Array.Copy(board.Board, 0, boardCopy, 0, board.Board.Length);
            var tempBoard = new BoardManager(boardCopy);
            for (var i = currentQuarter.EndX - 1; i >= currentQuarter.StartX; i--)
            {
                for (var j = currentQuarter.EndY - 1; j >= currentQuarter.StartY; j--)
                {
                    if (board.IsOccupied(j, i))
                        continue;

                    board.FillOccupiedPlaces(j, i);
                    _counter++;
                    currentHetmans.Push(new Point { X = j, Y = i });

                    PlaceHetman(board, depth + 1, currentHetmans, quarterArray);

                    currentHetmans.Pop();
                    board.Board = tempBoard.Board;
                }
            }
        }
        private void RotateSolutions()
        {
            var newSolutions = new List<List<Point>>();
            foreach (var item in Solutions)
            {
                var a = RotatePoints(item);
                var b = RotatePoints(a);
                var c = RotatePoints(b);
                newSolutions.Add(item);
                newSolutions.Add(a);
                newSolutions.Add(b);
                newSolutions.Add(c);
            }

            Solutions = newSolutions;
        }
        private List<Point> RotatePoints(IEnumerable<Point> hetmanPositions)
        {
            return hetmanPositions.Select(item => new Point { X = _boardSize - item.Y, Y = item.X }).ToList();
        }
    }
}