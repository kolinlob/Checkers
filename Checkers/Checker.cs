<<<<<<< HEAD
namespace Checkers
{
    public class Checker
    {
        private char symbol = 'x';

        public void Set(char symbol)
        {
            this.symbol = symbol;
        }

        public char GetValue()
        {
            return symbol;
        }

    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Checker
    {
        private char checkerSymbol;
        private bool isQueen;
        private bool isWhite;
        private int horizontalCoordinate;
        private int verticalCoordinate;

        //public char CheckerSymbol
        //{
        //    get { return CheckerSymbol = this.checkerSymbol; }
        //    set { checkerSymbol = value; }
        //}
    }
}
>>>>>>> b9fe8532dc5a1c2c377319563375eb3dc27b817c
