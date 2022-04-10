namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string val) => string.IsNullOrEmpty(val);

        public static bool IsNullOrWhiteSpace(this string val) => string.IsNullOrWhiteSpace(val);

        public static bool Equals(this string val, string otherVal)
        {
            var retval = string.Equals(val, otherVal, System.StringComparison.OrdinalIgnoreCase);
            return retval;
        }
    }
}