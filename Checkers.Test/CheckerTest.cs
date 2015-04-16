<<<<<<< HEAD
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
>>>>>>> b9fe8532dc5a1c2c377319563375eb3dc27b817c
