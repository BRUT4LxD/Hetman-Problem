using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HetmanProblem
{
    internal class Game
    {
        private const int boardSize = 8;
        private List<List<Point>> _solutions = new List<List<Point>>();
        private readonly List<Quarter> _quarters;
        private int _counter = 0;
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
            int counter = 0;
            List<List<Point>> solutionSet = new List<List<Point>>();
            for (int i = 0; i < _solutions.Count; i++)
            {
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
        public void Solve()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            BoardManager board = new BoardManager(boardSize);
            for (int i = 0; i < 8; i++)
            {
                board = new BoardManager(boardSize);
                PlaceHetman(board, i, 0, new Stack<Point>(), i > 7 ? i : 0);
            }

            stopwatch.Stop();

            Console.WriteLine("Time: " + stopwatch.ElapsedTicks + " ticks");
            Console.WriteLine("Single hetman placements: " + _counter);

            Console.WriteLine("Found " + _solutions.Count + " solutions before refinement");
            RefineSolutions();
            Console.WriteLine("Found " + _solutions.Count + " solutions after refinement");
            foreach (var item in _solutions)
            {
                board.PrintBoardWithHetmans(item.ToList());
            }
        }

        private void PlaceHetman(BoardManager board, int quarter, int depth, Stack<Point> currentHetmans, int addFactor = 0, bool depthFlag = false)
        {
            if (depth == 8)
            {
                var solution = currentHetmans.ToList();
                _solutions.Add(solution);
                return;
            };

            Quarter currentQuarter = _quarters[depthFlag ? depth % 4 : (quarter / 2) % 4];
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

                    PlaceHetman(board, (quarter + 1 + addFactor), depth + 1, currentHetmans);

                    currentHetmans.Pop();
                    board.Board = tempBoard.Board;
                }
            }

        }
        private class Quarter
        {
            public int StartX { get; }
            public int StartY { get; }
            public int EndX { get; }
            public int EndY { get; }

            public Quarter(int startX, int startY, int endX, int endY)
            {
                StartX = startX;
                StartY = startY;
                EndX = endX;
                EndY = endY;
            }
        }
    }
}