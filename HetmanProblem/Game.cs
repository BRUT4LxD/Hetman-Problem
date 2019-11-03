using System;
using System.Collections.Generic;
using System.Linq;

namespace HetmanProblem
{
    internal partial class Game
    {
        private const int boardSize = 8;
        private List<List<Point>> _solutions = new List<List<Point>>();
        private readonly List<Quarter> _quarters;
        private int _counter = 0;

        internal List<List<Point>> Solutions { get => _solutions; set => _solutions = value; }

        public Game()
        {
            _quarters = new List<Quarter>
            {
                new Quarter(0, 0, 4, 4),
                new Quarter(4, 0, 8, 4),
                new Quarter(0, 4, 4, 8),
                new Quarter(4, 4, 8, 8)
            };
        }
        private void RefineSolutions()
        {
            List<List<Point>> solutionSet = new List<List<Point>>();
            for (int i = 0; i < _solutions.Count; i++)
            {
                int counter = 0;
                for (int j = i + 1; j < _solutions.Count; j++)
                {
                    counter = 0;
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 8; l++)
                        {
                            if (_solutions[i][k].X == _solutions[j][l].X && _solutions[i][k].Y == _solutions[j][l].Y)
                            {
                                counter++;
                                break;
                            }
                        }
                    }
                    if (counter == 8)
                    {
                        break;
                    }
                }
                if (counter != 8)
                {
                    solutionSet.Add(_solutions[i]);
                }
            }

            _solutions = solutionSet;
        }
        private IEnumerable<int[]> GenerateQuarterArrays(int num)
        {
            Random rnd = new Random();
            int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3 };
            List<int[]> quarters = new List<int[]>();
            for (int i = 0; i < num; i++)
            {
                quarters.Add(arr.OrderBy(x => rnd.Next()).ToArray());
            }

            return quarters;
        }

        private IEnumerable<int[]> GenerateQuarterPermutations()
        {
            int[] xx = { 0, 1, 2, 3, 4, 5, 6, 7 };
            var arrays = GetPermutations(xx).ToList();

            foreach (var item in arrays)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    item[i] = item[i] / 2;
                }
            }
            return arrays;
        }

        private static IEnumerable<T[]> GetPermutations<T>(T[] values)
        {
            if (values.Length == 1)
                return new[] { values };

            return values.SelectMany(v => GetPermutations(values.Except(new[] { v }).ToArray()),
                (v, p) => new[] { v }.Concat(p).ToArray());
        }
        public List<List<Point>> Solve(bool reset = false)
        {
            if (reset)
            {
                _solutions = new List<List<Point>>();
            }
            _counter = 0;
            var arrays = GenerateQuarterArrays(1).ToList();
            //var arrays = GenerateQuarterPermutations();

            BoardManager board = new BoardManager();
            foreach (var array in arrays)
            {
                board = new BoardManager(boardSize);
                PlaceHetman(board, 0, 0, new Stack<Point>(), array);
                board = new BoardManager(boardSize);
                PlaceHetmanReverse(board, 0, 0, new Stack<Point>(), array);
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
            return _solutions;
        }

        private void PlaceHetman(BoardManager board, int quarter, int depth, Stack<Point> currentHetmans, int[] quarterArray)
        {
            if (depth == 8)
            {
                var solution = currentHetmans.ToList();
                _solutions.Add(solution);
                return;
            };

            Quarter currentQuarter = _quarters[quarterArray[depth]];
            var boardCopy = new bool[8, 8];

            Array.Copy(board.Board, 0, boardCopy, 0, board.Board.Length);
            BoardManager tempBoard = new BoardManager(boardCopy);
            for (var i = currentQuarter.StartX; i < currentQuarter.EndX; i++)
            {
                for (var j = currentQuarter.StartY; j < currentQuarter.EndY; j++)
                {
                    if (board.IsOccupied(j, i))
                        continue;

                    board.FillOccupiedPlaces(j, i);
                    _counter++;
                    currentHetmans.Push(new Point { X = j, Y = i });

                    PlaceHetman(board, (quarter + 1), depth + 1, currentHetmans, quarterArray);

                    currentHetmans.Pop();
                    board.Board = tempBoard.Board;
                }
            }

        }
        private void PlaceHetmanReverse(BoardManager board, int quarter, int depth, Stack<Point> currentHetmans, int[] quarterArray)
        {
            if (depth == 8)
            {
                var solution = currentHetmans.ToList();
                _solutions.Add(solution);
                return;
            };

            Quarter currentQuarter = _quarters[quarterArray[depth]];
            var boardCopy = new bool[8, 8];

            Array.Copy(board.Board, 0, boardCopy, 0, board.Board.Length);
            BoardManager tempBoard = new BoardManager(boardCopy);
            for (var i = currentQuarter.EndX - 1; i >= currentQuarter.StartX; i--)
            {
                for (var j = currentQuarter.EndY - 1; j >= currentQuarter.StartY; j--)
                {
                    if (board.IsOccupied(j, i))
                        continue;

                    board.FillOccupiedPlaces(j, i);
                    _counter++;
                    currentHetmans.Push(new Point { X = j, Y = i });

                    PlaceHetman(board, (quarter + 1), depth + 1, currentHetmans, quarterArray);

                    currentHetmans.Pop();
                    board.Board = tempBoard.Board;
                }
            }
        }
        private void RotateSolutions()
        {
            List<List<Point>> newSolutions = new List<List<Point>>();
            foreach (var item in _solutions)
            {
                var a = RotatePoints(item);
                var b = RotatePoints(a);
                var c = RotatePoints(b);
                newSolutions.Add(item);
                newSolutions.Add(a);
                newSolutions.Add(b);
                newSolutions.Add(c);
            }

            _solutions = newSolutions;
        }
        private List<Point> RotatePoints(List<Point> hetmanPositions)
        {
            List<Point> result = new List<Point>();
            foreach (var item in hetmanPositions)
            {
                result.Add(new Point { X = 7 - item.Y, Y = item.X });
            }

            return result;
        }
    }
}