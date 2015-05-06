using System;
using System.Text;

namespace Checkers
{
    public class Player
    {
        public Player(bool playsWhites)
        {
            PlaysWhites = playsWhites;
        }

        public bool PlaysWhites { get; set; }

        
    }
}