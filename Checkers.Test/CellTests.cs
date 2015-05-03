﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{

    [TestClass]
    public class CellTests
    {
        [TestMethod]
        public void _01_CanCreateCell()
        {
            var expected = new Cell();
            
            Assert.IsNotNull(expected);
        }

        [TestMethod]
        public void _02_IsCellEmpty()
        {
            var board = new Board();
            List<Checker> checkersSet = new List<Checker>();
            
            board.Draw(checkersSet);
            bool expected = board.IsEmpty(2, 1);

            Assert.IsTrue(expected);
        }

        [TestMethod]
        public void _03_IsCellUsable()
        {
            var board = new Board();
            List<Checker> checkersSet = new List<Checker>();
            
            board.Draw(checkersSet);
            bool expected = board.IsUsable(3, 0);

            Assert.IsTrue(expected);
        }  
    }
}