using System;
using System.Collections.Generic;

namespace Checkers
{
    public class Board
    {
        private readonly Cell[,] board;

        public Board()
        {
            board = new Cell[8, 8];

            var counter = 0;

            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var column = 0; column < board.GetLength(1); column++)
                {
                    if (counter % (board.GetLength(0) + 1) == 0)
                    {
                        counter++;
                    }

                    board[row, column] = new Cell();

                    if (counter % 2 != 0)
                    {
                        board[row, column].IsUsable = false;
                    }
                    counter++;
                }
            }
        }

        public void Draw(Checkers checkers)
        {
            DrawColumnHeader();

            for (var row = 0; row < board.GetLength(0); row++)
            {
                DrawMargin(row); Console.Write("\r\n  ");
                DrawRowNumber(row);

                for (var column = 0; column < board.GetLength(1); column++)
                {
                    if (board[row, column].IsUsable)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write("  ");

                        var checkerExists =
                                    checkers.Set.Exists(checker => checker.Coordinate.X == row
                                                               && checker.Coordinate.Y == column);

                        if (checkerExists)
                        {
                            var checker = checkers.Set.Find(c => c.Coordinate.X == row && c.Coordinate.Y == column);
                            board[row, column].DrawCell(checker);
                        }
                        else
                        {
                            board[row, column].DrawEmptyCell();
                        }
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("     ");
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                DrawRowNumber(row);
                DrawMargin(row);
            }
            Console.WriteLine();
            DrawColumnHeader();
        }

        private void DrawColumnHeader()
        {
            Console.WriteLine();
            Console.Write("     ");

            for (var column = 0; column < board.GetLength(1); column++)
            {   
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  " + Convert.ToChar(column + 65) + "  ");
            }
            Console.WriteLine();
        }

        private void DrawRowNumber(int row)
        {
            Console.Write(" " + (board.GetLength(0) - row) + " ");
        }

        private void DrawMargin(int row)
        {
            Console.Write("\r\n     ");

            for (var column = 0; column < board.GetLength(1); column++)
            {
                var i = column;
                if (row % 2 == 0)
                    i = i + 1;

                Console.BackgroundColor = i % 2 == 0 ? ConsoleColor.DarkBlue : ConsoleColor.Gray;
                Console.Write("     ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public bool DoesCellExist(Coordinate coordinate)
        {
            return coordinate.X >= 0
                && coordinate.X < 8
                && coordinate.Y >= 0
                && coordinate.Y < 8;
        }

        public bool IsCellUsable(Coordinate coordinate)
        {
            return (board[coordinate.X, coordinate.Y].IsUsable);
        }
        
        public Cell GetCell(Coordinate coordinate)
        {
            return board[coordinate.X, coordinate.Y];
        }

        public override string ToString()
        {
            var result = string.Empty;
            var counter = 0;

            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var column = 0; column < board.GetLength(1); column++)
                {
                    if (counter % (board.GetLength(0) + 1) == 0)
                    {
                        counter++;
                    }
                    board[row, column] = new Cell();

                    if (counter % 2 != 0)
                    {
                        board[row, column].IsUsable = false;
                        result += "_";
                    }
                    else
                    {
                        result += "Y";
                    }
                    counter++;
                }
                result += Environment.NewLine;
            }
            return result;
        }
    }
}
