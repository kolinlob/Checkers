namespace Checkers.Test
{
    public class FakePlayer : IUserInput
    {
        public FakePlayer(bool playsWhites)
        {
            PlaysWhites = playsWhites;
        }

        public bool PlaysWhites { get; set; }

        public string InputCoordinates()
        {
            return "B6";
        }
    }
}