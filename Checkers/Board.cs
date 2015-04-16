using System;
using System.Runtime.InteropServices;

namespace Checkers
{
    public class Board
    {
        private Cell[,] board = new Cell[8, 8];

        //public Board()
        //{
        //    int counter = 0;
        //
        //    for (int row = 0; row < board.GetLength(0); row++)
        //    {
        //        for (int column = 0; column < board.GetLength(1); column++)
        //        {
        //            if (counter % (board.GetLength(0) + 1) == 0)
        //                counter++;
        //
        //            if (counter % 2 == 0)
        //                board[row, column] = ' ';
        //            else
        //                board[row, column] = 'X';
        //
        //            counter++;
        //        }
        //    }
        //}



        public void DrawBoard()
        {
            int counter = 0;

            Console.Write("\r\n      ");

            for (int column = 0; column < board.GetLength(1); column++)
                Console.Write(" " + Convert.ToChar(column + 65) + "  ");

            Console.Write("\r\n     |");
            for (int column = 0; column < board.GetLength(1); column++)
                Console.Write("---|");

            Console.WriteLine();

            for (int row = 0; row < board.GetLength(0); row++)
            {
                Console.Write("   " + (board.GetLength(0) - row) + " |");

                for (int column = 0; column < board.GetLength(1); column++)
                {
                    if (counter % (board.GetLength(0) + 1) == 0)
                        counter++;

                    if (counter % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.DarkBlue;

                    else
                        Console.BackgroundColor = ConsoleColor.White;
                    
                    counter++;

                    board[row, column] = new Cell();


                    Console.Write(" " + board[row, column].GetValue().GetValue() + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("|");
                }
                Console.BackgroundColor = ConsoleColor.Black;

                Console.Write(" " + (board.GetLength(0) - row) + "\r\n     |");

                for (int column = 0; column < board.GetLength(1); column++)
                    Console.Write("---|");

                Console.WriteLine();
            }

            Console.Write("      ");
            for (int column = 0; column < board.GetLength(1); column++)
                Console.Write(" " + Convert.ToChar(column + 65) + "  ");

            Console.WriteLine();
        }







        public override string ToString()
        {
            var result = string.Empty;

            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int column = 0; column < board.GetLength(1); column++)
                {
                    result += board[row, column] + " ";
                }
                result += Environment.NewLine;
            }

            return result;
        }
    }
}