using System.Collections.Generic;

namespace Checkers
{
    public class Move
    {
        private readonly List<Coordinate> coordinates = new List<Coordinate>();

        public void AddCoordinate(Coordinate moveStartCoordinate)
        {
            coordinates.Add(moveStartCoordinate);
        }

        public void RemoveFirstCoordinate()
        {
            coordinates.RemoveAt(0);
        }

        public Coordinate GetStartCoordinate()
        {
            return coordinates[0];
        }

        public Coordinate GetEndCoordinate()
        {
            return coordinates[1];
        }
    }
}