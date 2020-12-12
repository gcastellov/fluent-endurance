namespace FluentEndurance
{
    public class Timeout
    {
        public static Timeout Being(int ms) => new Timeout(ms);

        private Timeout(int ms)
        {
            Value = ms;
        }

        internal int Value { get; }
    }
}