using System;
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
            var expected = game.CanMoveThere(adressOld, adressNew); //not a queen

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _005_Cannot_Move_To_Occupied_Black_Cell()
        {
            var game = new Game();
            game.Start();

            int[] adressOld = { 0, 1 };
            int[] adressNew = { 2, 1 };
            var expected = game.CanMoveThere(adressOld, adressNew);

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

            var expected = game.IsDiagonalMove(adressOld, adressNew);
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

            var actual = game.visibleEnemies[0].Coordinates;

            CollectionAssert.AreEqual(expected, actual);
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

            //var enemyList = game.GetEnemyCoordinates(new Coordinate(game.CheckersSet[0].CoordHorizontal, game.CheckersSet[0].CoordVertical));

            //Assert.IsTrue(game.CanTake());
        }

        [TestMethod]
        public void _015_Can_Get_Checker_Id()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CurrentPlayer = new FakePlayer(true)
            };

            game.CreateCheckers(false);
            game.CreateCheckers(true);

            var validAdress = game.CurrentPlayer.InputCoordinates();
            var adress = game.ConvertIntoCoordinates(validAdress);
            var coordinate = new Coordinate(adress[0], adress[1]);
            
            var actual = game.GetCheckerId(coordinate);
            
            Assert.AreEqual(8, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void _016_Cant_Get_Checker_Id_When_Empty_Cell_Selected()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CurrentPlayer = new FakePlayer(true)
            };

            game.CreateCheckers(false);
            game.CreateCheckers(true);

            const string validAdress = "A8";
            var adress = game.ConvertIntoCoordinates(validAdress);
            var coordinate = new Coordinate(adress[0], adress[1]);

            game.GetCheckerId(coordinate);
        }

        [TestMethod]
        public void _017_Cant_Move_to_Unusable_Cell()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CurrentPlayer = new FakePlayer(true)
            };

            game.CreateCheckers(false);
            game.CreateCheckers(true);

            game.CurrentPlayer = new FakePlayer(true);

            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var checkerId = game.GetCheckerId(new Coordinate(adressOld));
            game.CheckersSet[checkerId].ChangeSymbol();                 // make it queen

            var adressNew = game.ConvertIntoCoordinates("C4");

            var canMoveThere = game.CanMoveThere(adressOld, adressNew);
            Assert.IsFalse(canMoveThere);
        }

        [TestMethod]
        public void _018_Can_Remove_Enemies_Without_any_Empty_Cell_Behind()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, false, 3, 4), // CHECKER WE TEST
                    //new Checker(true, false, 1, 6),
                    new Checker(false, false, 2, 3),
                    new Checker(false, false, 6, 7),
                    new Checker(false, false, 2, 5),
                    new Checker(true, false, 1, 2)
                },
                CurrentPlayer = new FakePlayer(true)
            };

            game.Board.Draw(game.CheckersSet);

            var newMove = new Move();
            newMove.Coordinates.Add(new Coordinate(2, 5));

            var expected = new Dictionary<int, Move> {{0, newMove}};

            game.GetEnemyCoordinates(game.CheckersSet[0]);
            game.ExcludeNontakableEnemies(game.CheckersSet[0]);

            var actual = game.takableEnemies;

            CollectionAssert.AreEqual((System.Collections.ICollection)expected, (System.Collections.ICollection)actual); //references are not equal, hence test fail. However, dictionary is being filled properly, based on debug results
        }
    }
}