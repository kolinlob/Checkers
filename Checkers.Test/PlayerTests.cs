using System.Collections.Generic;
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
            var validInput = new FakePlayer(true).EnterCoordinates();

            var expectedChecker = new Checker(false, false, new Coordinate(2, 1));

            game.CreateCheckers(false);

            var moveStartCoordinate = game.ConvertIntoCoordinates(validInput);
            var actualChecker = game.GetChecker(moveStartCoordinate);

            Assert.AreEqual(actualChecker, expectedChecker);
        }

        [TestMethod]
        public void _002_First_Player_Cannot_Select_Black_Checkers()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true,   new Coordinate(2, 1)),
                    new Checker(true, false,  new Coordinate(3, 4)),                 
                    new Checker(false, false, new Coordinate(1, 2)),
                    new Checker(false, false, new Coordinate(2, 5)),
                    new Checker(false, false, new Coordinate(4, 5)),
                }
            };
            game.CurrentPlayer = game.Player1;

            var blackChecker = game.CheckersSet[2];
            var canSelectBlackChecker = game.CanSelectChecker(blackChecker);

            Assert.IsFalse(canSelectBlackChecker);
        }

        [TestMethod]
        public void _003_Second_Player_Cannot_Select_White_Checkers()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true,   new Coordinate(2, 1)),
                    new Checker(true, false,  new Coordinate(3, 4)),                 
                    new Checker(false, false, new Coordinate(1, 2)),
                    new Checker(false, false, new Coordinate(2, 5)),
                    new Checker(false, false, new Coordinate(4, 5)),
                }
            };
            game.CurrentPlayer = game.Player2;

            var whiteChecker = game.CheckersSet[0];
            var canSelectWhiteChecker = game.CanSelectChecker(whiteChecker);

            Assert.IsFalse(canSelectWhiteChecker);
        }
    }
}