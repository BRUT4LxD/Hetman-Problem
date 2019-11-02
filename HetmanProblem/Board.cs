using System;
using System.Collections.Generic;

namespace HetmanProblem
{
    internal struct BoardManager
    {
        public bool[,] Board { get; set; }

        public BoardManager(int boardSize)
        {
            Board = new bool[boardSize, boardSize];
            InitBoard();
        }

        public BoardManager(bool[,] board)
        {
            Board = board;
        }
        private void InitBoard()
        {

            for (var i = 0; i < Board.GetLength(0); i++)
            {
                for (var j = 0; j < Board.GetLength(1); j++)
                {
                    Board[i, j] = false;
                }
            }
        }


        public void FillOccupiedPlaces(int x, int y)
        {
            Board[y, x] = true;
            FillHorizontal(x, y);
            FillVertical(x, y);
            FillDiagonalLeft(x, y);
            FillDiagonalRight(x, y);
        }

        public bool IsOccupied(int x, int y) => Board[y, x];
        private void FillVertical(int x, int y)
        {
            var currentX = x;
            while (++currentX < 8)
            {
                Board[y, currentX] = true;
            }

            currentX = x;

            while (--currentX >= 0)
            {
                Board[y, currentX] = true;
            }

        }

        private void FillHorizontal(int x, int y)
        {
            var currentY = y;
            while (++currentY < 8)
            {
                Board[currentY, x] = true;
            }

            currentY = y;

            while (--currentY >= 0)
            {
                Board[currentY, x] = true;
            }

        }

        private void FillDiagonalRight(int x, int y)
        {
            var currentY = y;
            var currentX = x;
            while (++currentY < 8 && ++currentX < 8)
            {
                Board[currentY, currentX] = true;
            }

            currentY = y;
            currentX = x;

            while (--currentY >= 0 && --currentX >= 0)
            {
                Board[currentY, currentX] = true;
            }
        }

        private void FillDiagonalLeft(int x, int y)
        {
            var currentY = y;
            var currentX = x;
            while (--currentY >= 0 && ++currentX < 8)
            {
                Board[currentY, currentX] = true;
            }

            currentY = y;
            currentX = x;
            while (++currentY < 8 && --currentX >= 0)
            {
                Board[currentY, currentX] = true;
            }
        }

        public void PrintBoard()
        {
            for (var i = 0; i < Board.GetLength(0); i++)
            {
                for (var j = 0; j < Board.GetLength(1); j++)
                {
                    Console.Write(Board[i, j] ? "X " : "O ");
                }

                Console.WriteLine();
            }
        }


        public void PrintBoardWithHetmans(List<Point> hetmans)
        {
            for (var i = 0; i < Board.GetLength(0); i++)
            {
                for (var j = 0; j < Board.GetLength(1); j++)
                {
                    bool isHetman = false;
                    foreach (Point hetman in hetmans)
                    {
                        if (j == hetman.X && i == hetman.Y)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X ");
                            isHetman = true;
                            break;
                        }
                    }

                    if (!isHetman)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("O ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}