namespace Checkers
{
    public interface IUserInput
    {
        bool PlaysWhites { get; set; }
        string EnterCoordinates();
    }
}
