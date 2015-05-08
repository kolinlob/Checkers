using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class CheckerTests
    {
        [TestMethod]
        public void _001_CanCreateChecker()
        {
            var checker = new Checker(true, false, 0, 1);

            Assert.IsNotNull(checker);
        }

        [TestMethod]
        public void _002_CheckerCanBeMovedToSpecifiedCoordinates()
        {
            var checker = new Checker(true, false, 0, 1);
            
            checker.HorizontalCoord = 3;
            checker.VerticalCoord = 0;

            var actual = checker;
            var expected = new Checker(true, false, 3, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void _003_CheckerProperties()
        {
            var game = new Game();
            game.CreateCheckers(true);

            var actualChecker = game.CheckersSet[0];
            var expectedChecker = new Checker(true, false, 0, 1);

            Assert.AreEqual(expectedChecker, actualChecker);
        }

        [TestMethod]
        public void _004_WhiteCheckerCanBecomeQueen()
        {
            var game = new Game();
            game.CreateCheckers(true);

            game.CheckersSet[5].HorizontalCoord = 7;
            game.CheckersSet[5].VerticalCoord = 0;
            game.CheckerBecomesQueen(game.CheckersSet[5]);

            var expected = game.CheckersSet[5].IsQueen;

            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _005_BlackCheckerCanBecomeQueen()
        {
            var game = new Game();
            game.CreateCheckers(false);

            game.CheckersSet[7].HorizontalCoord = 0;
            game.CheckersSet[7].VerticalCoord = 1;

            game.CheckerBecomesQueen(game.CheckersSet[7]);

            var expected = game.CheckersSet[7].IsQueen;

            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _006_WhiteCheckerChangesItsSymbolUponBecomingQueen()
        {
            var game = new Game();
            game.CreateCheckers(true);

            game.CheckersSet[5].HorizontalCoord = 7;
            game.CheckersSet[5].VerticalCoord = 0;

            game.CheckerBecomesQueen(game.CheckersSet[5]);

            Assert.IsTrue(game.CheckersSet[5].DrawChecker() == 'K');
        }

        [TestMethod]
        public void _007_BlackCheckerChangesItsSymbolUponBecomingQueen()
        {
            var game = new Game();
            game.CreateCheckers(false);

            game.CheckersSet[7].HorizontalCoord = 0;
            game.CheckersSet[7].VerticalCoord = 1;

            game.CheckerBecomesQueen(game.CheckersSet[7]);

            Assert.IsTrue(game.CheckersSet[7].DrawChecker() == 'K');
        }
    }
}
