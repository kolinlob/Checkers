using System;

namespace Checkers
{
    public class Checker
    {
        char symbol = '☼';
        public bool IsWhite { get; set; }
        public bool IsQueen { get; set; }
        public int VerticalCoord { get; set; }
        public int HorizontalCoord { get; set; }

        public Checker(bool isWhite, bool isQueen, int horizontalCoord, int verticalCoord)
        {
            IsWhite = isWhite;
            IsQueen = isQueen;
            HorizontalCoord = horizontalCoord;
            VerticalCoord = verticalCoord;
        }

        public override int GetHashCode()
        {
            return String.Format("({0} {1} {2} {3})", IsWhite, IsQueen, HorizontalCoord, VerticalCoord).GetHashCode();
        }

        public override bool Equals(object other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public void ChangeSymbol()
        {
            symbol = 'K';          
        }

        public char Draw()
        {
            SetColor(IsWhite ? ConsoleColor.Yellow : ConsoleColor.Red);
            return symbol;
        }

        protected void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
