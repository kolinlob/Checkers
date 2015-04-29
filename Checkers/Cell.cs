using System;

namespace Checkers
{
    public class Cell
    {
        private char symbol;
        private ConsoleColor color;
        public bool IsEmpty { get; set; }
        public bool IsUsable { get; set; }



        public void Draw()
        {
            Console.Write(' ');
        }

        public void Draw(Checker checker)
        {
            Console.Write(checker.Draw());
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
            IsUsable = false;
        }

        protected void SetColor()
        {
            Console.ForegroundColor = color;
        }

        public char GetSymbol()
        {
            return symbol;
        }
    }
}
