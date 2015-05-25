﻿using System;
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

        public void Draw(List<Checker> checkersSet)
        {
            DrawColumnHeader();

            var counter = 0;
            for (var row = 0; row < board.GetLength(0); row++)
            {
                DrawMargin(row); Console.Write("\r\n  ");
                DrawRowNum(row);

                for (var column = 0; column < board.GetLength(1); column++)
                {
                    if (counter % (board.GetLength(0) + 1) == 0)
                    {
                        counter++;
                    }

                    if (counter % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write("  ");

                        foreach (var checker in checkersSet)
                        {
                            if (row == checker.CoordHorizontal && column == checker.CoordVertical)
                            {
                                board[row, column].DrawCell(checker);
                                board[row, column].IsEmpty = false;
                            }
                        }
                        if (board[row, column].IsEmpty)
                        {
                            board[row, column].DrawCell();
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("     ");
                    }
                    counter++;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                DrawRowNum(row);
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
                Console.Write("  " + Convert.ToChar(column + 65) + "  ");
            Console.WriteLine();
        }

        private void DrawRowNum(int row)
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

        public bool IsCellEmpty(int row, int col)
        {
            return (board[row, col].IsEmpty);
        }

        public bool IsCellUsable(int row, int col)
        {
            return (board[row, col].IsUsable);
        }

        public Cell GetCell(int row, int col)
        {
            return board[row, col];
        }

        public Cell GetCell(Coordinate coordinate)
        {
            return board[coordinate.CellAddress[0], coordinate.CellAddress[1]];
        }

        public bool CellExists(int[] adress)
        {
            return adress[0] >= 0 && adress[0] < 8 && adress[1] >= 0 && adress[1] < 8;
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
