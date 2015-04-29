using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void CanCreateGame()
        {
            var game = new Game();

            Assert.IsNotNull(game);
        }

        [TestMethod]
        public void WhitesCheckersAreInStartPosition()
        {
            Game game = new Game();
            game.CreateCheckers();


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

           

            CollectionAssert.AreEqual(expected, game.checkersSet);
        }

        [TestMethod]
        public void BlacksCheckersAreInStartPosition()
        {
            Game game = new Game();
            game.CreateCheckers("blacks");


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

            
            CollectionAssert.AreEqual(expected, game.checkersSet);
        }
        
    }
}