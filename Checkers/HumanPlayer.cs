using System;
using System.Text;

namespace Checkers
{
    public class HumanPlayer : IPlayer
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