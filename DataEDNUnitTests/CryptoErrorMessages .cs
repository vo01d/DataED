namespace DataEDNUnitTests {
    public static class CryptoErrorMessages {
        public static string InvalidFilePathError => "File with an invalid path exists.";
        public static string DecryptionMismatchError => "Decrypted text should match the original plaintext";

        public static string MethodShouldThrowArgumentException(string methodName, string reason) {
            return $"{methodName} should throw ArgumentException when {reason}.";
        }

        public static string ExpectedExceptionParameter(string parameterName) {
            return $"Exception parameter name should be '{parameterName}'";
        }
    }
}
