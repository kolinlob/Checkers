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
            var game = new Game();
            game.Start();
            game.CurrentPlayer = new FakePlayer(true);

            game.Move = new Move();

            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            game.Move.Coordinates.Add(new Coordinate(adressOld));

            var adressNew = game.ConvertIntoCoordinates("c5");
            game.Move.Coordinates.Add(new Coordinate(adressNew));

            game.MoveChecker();

            var id = game.GetCheckerId(new Coordinate(adressNew));
            var actual = game.CheckersSet[id];
            var expected = new Checker(false, false, 3, 2);

            Assert.AreEqual(expected, actual);
        }
    }
}