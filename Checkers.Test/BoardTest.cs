using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void CanCreateBoard()
        {
            var board = new Board();

            Assert.IsNotNull(board);
        }


        [TestMethod]
        public void CanDisplayBoard()
        {
            var board = new Board();
            var expected = "0 1 0 1 0 1 0 1 \r\n" +
                           "1 0 1 0 1 0 1 0 \r\n" +
                           "0 1 0 1 0 1 0 1 \r\n" +
                           "1 0 1 0 1 0 1 0 \r\n" +
                           "0 1 0 1 0 1 0 1 \r\n" +
                           "1 0 1 0 1 0 1 0 \r\n" +
                           "0 1 0 1 0 1 0 1 \r\n" +
                           "1 0 1 0 1 0 1 0 \r\n";

            Assert.AreEqual(expected, board.ToString());
        }
    }



}
