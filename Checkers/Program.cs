using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Program
    {
        static void Main()
        {
            var board = new Board();

            Console.ForegroundColor = ConsoleColor.White;
            board.DrawBoard();
            Console.ReadLine();
        }
    }
}
