using System;

namespace Checkers
{
    public class Checker
    {
        char symbol = '█';
        public bool IsWhite { get; private set; }
        public bool IsQueen { get; set; } // private set
        public Coordinate Coordinate { get; set; }

        public Checker(bool isWhite, bool isQueen, Coordinate coordinate) // delete isQueen
        {
            IsWhite = isWhite;
            IsQueen = isQueen; // false
            Coordinate = coordinate;
        }

        public void ChangeToQueen()
        {
            IsQueen = true;
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
            return String.Format("({0} {1} {2} {3})", IsWhite, IsQueen, Coordinate.X, Coordinate.Y).GetHashCode();
        }
        
        public override bool Equals(object other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
}