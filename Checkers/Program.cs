using System;

namespace Checkers
{
    class Program
    {
        static void Main()
        {
            var game = new Game();
            game.Start();

            while (!game.IsGameOver())
            {
                game.FindCheckersWithTakes();
                game.SetCoordinatesForMove();

                if (game.CheckersWithTakes.Count > 0)
                {
                    game.RemoveTakenChecker();
                }
                game.MoveChecker();
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