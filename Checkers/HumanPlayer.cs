using System;

namespace Checkers
{
    public class HumanPlayer : IUserInput
    {
        public HumanPlayer(bool playsWhites)
        {
            PlaysWhites = playsWhites;
        }

        public bool PlaysWhites { get; set; }

        public string InputCoordinates()
        {
            return Console.ReadLine();
        }
    }
}