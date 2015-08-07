using System;

namespace Checkers
{
    public class Checker
    {
        char symbol = '█';
        public bool IsWhite { get; private set; }
        public bool IsQueen { get; private set; }
        public Coordinate Coordinate { get; private set; }

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

        protected bool Equals(Checker other)
        {
            return IsWhite.Equals(other.IsWhite) && IsQueen.Equals(other.IsQueen) && Coordinate.Equals(other.Coordinate) && symbol == other.symbol;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Checker) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsWhite.GetHashCode();
                hashCode = (hashCode*397) ^ IsQueen.GetHashCode();
                hashCode = (hashCode*397) ^ Coordinate.GetHashCode();
                hashCode = (hashCode*397) ^ symbol.GetHashCode();
                return hashCode;
            }
        }
    }
}