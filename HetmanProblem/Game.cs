using System;
using System.Collections.Generic;
using System.Linq;

namespace HetmanProblem
{
    internal partial class Game
    {
        private int _boardSize;

        internal List<List<Point>> Solutions { get; set; } = new List<List<Point>>();

        public Game(int boardSize)
        {
            _boardSize = boardSize;
        }

        public List<List<Point>> Solve()
        {

            var board = new BoardManager(_boardSize);
            PlaceHetman(board, 0, new Stack<Point>());

            return Solutions;
        }

        private bool PlaceHetman(BoardManager board, int depth, Stack<Point> currentHetmans)
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
            for (var i = 0; i < _boardSize; i++)
            {
                if (board.IsOccupied(i, depth))
                    continue;

                board.FillOccupiedPlaces(i, depth);

                currentHetmans.Push(new Point { X = i, Y = depth });

                if (PlaceHetman(board, CostFunction(depth), currentHetmans)) return true;

                currentHetmans.Pop();

                Array.Copy(tempBoard.Board, 0, board.Board, 0, tempBoard.Board.Length);
            }

            return false;
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
        private int CostFunction(int depth) => depth + 1;
    }
}