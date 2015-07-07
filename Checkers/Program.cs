﻿using System;

namespace Checkers
{
    class Program
    {
        static void Main()
        {
            Console.Title = "ITLabs - AndriiCheckers v1.0";
            Console.WindowWidth = 90;
            Console.WindowHeight = 30;

            var game = new Game();
            game.Start();
            
            while (!game.IsGameOver())
            {
                game.FindCheckersWithTakes();
                game.SetMove();
                game.SwitchPlayer();
            }
            game.ClearMessageBar();
            Console.SetCursorPosition(50, 10);
            Console.Write("Game Over");
            Console.ReadLine();
        }
    }
}