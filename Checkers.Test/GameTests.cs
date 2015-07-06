using System;
using System.Collections.Generic;
using System.Linq;
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
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 0, 1),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5)
                },
            };
            game.CurrentPlayer = game.Player1;

            var moveStartCoordinate = new Coordinate(0, 1);
            var moveEndCoordinate = new Coordinate(2, 2);
            var expected = game.CanMoveThere(moveStartCoordinate, moveEndCoordinate);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _005_Cannot_Move_To_Occupied_Black_Cell()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 0, 1),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 2, 1),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;

            var moveStartCoordinate = new Coordinate(0, 1);
            var moveEndCoordinate = new Coordinate(2, 1);
            var expected = game.CanMoveThere(moveStartCoordinate, moveEndCoordinate);

            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _006_Game_Is_Over_When_No_Black_Checkers_Remained()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 0, 7),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;

            game.CheckersSet.RemoveAll(checker => !checker.IsWhite);

            Assert.IsTrue(game.IsGameOver());
        }

        [TestMethod]
        public void _007_Game_Is_Over_When_No_White_Checkers_Remained()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 0, 7),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;

            game.CheckersSet.RemoveAll(checker => checker.IsWhite);

            Assert.IsTrue(game.IsGameOver());
        }

        [TestMethod]
        public void _008_Ordinary_Checker_Moves_At_One_Cell_Only()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 0, 7),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;

            var moveStartCoordinate = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var moveEndCoordinate = game.ConvertIntoCoordinates("D4");

            var expected = game.IsOneCellMove(moveStartCoordinate, moveEndCoordinate);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _009_Queen_Moves_At_More_than_1_Cell()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 2, 1),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;
            
            var input = game.CurrentPlayer.InputCoordinates();
            var moveStartCoordinate = game.ConvertIntoCoordinates(input);
            var moveEndCoordinate = game.ConvertIntoCoordinates("D4");

            game.GetChecker(moveStartCoordinate).IsQueen = true;
            
            var expected = game.IsOneCellMove(moveStartCoordinate, moveEndCoordinate);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _010_Ordinary_Checkers_Cannot_Move_Backwards()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 2, 1),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;

            var moveStartCoordinate = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var moveEndCoordinate = game.ConvertIntoCoordinates("A5");

            var expected = game.IsMoveForward(moveStartCoordinate, moveEndCoordinate);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _011_Common_Checker_Can_Move_Only_on_Diagonals()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 2, 1),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;

            var moveStartCoordinate = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var moveEndCoordinate = game.ConvertIntoCoordinates("B4");
            
            var currentChecker = game.GetChecker(moveStartCoordinate);
            currentChecker.IsQueen = false;

            var expected = game.IsDiagonalMove(moveStartCoordinate, moveEndCoordinate);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void _012_Queen_Can_Move_Only_on_Diagonals()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, true, 2, 1),
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),
                }
            };
            game.CurrentPlayer = game.Player1;

            var moveStartCoordinate = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var moveEndCoordinate = game.ConvertIntoCoordinates("B4");
            
            var currentChecker = game.GetChecker(moveStartCoordinate);
            currentChecker.IsQueen = true;

            var expected = game.IsDiagonalMove(moveStartCoordinate, moveEndCoordinate);
            Assert.IsFalse(expected);
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
                    new Checker(true, false, 3, 4),
                    new Checker(true, false, 4, 5),

                    new Checker(false, false, 2, 3),
                    new Checker(false, false, 6, 7),
                    new Checker(false, false, 2, 5)
                }
            };
            game.CurrentPlayer = game.Player1;
            var checker = game.CheckersSet[0];
            game.FindPossibleTakes(checker);

            var actual_1X = game.EnemiesCoordinates[0].X;
            var actual_1Y = game.EnemiesCoordinates[0].Y;
            var actual_2X = game.EnemiesCoordinates[1].X;
            var actual_2Y = game.EnemiesCoordinates[1].Y;

            Assert.AreEqual(actual_1X, 2);
            Assert.AreEqual(actual_1Y, 3);
            Assert.AreEqual(actual_2X, 2);
            Assert.AreEqual(actual_2Y, 5);
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
            game.CurrentPlayer = game.Player1;

            var checker = game.CheckersSet[0];
            game.FindPossibleTakes(checker);
            var emptyCellsAreAvailableBehindEnemy = game.PossibleTakes.Any();

            Assert.IsTrue(emptyCellsAreAvailableBehindEnemy);
        }

        [TestMethod]
        public void _015_Can_Get_Checker()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false)
            };
            game.CurrentPlayer = game.Player2;

            game.CreateCheckers(false);
            game.CreateCheckers(true);

            game.FindCheckersWithTakes();

            var validAdress = game.CurrentPlayer.InputCoordinates();
            var moveStartCoordinate = game.ConvertIntoCoordinates(validAdress);

            var checker = game.GetChecker(moveStartCoordinate);
            var canSelectChecker = game.CanSelectChecker(checker);

            Assert.IsTrue(canSelectChecker);
        }

        [TestMethod]
        public void _016_Cannot_Get_Any_Checker_If_Empty_Cell_Selected()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false)
            };
            game.CurrentPlayer = game.Player1;

            game.CreateCheckers(false);
            game.CreateCheckers(true);

            const string validAdress = "A8";
            var adress = game.ConvertIntoCoordinates(validAdress);
            var coordinate = new Coordinate(adress.X, adress.Y);

            var checker = game.GetChecker(coordinate);
            var canSelect = game.CanSelectChecker(checker);

            Assert.IsFalse(canSelect);
        }

        [TestMethod]
        public void _017_Cannot_Move_to_Unusable_Cell()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false)
            };
            game.CurrentPlayer = game.Player1;

            game.CreateCheckers(false);
            game.CreateCheckers(true);

            game.CurrentPlayer = new FakePlayer(true);

            var moveStartCoordinate = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            var checker = game.GetChecker(moveStartCoordinate);
            checker.IsQueen = true;

            var moveEndCoordinate = game.ConvertIntoCoordinates("C4");

            var canMoveThere = game.CanMoveThere(moveStartCoordinate, moveEndCoordinate);
            Assert.IsFalse(canMoveThere);
        }

        [TestMethod]
        public void _018_Find_Possible_Takes()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, false, 3, 4),
                    new Checker(false, false, 4, 5)
                }
            };
            game.CurrentPlayer = game.Player1;

            var takingMove = new List<Coordinate> {new Coordinate(3, 4), new Coordinate(5, 6)};

            var expectedChecker = game.GetChecker(takingMove[0]);
            var currentChecker = game.CheckersSet[0];

            var expectedPossibleTakes = new Dictionary<Checker, List<Coordinate>>
            {
                {
                    expectedChecker, takingMove
                }
            };

            game.FindPossibleTakes(currentChecker);

            var expectedX = expectedPossibleTakes[expectedChecker][1].X; //5
            var expectedY = expectedPossibleTakes[expectedChecker][1].Y; //6
            var actualX = game.PossibleTakes[currentChecker][0].X;       //5
            var actualY = game.PossibleTakes[currentChecker][0].Y;       //6

            Assert.AreEqual(actualX, expectedX);
            Assert.AreEqual(actualY, expectedY);
        }

        [TestMethod]
        public void _019_Find_All_Checkers_With_Takes()
        {
            var game = new Game
            {
                Board = new Board(),
                Player1 = new FakePlayer(true),
                Player2 = new FakePlayer(false),
                CheckersSet = new List<Checker>
                {
                    new Checker(true, false, 3, 4),                 
                    new Checker(false, false, 1, 2),
                    new Checker(false, false, 2, 5),
                    new Checker(false, false, 4, 5),

                    new Checker(true, true, 7, 0),
                    new Checker(false, false, 5, 2),
                    new Checker(false, false, 5, 0),
                }
            };
            game.CurrentPlayer = game.Player1;

            game.FindCheckersWithTakes();
            
            var actual_1X = game.CheckersWithTakes[0].Coordinate.X;
            var actual_1Y = game.CheckersWithTakes[0].Coordinate.Y;

            var actual_2X = game.CheckersWithTakes[1].Coordinate.X;
            var actual_2Y = game.CheckersWithTakes[1].Coordinate.Y;

            Assert.AreEqual(actual_1X, 3);
            Assert.AreEqual(actual_1Y, 4);
            Assert.AreEqual(actual_2X, 7);
            Assert.AreEqual(actual_2Y, 0);
        }

        [TestMethod]
        public void _020_Checker_Can_Jump_Over_FoeChecker()
        {
            var expected = false;
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _021_Cannot_Select_Blocked_Checker()
        {
            var expected = false;
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _022_Can_Remove_Taken_Checker()
        {
            var expected = false;
            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _023_Can_Make_Compound_Move()
        {
            var expected = false;
            Assert.IsTrue(expected);
        }
    }
}