namespace Checkers
{
    public class Checker
    {
        private char symbol = 'x';

        public void Set(char symbol)
        {
            this.symbol = symbol;
        }

        public char GetValue()
        {
            return symbol;
        }

    }
}