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
        public void _002_IsCellUsable()
        {
            var board = new Board();
            var coordinate = new Coordinate(3, 0);
            var expected = board.IsCellUsable(coordinate);

            Assert.IsTrue(expected);
        }
    }
}