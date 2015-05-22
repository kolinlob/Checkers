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

        public Coordinate(int horizontalCoord, int verticalCoord)
        {
            CellAddress[0] = horizontalCoord;
            CellAddress[1] = verticalCoord;
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
