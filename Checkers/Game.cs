using System;
using System.Collections.Generic;

namespace Checkers
{
    public class Game
    {
        public List<Checker> checkersSet = new List<Checker>();
        public void Start()
        {
            var board = new Board();

            checkersSet.Add(new Checker(true, false, 0, 1));
            checkersSet.Add(new Checker(true, false, 0, 3));
            checkersSet.Add(new Checker(true, false, 0, 5));
            checkersSet.Add(new Checker(true, false, 0, 7));

            checkersSet.Add(new Checker(true, false, 1, 0));
            checkersSet.Add(new Checker(true, false, 1, 2));
            checkersSet.Add(new Checker(true, false, 1, 4));
            checkersSet.Add(new Checker(true, false, 1, 6));

            checkersSet.Add(new Checker(true, false, 2, 1));
            checkersSet.Add(new Checker(true, false, 2, 3));
            checkersSet.Add(new Checker(true, false, 2, 5));
            checkersSet.Add(new Checker(true, false, 2, 7));




            checkersSet.Add(new Checker(false, false, 5, 0));
            checkersSet.Add(new Checker(false, false, 5, 2));
            checkersSet.Add(new Checker(false, false, 5, 4));
            checkersSet.Add(new Checker(false, false, 5, 6));

            checkersSet.Add(new Checker(false, false, 6, 1));
            checkersSet.Add(new Checker(false, false, 6, 3));
            checkersSet.Add(new Checker(false, false, 6, 5));
            checkersSet.Add(new Checker(false, false, 6, 7));

            checkersSet.Add(new Checker(false, false, 7, 0));
            checkersSet.Add(new Checker(false, false, 7, 2));
            checkersSet.Add(new Checker(false, false, 7, 4));
            checkersSet.Add(new Checker(false, false, 7, 6));

            Console.ForegroundColor = ConsoleColor.White;

            board.Draw(checkersSet);
        }
    }
}