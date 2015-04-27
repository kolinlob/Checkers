using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Checkers
{
    public class Board
    {
        private Cell[,] board = new Cell[8, 8];

        public void Draw(List<Checker> checkersSet)
        {
            ColumnHeader();

            int counter = 0;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                Margin(row); Console.Write("\r\n  ");
                RowNum(row);

                for (int column = 0; column < board.GetLength(1); column++)
                {
                    if (counter % (board.GetLength(0) + 1) == 0)
                        counter++;
                    board[row, column] = new Cell();

                    if (counter % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write("  ");

                        foreach (var checker in checkersSet)
                        {
                            if (row == checker.horizontalCoord && column == checker.verticalCoord)
                            {
                                Console.Write(board[row, column].Draw(checker));
                                board[row, column].isEmpty = false;
                            }
                        }
                        if (board[row, column].isEmpty)
                            Console.Write(board[row, column].Draw());

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

                RowNum(row);
                Margin(row);
            }
            Console.WriteLine();
            ColumnHeader();
        }

        private void ColumnHeader()
        {
            Console.WriteLine();
            Console.Write("     ");
            for (int column = 0; column < board.GetLength(1); column++)
                Console.Write("  " + Convert.ToChar(column + 65) + "  ");
            Console.WriteLine();
        }

        private void RowNum(int row)
        {
            Console.Write(" " + (board.GetLength(0) - row) + " ");
        }

        private void Margin(int row)
        {
            Console.Write("\r\n     ");

            for (int column = 0; column < board.GetLength(1); column++)
            {
                int i = column;
                if (row % 2 == 0)
                    i = i + 1;

                if (i % 2 == 0)
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                else
                    Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("     ");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public override string ToString()
        {
            string result = string.Empty;

            result += Environment.NewLine;
            result += "     ";
            for (int column = 0; column < board.GetLength(1); column++)
                result += "  " + Convert.ToChar(column + 65) + "  ";
            result += Environment.NewLine;

            int counter = 0;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                result += "\r\n     ";
                for (int column = 0; column < board.GetLength(1); column++)
                    result += "     ";

                result += "\r\n  ";
                result += " " + (board.GetLength(0) - row) + " ";

                for (int column = 0; column < board.GetLength(1); column++)
                {
                    if (counter % (board.GetLength(0) + 1) == 0)
                        counter++;
                    if (counter % 2 == 0)
                    {
                        if (row >= 0 && row < 3)
                        {
                            board[row, column] = new Cell(ConsoleColor.Red);
                            result += "  " + board[row, column].GetSymbol() + "  ";
                        }
                        if (row >= 3 && row < 5)
                        {
                            board[row, column] = new Cell();
                            result += "  " + board[row, column].Draw() + "  ";
                        }
                        if (row >= 5 && row < 8)
                        {
                            board[row, column] = new Cell(ConsoleColor.Yellow);
                            result += "  " + board[row, column].GetSymbol() + "  ";
                        }
                    }
                    else
                    {
                        board[row, column] = new Cell();
                        result += "  " + board[row, column].Draw() + "  ";
                    }
                    counter++;
                }
                result += " " + (board.GetLength(0) - row) + " ";
                result += "\r\n     ";
                for (int column = 0; column < board.GetLength(1); column++)
                    result += "     ";
            }
            result += Environment.NewLine;
            result += Environment.NewLine;
            result += "     ";
            for (int column = 0; column < board.GetLength(1); column++)
                result += "  " + Convert.ToChar(column + 65) + "  ";
            result += Environment.NewLine;

            return result;
        }
    }
}
