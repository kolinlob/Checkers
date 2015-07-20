using System.Collections.Generic;

namespace Checkers
{
    public class Move
    {
        public List<Coordinate> Coordinates = new List<Coordinate>();

        public void AddCoordinate(Coordinate moveStartCoordinate)
        {
            Coordinates.Add(moveStartCoordinate);
        }

        public void RemoveFirstCoordinate()
        {
            Coordinates.RemoveAt(0);
        }

        public Coordinate GetStartCoordinate()
        {
            return Coordinates[0];
        }

        public Coordinate GetEndCoordinate()
        {
            return Coordinates[1];
        }
    }
}