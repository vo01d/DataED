namespace DataED.Utils {
    public static class ToInt32Parser {
        public static int Parse(string input) {
            return int.Parse(input);
        }

        public static int ParseInRange(string input, int rangeBegin, int rangeEnd) {
            if (rangeBegin > rangeEnd) {
                throw new ArgumentException("Range begin greater than range end.");
            }

            int result = Parse(input);
            if (result < rangeBegin || result > rangeEnd) {
                throw new ArgumentOutOfRangeException($"Input string is not in the range [{rangeBegin}, {rangeEnd}].");
            }

            return result;
        }
    }
}
