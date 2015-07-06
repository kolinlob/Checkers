using System;

namespace Checkers
{
    public class Coordinate
    {
        public int[] CellAddress = new int[2];

        public int X
        {
            get { return CellAddress[0]; }
            set { CellAddress[0] = value; }
        }

        public int Y
        {
            get { return CellAddress[1]; }
            set { CellAddress[1] = value; }
        }

        public Coordinate(int coordHorizontal, int coordVertical)
        {
            CellAddress[0] = coordHorizontal;
            CellAddress[1] = coordVertical;
        }

        public override int GetHashCode()
        {
            return String.Format("({0})", CellAddress).GetHashCode();
        }

        public override bool Equals(object other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
}
