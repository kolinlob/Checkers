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
        public void CheckerCanMove()
        {
            var game = new Game();
            game.Start();

            int expectedhorizontalCoord = game.checkersSet[8].horizontalCoord;
            //int expectedVerticalCoord = game.checkersSet[8].verticalCoord;


            game.checkersSet[8].horizontalCoord = 3;
            //game.checkersSet[8].verticalCoord = 0;


            int actualhorizontalCoord = game.checkersSet[8].horizontalCoord;
            //int actualVerticalCoord = game.checkersSet[8].verticalCoord;

            Assert.AreNotEqual(expectedhorizontalCoord, actualhorizontalCoord);
            //Assert.AreNotEqual(expectedVerticalCoord, actualVerticalCoord);
        }
    }
}
