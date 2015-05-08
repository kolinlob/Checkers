using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void _001_PlayerCanSelectChecker()
        {
            const string validInput = "B6";
            const int expectedCheckerId = 8;
            
            var game = new Game();
            var player = new HumanPlayer(true);

            game.CreateCheckers(true);

            var address = game.ConvertInputToCoordinates(validInput);
            var actualCheckerId = game.GetCheckerId(address);

            Assert.AreEqual(actualCheckerId, expectedCheckerId);
        }

        [TestMethod]
        public void _002_WhitePlayerCanSelectOnlyWhiteChecker()
        {
            var game = new Game();
            var playerWhite = new HumanPlayer(true); 
            
            game.CreateCheckers(true);
            game.CreateCheckers(false);

            const int blackCheckerId = 23;

            game.CurrentPlayer = playerWhite;
            
            var canSelectBlackChecker = game.CanSelectChecker(blackCheckerId);

            Assert.IsFalse(canSelectBlackChecker);
        }

        [TestMethod]
        public void _003_BlackPlayerCanSelectOnlyBlackChecker()
        {
            var game = new Game();
            var playerBlack = new HumanPlayer(false);

            game.CreateCheckers(true);
            game.CreateCheckers(false);

            const int whiteCheckerId = 8;
            
            game.CurrentPlayer = playerBlack;

            var canSelectWhiteChecker = game.CanSelectChecker(whiteCheckerId);

            Assert.IsFalse(canSelectWhiteChecker);
        }


    }
}