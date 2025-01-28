namespace DataED.Utils {
    static class InputValidationHelper {
        public static int ValidateInt32(string input) {
            return int.Parse(input);
        }

        public static int ValidateInt32InRange(string input, int rangeBegin, int rangeEnd) {
            if (rangeBegin > rangeEnd) {
                throw new ArgumentException("Range begin greater than range end.");
            }

            int result = ValidateInt32(input);
            if (result < rangeBegin || result > rangeEnd) {
                throw new ArgumentOutOfRangeException($"Input string is not in the range [{rangeBegin}, {rangeEnd}].");
            }

            return result;
        }

        public static int ValidateInt32IsNegative(string input) {
            return ValidateInt32InRange(input, int.MinValue, -1);
        }

        public static int ValidateInt32IsPositive(string input) {
            return ValidateInt32InRange(input, 1, int.MaxValue);
        }
    }
}
