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
        public void _002_WhiteCheckersAreInStartPosition()
        {
            Game game = new Game();
            game.CreateCheckers(true);

            List<Checker> expected = new List<Checker>();
            expected.Add(new Checker(true, false, 0, 1));
            expected.Add(new Checker(true, false, 0, 3));
            expected.Add(new Checker(true, false, 0, 5));
            expected.Add(new Checker(true, false, 0, 7));
            expected.Add(new Checker(true, false, 1, 0));
            expected.Add(new Checker(true, false, 1, 2));
            expected.Add(new Checker(true, false, 1, 4));
            expected.Add(new Checker(true, false, 1, 6));
            expected.Add(new Checker(true, false, 2, 1));
            expected.Add(new Checker(true, false, 2, 3));
            expected.Add(new Checker(true, false, 2, 5));
            expected.Add(new Checker(true, false, 2, 7));

            CollectionAssert.AreEqual(expected, game.CheckersSet);
        }

        [TestMethod]
        public void _002_BlackCheckersAreInStartPosition()
        {
            Game game = new Game();
            game.CreateCheckers(false);

            List<Checker> expected = new List<Checker>();
            expected.Add(new Checker(false, false, 5, 0));
            expected.Add(new Checker(false, false, 5, 2));
            expected.Add(new Checker(false, false, 5, 4));
            expected.Add(new Checker(false, false, 5, 6));
            expected.Add(new Checker(false, false, 6, 1));
            expected.Add(new Checker(false, false, 6, 3));
            expected.Add(new Checker(false, false, 6, 5));
            expected.Add(new Checker(false, false, 6, 7));
            expected.Add(new Checker(false, false, 7, 0));
            expected.Add(new Checker(false, false, 7, 2));
            expected.Add(new Checker(false, false, 7, 4));
            expected.Add(new Checker(false, false, 7, 6));

            CollectionAssert.AreEqual(expected, game.CheckersSet);
        }

        [TestMethod]
        public void _003_CannotMoveToWhiteCell()
        {
            var game = new Game();
            game.Start();

            int[] adress = { 2, 2 };
            bool expected = game.CanMoveThere(adress);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _004_CannotMoveToOccupiedBlackCell()
        {
            var game = new Game();
            game.Start();

            int[] adress = { 2, 1 };
            bool expected = game.CanMoveThere(adress);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _005_GameIsOver_When_No_Black_CheckersRemained()
        {
            var game = new Game();
            game.Start();
            game.CheckersSet.RemoveAll(checker => checker.IsWhite == false);

            Assert.IsTrue(game.GameIsOver());
        }

        [TestMethod]
        public void _006_GameIsOver_When_No_White_CheckersRemained()
        {
            var game = new Game();
            game.Start();
            game.CheckersSet.RemoveAll(checker => checker.IsWhite);

            Assert.IsTrue(game.GameIsOver());
        }
    }
}