using System;

namespace Checkers
{
    public static class Screen
    {
        private const string selectCheckerToMoveMessage = "Choose a checker: ";
        private const string selectDestinationMessage = "Target cell: ";
        private const string cantMoveHereMessage = "Error! Wrong move!";
        private const string incorrectInputMessage = "Error! Incorrect input.";
        private const string cantSelectMessage = "Error! Cannot select!";


        public static void DisplaySelectCheckerMessage()
        {
            Console.WriteLine(selectCheckerToMoveMessage); 
        }

        public static void DisplaySelectDestinationMessage()
        {
            Console.WriteLine(selectDestinationMessage); 
        }
        public static void DisplayCantMoveHereMessage()
        {
            Console.WriteLine(cantMoveHereMessage); 
        }
        public static void DisplayIncorrectInputMessage()
        {
            Console.WriteLine(incorrectInputMessage); 
        }
        public static void DisplayCantSelectMessage()
        {
            Console.WriteLine(cantSelectMessage); 
        }

        public static string GetSelectCheckerMessage()
        {
            return selectCheckerToMoveMessage;
        }

        public static string GetSelectDestinationMessage()
        {
            return selectDestinationMessage;
        }


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