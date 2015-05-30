using System;

namespace Checkers
{
    public class Cell
    {
        private char symbol;
        private ConsoleColor color;
        public bool IsEmpty  { get; set; }
        public bool IsUsable { get; set; }

        public Cell()
        {
            IsEmpty = true;
            IsUsable = true;
            symbol = ' ';
        }

        public Cell(ConsoleColor color)
        {
            IsEmpty = true;
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
