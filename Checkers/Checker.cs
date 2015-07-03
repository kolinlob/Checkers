using System;

namespace Checkers
{
    public class Checker
    {
        char symbol = '█';
        public bool IsWhite { get; set; }
        public bool IsQueen { get; set; }
        public int CoordVertical { get; set; }
        public int CoordHorizontal { get; set; }

        public Checker(bool isWhite, bool isQueen, int coordHorizontal, int coordVertical)
        {
            IsWhite = isWhite;
            IsQueen = isQueen;
            CoordHorizontal = coordHorizontal;
            CoordVertical = coordVertical;

            if (isQueen)
            {
                GetQueenSymbol();
            }
        }

        public void GetQueenSymbol()
        {
            symbol = '☼';          
        }

        public char DrawChecker()
        {
            SetColor(IsWhite ? ConsoleColor.White : ConsoleColor.Red);
            return symbol;
        }

        protected void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public override int GetHashCode()
        {
            return String.Format("({0} {1} {2} {3})", IsWhite, IsQueen, CoordHorizontal, CoordVertical).GetHashCode();
        }

        public override bool Equals(object other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
}
