using System;
using System.Collections.Generic;

namespace Checkers
{
    public class Board
    {
        private readonly Cell[,] board = new Cell[8, 8];

        public void Draw(List<Checker> checkersSet)
        {
            DrawColumnHeader();

            int counter = 0;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                DrawMargin(row); Console.Write("\r\n  ");
                DrawRowNum(row);

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
                            if (row == checker.HorizontalCoord && column == checker.VerticalCoord)
                            {
                                board[row, column].DrawCell(checker);
                                board[row, column].IsEmpty = false;
                            }
                        }
                        if (board[row, column].IsEmpty)
                            board[row, column].DrawCell();

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("     ");
                        board[row, column].IsUsable = false;
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
            for (int column = 0; column < board.GetLength(1); column++)
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

        public bool IsCellEmpty(int row, int col)
        {
            return (board[row, col].IsEmpty);
        }

        public bool IsCellUsable(int row, int col)
        {
            return (board[row, col].IsUsable);
        }

        //public override string ToString()
        //{
        //    string result = string.Empty;
        //
        //    result += "\r\n     ";
        //    for (int column = 0; column < board.GetLength(1); column++)
        //        result += "  " + Convert.ToChar(column + 65) + "  ";
        //    result += Environment.NewLine;
        //
        //    for (int row = 0; row < board.GetLength(0); row++)
        //    {
        //        result += "\r\n     ";
        //        for (int column = 0; column < board.GetLength(1); column++)
        //            result += "     ";
        //
        //        result += "\r\n  ";
        //        result += " " + (board.GetLength(0) - row) + " ";
        //
        //        for (int column = 0; column < board.GetLength(1); column++)
        //        {
        //                    board[row, column] = new Cell();
        //                    result += "  " + Convert.ToString(board[row, column].GetSymbol()) + "  ";
        //        }
        //        result += " " + (board.GetLength(0) - row) + " ";
        //        result += "\r\n     ";
        //        for (int column = 0; column < board.GetLength(1); column++)
        //            result += "     ";
        //    }
        //    result += Environment.NewLine;
        //    result += Environment.NewLine;
        //    result += "     ";
        //    for (int column = 0; column < board.GetLength(1); column++)
        //        result += "  " + Convert.ToChar(column + 65) + "  ";
        //    result += Environment.NewLine;
        //
        //    return result;
        //}

        public override int GetHashCode()
        {
            return String.Format("({0})",board).GetHashCode();
        }

        public override bool Equals(object other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
}
