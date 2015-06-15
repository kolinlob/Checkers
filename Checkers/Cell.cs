using System;

namespace Checkers
{
    public class Cell
    {
        private char symbol;
        private ConsoleColor color;

        public bool IsUsable { get; set; }

        public Cell()
        {
            IsUsable = true;
            symbol = ' ';
        }

        public Cell(ConsoleColor color)
        {
            IsUsable = true;
            this.color = color;
            symbol = ' ';
            SetColor();
        }

        protected void SetColor()
        {
            Console.ForegroundColor = color;
        }

        public string GetSymbol()
        {
            return Convert.ToString(symbol);
        }

        public void DrawEmptyCell()
        {
            Console.Write(' ');
        }

        public void DrawCell(Checker checker)
        {
            Console.Write(checker.DrawChecker());
        }
    }
}
