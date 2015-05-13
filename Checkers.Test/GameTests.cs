using System;
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

            int[] adressOld = { 1, 1 };
            int[] adressNew = { 2, 2 };
            var expected = game.CanMoveThere(adressOld, adressNew, false);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _005_CannotMoveToOccupiedBlackCell()
        {
            var game = new Game();
            game.Start();

            int[] adressOld = { 1, 1 };
            int[] adressNew = { 2, 1 };
            var expected = game.CanMoveThere(adressOld, adressNew, false);

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

        [TestMethod]
        public void _008_Ordinary_Checker_Moves_At_One_Cell_Only()
        {
            Game game = new Game();
            game.Start();

            
            game.CurrentPlayer = new FakePlayer(true);

            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var adressNew = game.ConvertIntoCoordinates("D4");

            var expected = game.OneCellMove(adressOld, adressNew);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _009_Quuine_Moves_At_More_Cell()
        {
            Game game = new Game();
            game.Start();

          
            game.CurrentPlayer = new FakePlayer(true);


            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var adressNew = game.ConvertIntoCoordinates("D4");

            
            var checkerId = game.GetCheckerId(adressOld);
            game.CheckersSet[checkerId].IsQueen = true;

            var expected = game.OneCellMove(adressOld, adressNew);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _010_Ordinary_Checkers_Cannot_Move_Backwards()
        {
            Game game = new Game();
            game.Start();

            
            game.CurrentPlayer = new FakePlayer(true);


            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var adressNew = game.ConvertIntoCoordinates("A7");


            var expected = game.MoveForward(adressOld, adressNew);
            Assert.IsFalse(expected);
        }


    }

    public class FakePlayer : IUserInput
    {

        public FakePlayer(bool playsWhites)
        {
            PlaysWhites = playsWhites;
        }

        public bool PlaysWhites { get; set; }

        public string InputCoordinates()
        {
            return "B6";
        }
    }
}