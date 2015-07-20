using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Checkers.Test
{
    [TestClass]
    public class MoveTests
    {
        [TestMethod]
        public void _001_Can_Create_Move()
        {
            var move = new Move();

            Assert.IsNotNull(move);
        }


        [TestMethod]
        public void _002_Can_Make_Move()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, false,  new Coordinate(2, 1)),
                    new Checker(true, false,  new Coordinate(3, 4)),                 
                    new Checker(false, false, new Coordinate(1, 2)),
                    new Checker(false, false, new Coordinate(2, 5)),
                    new Checker(false, false, new Coordinate(4, 5)),
                }
            };
            game.CurrentPlayer = game.Player1;

            game.Move = new Move();

            var moveStartCoordinate = game.ConvertIntoCoordinates(game.CurrentPlayer.EnterCoordinates());
            game.Move.Coordinates.Add(moveStartCoordinate);

            var moveEndCoordinate = game.ConvertIntoCoordinates("c5");
            game.Move.Coordinates.Add(moveEndCoordinate);

            game.MoveChecker();

            var actual = game.GetChecker(moveEndCoordinate);
            var expected = new Checker(true, false, new Coordinate(3, 2));

            Assert.AreEqual(expected, actual);
        }
    }
}