using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{

    [TestClass]
    public class CellTests
    {
        [TestMethod]
        public void _001_CanCreateCell()
        {
            var expected = new Cell();
            
            Assert.IsNotNull(expected);
        }

        [TestMethod]
        public void _002_IsCellEmpty()
        {
            var board = new Board();
            var checkersSet = new List<Checker>();
            
            board.Draw(checkersSet);
            var expected = board.IsCellEmpty(2, 1);

            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _003_IsCellUsable()
        {
            var board = new Board();
            var checkersSet = new List<Checker>();
            
            board.Draw(checkersSet);
            var expected = board.IsCellUsable(3, 0);

            Assert.IsTrue(expected);
        }  
    }
}