using System;
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
    }
}