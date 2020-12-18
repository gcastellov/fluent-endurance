namespace FluentEndurance
{
    public class Times
    {
        public static Times Once { get; } = new Times(1);

        public static Times Twice { get; } = new Times(2);

        public static Times Max { get; } = new Times(int.MaxValue);

        public static Times As(int times) => new Times(times);

        private Times(int times)
        {
            Value = times;
        }

        internal int Value { get; }
    }
}