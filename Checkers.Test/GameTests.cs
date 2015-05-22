using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void _001_Can_Create_Game()
        {
            var game = new Game();

            Assert.IsNotNull(game);
        }

        [TestMethod]
        public void _002_Black_Checkers_Are_In_Start_Position()
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
        public void _003_White_Checkers_Are_In_Start_Position()
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
        public void _004_Cannot_Move_To_White_Cell()
        {
            var game = new Game();
            game.Start();

            int[] adressOld = { 0, 1 };
            int[] adressNew = { 2, 2 };
            var expected = game.CanMoveThere(adressOld, adressNew, false);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _005_Cannot_Move_To_Occupied_Black_Cell()
        {
            var game = new Game();
            game.Start();

            int[] adressOld = { 0, 1 };
            int[] adressNew = { 2, 1 };
            var expected = game.CanMoveThere(adressOld, adressNew, false);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _006_Game_Is_Over_When_No_Black_Checkers_Remained()
        {
            var game = new Game();
            game.Start();
            game.CheckersSet.RemoveAll(checker => checker.IsWhite == false);

            Assert.IsTrue(game.IsGameOver());
        }

        [TestMethod]
        public void _007_Game_Is_Over_When_No_White_Checkers_Remained()
        {
            var game = new Game();
            game.Start();
            game.CheckersSet.RemoveAll(checker => checker.IsWhite);

            Assert.IsTrue(game.IsGameOver());
        }

        [TestMethod]
        public void _008_Ordinary_Checker_Moves_At_One_Cell_Only()
        {
            var game = new Game();
            game.Start();

            game.CurrentPlayer = new FakePlayer(true);

            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var adressNew = game.ConvertIntoCoordinates("D4");

            var expected = game.OneCellMove(adressOld, adressNew);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _009_Queen_Moves_At_More_than_1_Cell()
        {
            var game = new Game();
            game.Start();

            game.CurrentPlayer = new FakePlayer(true);

            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var adressNew = game.ConvertIntoCoordinates("D4");

            var checkerId = game.GetCheckerId(new Coordinate(adressOld));
            game.CheckersSet[checkerId].IsQueen = true;

            var expected = game.OneCellMove(adressOld, adressNew);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _010_Ordinary_Checkers_Cannot_Move_Backwards()
        {
            var game = new Game();
            game.Start();

            game.CurrentPlayer = new FakePlayer(true);

            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var adressNew = game.ConvertIntoCoordinates("A7");

            var expected = game.MoveForward(adressOld, adressNew);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _011_Queen_Move_Only_on_Diagonals()
        {
            var game = new Game();
            game.Start();

            game.CurrentPlayer = new FakePlayer(true);


            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var adressNew = game.ConvertIntoCoordinates("B4");

            var expected = game.QueenMove(adressOld, adressNew);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _012_Checker_Can_Take_Opponents_Checkers()
        {
            var game = new Game();
            game.Start();

            var expected = false;
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _013_Can_Define_Enemy_Coordinates()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 3, 4),
                    new Checker(true, false, 4, 5),

                    new Checker(false, false, 2, 3),
                    new Checker(false, false, 6, 7),
                    new Checker(false, false, 2, 5)
                }
            };

            game.CurrentPlayer = game.Player1;

            var expected = new List<Coordinate>()
            {
                new Coordinate(2, 3),
                new Coordinate(2, 5),
            };

            var actual = game.GetEnemyCoordinates(new Coordinate(3, 4));

            CollectionAssert.AreEqual(expected, actual.Coordinates);
        }

        [TestMethod]
        public void _014_Is_There_A_Free_Cell_Behind_The_Enemy_For_A_Take()
        {

            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, false, 3, 4), // CHECKER WE TEST
                    new Checker(true, false, 4, 5),

                    new Checker(false, false, 2, 3),
                    new Checker(false, false, 6, 7),
                    new Checker(false, false, 2, 5)
                }
            };

            var enemyList = game.GetEnemyCoordinates(new Coordinate(game.CheckersSet[0].CoordHorizontal, game.CheckersSet[0].CoordVertical));

            Assert.IsTrue(game.CanTake());
        }
    }
}