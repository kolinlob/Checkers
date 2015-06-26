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

            var expectedChecker = new Checker(false, false, 2, 1);

            game.CreateCheckers(false);

            var moveStartCoordinate = game.ConvertIntoCoordinates(validInput);
            var actualChecker = game.GetChecker(moveStartCoordinate);

            Assert.AreEqual(actualChecker, expectedChecker);
        }

        [TestMethod]
        public void _002_First_Player_Can_Select_Only_White_Checkers()
        {
            var game = new Game();
            game.Start();

            var blackChecker = new Checker(false, false, 1, 0);
            var canSelectBlackChecker = game.CanSelectChecker(blackChecker);

            Assert.IsFalse(canSelectBlackChecker);
        }

        [TestMethod]
        public void _003_Second_Player_Can_Select_Only_Black_Checkers()
        {
            var game = new Game();
            game.Start();
            
            game.CurrentPlayer.PlaysWhites = false;

            var whiteChecker = new Checker(true, false, 5, 6);
            var canSelectWhiteChecker = game.CanSelectChecker(whiteChecker);

            Assert.IsFalse(canSelectWhiteChecker);
        }
    }
}