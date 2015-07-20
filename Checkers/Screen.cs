using System;

namespace Checkers
{
    public static class Screen
    {
        public static void SetGraphicParameters()
        {
            Console.Title = "Console Checkers v1.0";
            Console.WindowWidth = 90;
            Console.WindowHeight = 30;
        }

        public static void ClearMessageBar()
        {
            Console.SetCursorPosition(50, 3); Console.Write("                              ");
            Console.SetCursorPosition(50, 3);
        }

        public static void DisplayCurrentPlayerMessage(IUserInput currentPlayer)
        {
            Console.SetCursorPosition(50, 1); Console.Write("                     ");
            Console.SetCursorPosition(50, 1); Console.Write("{0} move!", currentPlayer.PlaysWhites ? "White" : "Black");
            Console.SetCursorPosition(50, 3);
        }

        public static void GameOverMessage()
        {
            ClearMessageBar();
            Console.SetCursorPosition(50, 10);
            Console.Write("Game Over");
            Console.ReadLine();
        }
    }
}