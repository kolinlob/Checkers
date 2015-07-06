using System;

namespace Checkers
{
    class Program
    {
        static void Main()
        {
            Console.WindowWidth = 90;
            Console.WindowHeight = 30;

            var game = new Game();
            game.Start();
            
            while (!game.IsGameOver())
            {
                game.FindCheckersWithTakes();
                game.SetMove();

                if (game.PossibleTakes.Count > 0)
                {
                    game.RemoveTakenChecker();
                }
                game.MoveChecker();

                Console.SetCursorPosition(0, 0);
                game.Board.Draw(game.CheckersSet);

                game.PossibleTakes.Clear();
                game.SwitchPlayer();
            }
            game.ClearMessageBar();
            Console.SetCursorPosition(50, 10);
            Console.Write("Game Over");
            Console.ReadLine();
        }
    }
}