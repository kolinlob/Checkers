using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void _001_Can_Create_Board()
        {
            var board = new Board();

            Assert.IsNotNull(board);
        }

        [TestMethod]
        public void _002_Can_Display_Board()
        {
            var board = new Board();
            const string expected = "_Y_Y_Y_Y\r\n" +
                                    "Y_Y_Y_Y_\r\n" +
                                    "_Y_Y_Y_Y\r\n" +
                                    "Y_Y_Y_Y_\r\n" +
                                    "_Y_Y_Y_Y\r\n" +
                                    "Y_Y_Y_Y_\r\n" +
                                    "_Y_Y_Y_Y\r\n" +
                                    "Y_Y_Y_Y_\r\n";
            //const string expected =
            //"\r\n       A    B    C    D    E    F    G    H  \r\n" +
            //"\r\n"                                                   +
            //"                                             \r\n"      +
            //"   8                                          8 \r\n"   +
            //"                                             \r\n"      +
            //"                                             \r\n"      +
            //"   7                                          7 \r\n"   +
            //"                                             \r\n"      +
            //"                                             \r\n"      +
            //"   6                                          6 \r\n"   +
            //"                                             \r\n"      +
            //"                                             \r\n"      +
            //"   5                                          5 \r\n"   +
            //"                                             \r\n"      +
            //"                                             \r\n"      +
            //"   4                                          4 \r\n"   +
            //"                                             \r\n"      +
            //"                                             \r\n"      +
            //"   3                                          3 \r\n"   +
            //"                                             \r\n"      +
            //"                                             \r\n"      +
            //"   2                                          2 \r\n"   +
            //"                                             \r\n"      +
            //"                                             \r\n"      +
            //"   1                                          1 \r\n"   +
            //"                                             \r\n"      +
            //"\r\n"                                                   +
            //"       A    B    C    D    E    F    G    H  \r\n";

            Assert.AreEqual(expected, board.ToString());
        }

        [TestMethod]
        public void _003_Does_Particular_Cell_Exist()
        {
            var board = new Board();
            var expected = board.DoesCellExist(new Coordinate(3, 4));

            Assert.IsTrue(expected);
        }  
    }
}
