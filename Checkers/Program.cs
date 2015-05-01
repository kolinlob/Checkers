using System;

namespace Checkers
{
    class Program
    {
        static void Main()
        {
            var game = new Game();

            game.Start();

            while (!game.GameIsOver())
                game.MakeMove();

            Console.WriteLine("Game Over");
            Console.ReadLine();
        }
    }
}
