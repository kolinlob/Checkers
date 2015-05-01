using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class CheckerTest
    {
        [TestMethod]
        public void CanCreateChecker()
        {
            var checker = new Checker(true, false, 0, 1);

            Assert.IsNotNull(checker);
        }

        [TestMethod]
        public void CheckerCanBeMovedToSpecifiedCoordinates()
        {
            var checker = new Checker(true, false, 0,1);
            
            checker.HorizontalCoord = 3;
            checker.VerticalCoord = 0;

            var actual = checker;
            var expected = new Checker(true, false, 3, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckerProperties()
        {
            var game = new Game();
            game.CreateCheckers();

            Checker actualChecker = game.checkersSet[0];

            Checker expectedChecker = new Checker(true, false, 0, 1);

            Assert.AreEqual(expectedChecker, actualChecker);
        }

        [TestMethod]
        public void WhiteCheckerCanBecomeQueen()
        {
            Game game = new Game();
            game.CreateCheckers();

            game.checkersSet[5].HorizontalCoord = 7;
            game.checkersSet[5].VerticalCoord = 0;

            game.CheckerBecomesQueen(game.checkersSet[5]);

            bool expected = game.checkersSet[5].IsQueen;

            Assert.IsTrue(expected);
        }


        [TestMethod]
        public void BlackCheckerCanBecomeQueen()
        {
            Game game = new Game();
            game.CreateCheckers("blacks");

            game.checkersSet[7].HorizontalCoord = 0;
            game.checkersSet[7].VerticalCoord = 1;

            game.CheckerBecomesQueen(game.checkersSet[7]);

            bool expected = game.checkersSet[7].IsQueen;

            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void WhiteCheckerChangesItsSymbolUponBecomingQueen()
        {
            Game game = new Game();
            game.CreateCheckers();

            game.checkersSet[5].HorizontalCoord = 7;
            game.checkersSet[5].VerticalCoord = 0;

            game.CheckerBecomesQueen(game.checkersSet[5]);
            Assert.IsTrue(game.checkersSet[5].Draw() == 'K');
        }

        [TestMethod]
        public void BlackCheckerChangesItsSymbolUponBecomingQueen()
        {
            Game game = new Game();
            game.CreateCheckers("blacks");

            game.checkersSet[7].HorizontalCoord = 0;
            game.checkersSet[7].VerticalCoord = 1;

            game.CheckerBecomesQueen(game.checkersSet[7]);
            Assert.IsTrue(game.checkersSet[7].Draw() == 'K');
        }
    }
}
