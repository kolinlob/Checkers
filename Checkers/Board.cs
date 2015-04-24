using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Checkers
{
    public class Board
    {
        private Cell[,] board = new Cell[8, 8];
        //List<Checker> checkersSet = new List<Checker>();

        public void Draw(List<Checker> checkersSet)
        {
            int counter = 0;

            Console.Write("     ");
            for (int column = 0; column < board.GetLength(1); column++)
                Console.Write(" " + Convert.ToChar(column + 65) + " ");

            Console.WriteLine();

            for (int row = 0; row < board.GetLength(0); row++)
            {
                Console.Write("   " + (board.GetLength(0) - row) + " ");
                
                for (int column = 0; column < board.GetLength(1); column++)
                {
                    if (counter % (board.GetLength(0) + 1) == 0)
                        counter++;

                    if (counter % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    else
                        Console.BackgroundColor = ConsoleColor.Gray;
                    
                    counter++;

                    board[row, column] = new Cell();
                    foreach (var checker in checkersSet)
                    {
                        if (row == checker.horizontalCoord && column == checker.verticalCoord)
                        {
                            board[row, column].Draw(checker);
                        }
                        else
                        {
                            board[row, column].Draw();
                        }
                    }
                    

                    
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.BackgroundColor = ConsoleColor.Black;

                Console.Write(" " + (board.GetLength(0) - row));

                Console.WriteLine();
            }

            Console.Write("     ");
            for (int column = 0; column < board.GetLength(1); column++)
                Console.Write(" " + Convert.ToChar(column + 65) + " ");

            Console.WriteLine();
        }







        public override string ToString()
        {
            var result = string.Empty;

            int counter = 0;

           result += "     ";
            for (int column = 0; column < board.GetLength(1); column++)
                result += " " + Convert.ToChar(column + 65) + " ";
           
            result += Environment.NewLine;

            for (int row = 0; row < board.GetLength(0); row++)
            {
                result += "   " + (board.GetLength(0) - row) + " ";

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


                    //result += board[row, column].Draw();
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.BackgroundColor = ConsoleColor.Black;

                result += " " + (board.GetLength(0) - row);

                result += Environment.NewLine;
            }

            result += "     ";
            for (int column = 0; column < board.GetLength(1); column++)
                result += " " + Convert.ToChar(column + 65) + " ";
            
            result += Environment.NewLine;

            return result;
        }
    }
}
