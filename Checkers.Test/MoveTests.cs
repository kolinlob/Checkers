using System.Runtime.InteropServices.ComTypes;
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
            //var game = new Game();
            //game.CreateCheckers(true);
            //game.CreateCheckers(false);
            //
            //string PlayerChooseChecker = "B6";
            //string Destination = "D4";
            //
            //var adress = game.ConvertInputToCoordinates(PlayerChooseChecker);
            //
            //var SelectedCheckerID = game.GetCheckerId(adress);
            //
            //var AdressNew = game.ConvertInputToCoordinates(Destination);
            //
            // game.UpdateCoordinates(SelectedCheckerID, AdressNew);
            //
            //
            //var actual = game.CheckersSet[SelectedCheckerID];
            //var expected = new Checker(true, false, 4, 3);
            //Assert.AreEqual(expected, actual);
        }
    }
}