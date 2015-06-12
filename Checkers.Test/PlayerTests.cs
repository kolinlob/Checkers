using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void _001_Player_Can_Select_Checker()
        {
            var game = new Game();
            var validInput = new FakePlayer(true).InputCoordinates();

            const int expectedCheckerId = 8;

            game.CreateCheckers(false);

            var address = game.ConvertIntoCoordinates(validInput);
            var actualCheckerId = game.GetCheckerId(new Coordinate(address));

            Assert.AreEqual(actualCheckerId, expectedCheckerId);
        }

        [TestMethod]
        public void _002_First_Player_Can_Select_Only_White_Checkers()
        {
            var game = new Game();
            game.Start();

            const int id = 5;
            var blackChecker = game.CheckersSet[id];
            var canSelectBlackChecker = game.CanSelectChecker(blackChecker);

            Assert.IsFalse(canSelectBlackChecker);
        }

        [TestMethod]
        public void _003_Second_Player_Can_Select_Only_Black_Checkers()
        {
            var game = new Game();
            game.Start();
            
            game.CurrentPlayer.PlaysWhites = false;

            const int id = 15;
            var whiteChecker = game.CheckersSet[id];
            var canSelectWhiteChecker = game.CanSelectChecker(whiteChecker);

            Assert.IsFalse(canSelectWhiteChecker);
        }
    }
}