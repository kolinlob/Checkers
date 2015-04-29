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

        [TestMethod] //доробити
        public void CheckerCanBeMoved()
        {
            var game = new Game();
            game.CreateCheckers();

            int expectedhorizontalCoord = game.checkersSet[8].horizontalCoord;
            //int expectedVerticalCoord = game.checkersSet[8].verticalCoord;


            game.checkersSet[8].horizontalCoord = 3;
            //game.checkersSet[8].verticalCoord = 0;


            int actualhorizontalCoord = game.checkersSet[8].horizontalCoord;
            //int actualVerticalCoord = game.checkersSet[8].verticalCoord;

            Assert.AreNotEqual(expectedhorizontalCoord, actualhorizontalCoord);
            //Assert.AreNotEqual(expectedVerticalCoord, actualVerticalCoord);
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
        public void WhiteCheckerIsQueen()
        {
            Game game = new Game();
            game.CreateCheckers();

            game.checkersSet[0].horizontalCoord = 7;
            game.checkersSet[0].verticalCoord = 0;

            game.SetQueen(game.checkersSet[0]);

            bool expected = game.checkersSet[0].isQueen;

            Assert.IsTrue(expected);
        }


        [TestMethod]
        public void BlacksCheckerIsQueen()
        {
            Game game = new Game();
            game.CreateCheckers("blacks");

            game.checkersSet[11].horizontalCoord = 0;
            game.checkersSet[11].verticalCoord = 1;

            game.SetQueen(game.checkersSet[11]);

            bool expected = game.checkersSet[11].isQueen;

            Assert.IsTrue(expected);
        }
    }
}
