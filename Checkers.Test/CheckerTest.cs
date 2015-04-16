using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Checkers.Test
{
    [TestClass]
    public class CheckerTest
    {
        [TestMethod]
        public void CanCreateChecker()
        {
            var checker = new Checker();

            Assert.IsNotNull(checker);
        }
    }
}