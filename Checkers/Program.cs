using System;

namespace Checkers
{
    class Program
    {
        static void Main()
        {
            var game = new Game();
            game.Start();
            
            while (true)//!game.IsGameOver())
            {
                game.FindCheckersWithTakes();
                game.SetCoordinatesForMove();
                if (game.CheckersWithTakes.Count>0)
                {
                    game.RemoveBeatenChecker();    
                }
                game.MoveChecker();
                game.PossibleTakes.Clear();
                game.SwitchPlayer();
            }
            Console.SetCursorPosition(50, 10);
            Console.Write("Game Over");
            Console.ReadLine();
        }
    }
}