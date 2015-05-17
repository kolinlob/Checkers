using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Checkers.Test
{
    [TestClass]
    public class MoveTests
    {
        [TestMethod]
        public void _001_CanCreateMove()
        {
            var move = new Move();

            Assert.IsNotNull(move);
        }


        [TestMethod]
        public void _002_CanMakeMove()
        {
            var game = new Game();
            game.Start();
            game.CurrentPlayer = new FakePlayer(true);

            game.Move = new Move();

            var adressOld = game.ConvertIntoCoordinates(game.CurrentPlayer.InputCoordinates());
            game.Move.MoveCoordinates.Add(adressOld);

            var adressNew = game.ConvertIntoCoordinates("c5");
            game.Move.MoveCoordinates.Add(adressNew);

            game.MoveChecker();

            var id = game.GetCheckerId(adressNew);
            var actual = game.CheckersSet[id];
            var expected = new Checker(false, false, 3, 2);

            Assert.AreEqual(expected, actual);
        }
    }
}