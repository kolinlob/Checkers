using System;

namespace Checkers
{
    public class Cell
    {
        private const char symbol = ' ';

        public bool IsUsable { get; set; }

        public Cell()
        {
            IsUsable = true;
        }

        public void DrawEmptyCell()
        {
            Console.Write(symbol);
        }

        public void DrawCell(Checker checker)
        {
            Console.Write(checker.DrawChecker());
        }
    }
}