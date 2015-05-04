using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void _001_PlayerCanSelectChecker()
        {
            //9th checker, number 8 in a collection
            string input = "B6"; 
            int expectedCheckerID = 8;
            
            Game game = new Game();
            Player player = new Player(true);

            game.CreateCheckers(true);

            int[] address = player.RawInput(input);
            int actualCheckerID = game.SelectedCheckerId(address);

            Assert.AreEqual(actualCheckerID, expectedCheckerID);
        }

        [TestMethod]
        public void _002_WhitePlayerCanSelectOnlyWhiteChecker()
        {
            Game game = new Game();
            Player player_white = new Player(true); 
            
            game.CreateCheckers(true);
            game.CreateCheckers(false);

            int blackCheckerID = 23;

            game.CurrentPlayer = player_white;
            
            bool CanSelectBlackChecker = game.CanSelectChecker(blackCheckerID);

            Assert.IsFalse(CanSelectBlackChecker);
        }

        [TestMethod]
        public void _003_BlackPlayerCanSelectOnlyBlackChecker()
        {
            Game game = new Game();
            Player player_black = new Player(false);

            game.CreateCheckers(true);
            game.CreateCheckers(false);

            int whiteCheckerID = 8;
            
            game.CurrentPlayer = player_black;

            bool CanSelectWhiteChecker = game.CanSelectChecker(whiteCheckerID);

            Assert.IsFalse(CanSelectWhiteChecker);
        }


    }
}