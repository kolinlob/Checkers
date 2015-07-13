using System;

namespace Checkers
{
    public class HumanPlayer : IUserInput
    {
        public bool PlaysWhites { get; set; }
        
        public HumanPlayer(bool playsWhites)
        {
            PlaysWhites = playsWhites;
        }
        
        public string InputCoordinates()
        {
            var input = string.Empty;
            const int limit = 2;

            while (true)
            {
                var keyChar = Console.ReadKey(true).KeyChar;
                if (keyChar == '\r')
                    break;
                if (keyChar == '\b')
                {
                    if (input == "") continue;
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
                else if (input.Length < limit)
                {
                    Console.Write(keyChar);
                    input += keyChar;
                }
            }
            return input;
        }
    }
}