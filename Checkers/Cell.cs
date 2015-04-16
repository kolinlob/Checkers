namespace Checkers
{
    public class Cell
    {
        private Checker checker = new Checker();



        public void Set(Checker checker)
        {
            this.checker = checker;
        }

        public Checker GetValue()
        {
            return checker;
        }
    }
}