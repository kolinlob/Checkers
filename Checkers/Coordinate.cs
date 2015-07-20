using System;

namespace Checkers
{
    public class Coordinate : IEquatable<Coordinate>
    {
        private readonly int[] cellAddress = new int[2];

        public int X
        {
            get { return cellAddress[0]; }
        }

        public int Y
        {
            get { return cellAddress[1]; }
        }

        public Coordinate(int coordHorizontal, int coordVertical)
        {
            cellAddress[0] = coordHorizontal;
            cellAddress[1] = coordVertical;
        }

        public bool Equals(Coordinate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return cellAddress.Equals(other.cellAddress);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Coordinate) obj);
        }

        public override int GetHashCode()
        {
            return cellAddress.GetHashCode();
        }

        public void Change(Coordinate newCoordinate)
        {
            cellAddress[0] = newCoordinate.X;
            cellAddress[1] = newCoordinate.Y;
        }

        //public override int GetHashCode()
        //{
        //    return String.Format("({0} {1})", X, Y).GetHashCode();
        //}
        //
        //public override bool Equals(object other)
        //{
        //    return GetHashCode() == other.GetHashCode();
        //}
    }
}