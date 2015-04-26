using System;

namespace Checkers
{
    public class Cell
    {
        //private Checker checker = new Checker();

        private char symbol;
        private ConsoleColor color;
        public bool isEmpty { get; set; }


        //public void Set(Checker checker)
        //{
        //    this.checker = checker;
        //}

        public char Draw()
        {
            return ' ';
        }

        public char Draw(Checker checker)
        {
            return checker.Draw();
        }

        public Cell(ConsoleColor color)
        {
            this.color = color;
            this.isEmpty = isEmpty;
            symbol = '☼';
            SetColor();
        }

        public Cell()
        {
            isEmpty = true;
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
