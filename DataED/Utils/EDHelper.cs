using System.Security.Cryptography;

namespace DataED.Utils {
    static class EDHelper {
        public static void EncryptionToFile_AES(string plaintext, string keyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("The plaintext to encrypt cannot be null or empty.", nameof(plaintext));
            }

            if (!File.Exists(keyFilePath)) {
                throw new ArgumentException("The key file path must be a valid, non-empty string.", nameof(keyFilePath));
            }

            if (!File.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file path must be a valid, non-empty string.", nameof(encryptedFilePath));
            }

            using var fileStream = new FileStream(encryptedFilePath, FileMode.Create);
            using var aes = Aes.Create();

            File.WriteAllBytes(keyFilePath, aes.Key);
            fileStream.Write(aes.IV, 0, aes.IV.Length);

            using var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using var writer = new StreamWriter(cryptoStream);

            writer.Write(plaintext);
        }

        public static string DecryptionFromFileToString_AES(string encryptedFilePath, string keyFilePath) {
            if (!File.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file path must be a valid, non-empty string.", nameof(encryptedFilePath));
            }

            if (!File.Exists(keyFilePath)) {
                throw new ArgumentException("The key file path must be a valid, non-empty string.", nameof(keyFilePath));
            }

            using var fileStream = new FileStream(encryptedFilePath, FileMode.Open);
            using var aes = Aes.Create();

            byte[] iv = new byte[aes.IV.Length];
            int numBytesToRead = aes.IV.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0) {
                int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                if (n == 0) {
                    break;
                }

                numBytesRead += n;
                numBytesToRead -= n;
            }

            byte[] key = File.ReadAllBytes(keyFilePath);

            using CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read);
            using StreamReader decryptReader = new(cryptoStream);

            return decryptReader.ReadToEnd();
        }
    }
}
