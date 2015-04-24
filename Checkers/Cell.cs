using System;

namespace Checkers
{
    public class Cell
    {
        //private Checker checker = new Checker();



        //public void Set(Checker checker)
        //{
        //    this.checker = checker;
        //}

        public void Draw()
        {
            Console.Write(" " + " " + " ");
        }

        public void Draw(Checker checker)
        {
            Console.Write(" " + checker.Draw() + " ");
        }


        private char symbol;

        private ConsoleColor color;
        private bool IsKing;

        private string value;

        public Cell(ConsoleColor color)
        {
            this.color = color;
            IsKing = true;
            symbol = '☼';
            SetColor();
        }

        public Cell()
        {
            symbol = ' ';
        }

        protected void SetColor()
        {
            Console.ForegroundColor = color;
        }

        public void SetSymbol()
        {
            if (IsKing)
                symbol = 'K';
        }

        public char GetSymbol()
        {
            return symbol;
        }



        
    }
    }
