using System;

namespace Checkers
{
    public class Cell
    {
        private char symbol;
        private ConsoleColor color;
        public bool IsEmpty { get; set; }
        public bool IsUsable { get; set; }

        public void DrawCell()
        {
            Console.Write(' ');
        }

        public void DrawCell(Checker checker)
        {
            Console.Write(checker.DrawChecker());
        }

        public Cell(ConsoleColor color)
        {
            this.color = color;
            IsEmpty = true;
            IsUsable = true;
            symbol = ' ';
            SetColor();
        }

        public Cell()
        {
            IsEmpty = true;
            IsUsable = true;
        }

        protected void SetColor()
        {
            Console.ForegroundColor = color;
        }

        public string GetSymbol()
        {
            return Convert.ToString(symbol);
        }
    }
}
