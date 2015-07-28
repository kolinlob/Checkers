using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    public class Checkers
    {
        private List<Checker> checkers;

        public List<Checker> Set
        {
            get { return checkers; }
        }

        public Checkers()
        {
            checkers = new List<Checker>();
        }

        public void Create(bool isWhite)
        {
            var startRow = 0;
            var endRow = 3;

            if (isWhite)
            {
                startRow = 5;
                endRow = 8;
            }

            for (var i = startRow; i < endRow; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if ((i % 2 == 0 && j % 2 != 0) ||
                        (i % 2 != 0 && j % 2 == 0))
                    {
                        checkers.Add(new Checker(isWhite, false, new Coordinate(i, j)));
                    }
                }
            }
        }
        public List<Checker> GetOwnCheckers(IUserInput currentPlayer)
        {
            return new List<Checker>(checkers.Where(checker => currentPlayer.PlaysWhites == checker.IsWhite));
        }

        public Checker GetChecker(Coordinate coordinate)
        {
            return checkers.Find(checker => checker.Coordinate.X == coordinate.X && checker.Coordinate.Y == coordinate.Y);
        }
    }
}