using System;

namespace Checkers
{
    public class Coordinate
    {
        private readonly int[] cellAddress = new int[2];

        public int X
        {
            get { return cellAddress[0]; }
            set { cellAddress[0] = value; }
        }

        public int Y
        {
            get { return cellAddress[1]; }
            set { cellAddress[1] = value; }
        }

        public Coordinate(int coordHorizontal, int coordVertical)
        {
            cellAddress[0] = coordHorizontal;
            cellAddress[1] = coordVertical;
        }

        public override int GetHashCode()
        {
            return String.Format("({0} {1})", X, Y).GetHashCode();
        }

        public override bool Equals(object other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
}