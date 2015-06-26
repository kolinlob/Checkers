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
            game.Move.Coordinates.Add(adressOld);

            var adressNew = game.ConvertIntoCoordinates("c5");
            game.Move.Coordinates.Add(adressNew);

            game.MoveChecker();

            var actual = game.GetChecker(adressNew);
            var expected = new Checker(false, false, 3, 2);

            Assert.AreEqual(expected, actual);
        }
    }
}