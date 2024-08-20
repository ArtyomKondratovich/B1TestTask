namespace B1TestTask.Main.Utilities.Extensions
{
    internal static class HelpExtensions
    {
        public static Func<string, bool> GetPredicate(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return (x) => true;
            }

            return (x) => !str.Contains(x);
        }
    }
}
