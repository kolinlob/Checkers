using System;
using System.Runtime.InteropServices;

namespace Checkers
{
    public class Board
    {
        private char[,] board = new char[8, 8];

        public Board()
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                if (row % 2 == 0)
                {
                    for (int column = 0; column < board.GetLength(1); column++)
                    {
                        if (row % 2 == 0 && column % 2 == 0)
                        {
                            board[row, column] = '0';
                        }
                        else
                        {
                            board[row, column] = '1';
                        }
                    }
                }

                if (row % 2 != 0)
                {
                    for (int column = 0; column < board.GetLength(1); column++)
                    {
                        if (row % 2 != 0 && column % 2 == 0)
                        {
                            board[row, column] = '1';
                        }
                        else
                        {
                            board[row, column] = '0';
                        }
                    }
                }
            }
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