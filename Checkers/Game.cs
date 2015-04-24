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
            //checkersSet.Add(new Checker(true, false, 1, 2));
            //checkersSet.Add(new Checker(true, false, 2, 3));

            Console.ForegroundColor = ConsoleColor.White;
            board.Draw(checkersSet);
        }
    }
}