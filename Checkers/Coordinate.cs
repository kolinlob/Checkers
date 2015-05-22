using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Coordinate
    {
        public readonly int[] CellAddress = new int[2];

        public Coordinate(int coordHorizontal, int coordVertical)
        {
            CellAddress[0] = coordHorizontal;
            CellAddress[1] = coordVertical;
        }


        public Coordinate(int[] cellAddress)
        {
            CellAddress = cellAddress;
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
