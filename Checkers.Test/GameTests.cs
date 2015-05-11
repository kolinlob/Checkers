using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void _001_CanCreateGame()
        {
            var game = new Game();

            Assert.IsNotNull(game);
        }

        [TestMethod]
        public void _002_BlackCheckersAreInStartPosition()
        {
            var game = new Game();
            game.CreateCheckers(false);

            var expected = new List<Checker>
            {
                new Checker(false, false, 0, 1),
                new Checker(false, false, 0, 3),
                new Checker(false, false, 0, 5),
                new Checker(false, false, 0, 7),
                new Checker(false, false, 1, 0),
                new Checker(false, false, 1, 2),
                new Checker(false, false, 1, 4),
                new Checker(false, false, 1, 6),
                new Checker(false, false, 2, 1),
                new Checker(false, false, 2, 3),
                new Checker(false, false, 2, 5),
                new Checker(false, false, 2, 7)
                
            };

            CollectionAssert.AreEqual(expected, game.CheckersSet);
        }

        [TestMethod]
        public void _003_WhiteCheckersAreInStartPosition()
        {
            var game = new Game();
            game.CreateCheckers(true);

            var expected = new List<Checker>
            {
                new Checker(true, false, 5, 0),
                new Checker(true, false, 5, 2),
                new Checker(true, false, 5, 4),
                new Checker(true, false, 5, 6),
                new Checker(true, false, 6, 1),
                new Checker(true, false, 6, 3),
                new Checker(true, false, 6, 5),
                new Checker(true, false, 6, 7),
                new Checker(true, false, 7, 0),
                new Checker(true, false, 7, 2),
                new Checker(true, false, 7, 4),
                new Checker(true, false, 7, 6)
            };

            CollectionAssert.AreEqual(expected, game.CheckersSet);
        }

        [TestMethod]
        public void _004_CannotMoveToWhiteCell()
        {
            var game = new Game();
            game.Start();

            int[] adress = { 2, 2 };
            var expected = game.CanMoveThere(adress);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _005_CannotMoveToOccupiedBlackCell()
        {
            var game = new Game();
            game.Start();

            int[] adress = { 2, 1 };
            var expected = game.CanMoveThere(adress);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _006_GameIsOver_When_No_Black_CheckersRemained()
        {
            var game = new Game();
            game.Start();
            game.CheckersSet.RemoveAll(checker => checker.IsWhite == false);

            Assert.IsTrue(game.IsGameOver());
        }

        [TestMethod]
        public void _007_GameIsOver_When_No_White_CheckersRemained()
        {
            var game = new Game();
            game.Start();
            game.CheckersSet.RemoveAll(checker => checker.IsWhite);

            Assert.IsTrue(game.IsGameOver());
        }
    }
}