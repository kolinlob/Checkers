﻿using System;

namespace Checkers
{
    public class Checker
    {
        char symbol = '☼';
        public bool isWhite { get; set; }
        public bool isQueen { get; set; }
        public int verticalCoord { get; set; }
        public int horizontalCoord { get; set; }

        public Checker(bool isWhite, bool isQueen, int horizontalCoord, int verticalCoord)
        {
            this.isWhite = isWhite;
            this.isQueen = isQueen;
            this.horizontalCoord = horizontalCoord;
            this.verticalCoord = verticalCoord;
        }

        public void Set(char symbol)
        {
            this.symbol = symbol;          
        }

        public char Draw()
        {
            SetColor(isWhite ? ConsoleColor.Yellow : ConsoleColor.Red);
            return symbol;
        }

        protected void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }


        public override int GetHashCode()
        {

            return String.Format("({0} {1} {2} {3})", isWhite, isQueen, horizontalCoord, verticalCoord).GetHashCode();
        }

        public override bool Equals(object other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

    }
}
