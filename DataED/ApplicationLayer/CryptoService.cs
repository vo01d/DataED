using System.Security.Cryptography;
using System.Text;

namespace DataED.ApplicationLayer {
    class CryptoService : ICryptoService {
        public void AESEncryptAndSaveToFile(string plaintext, string toStoreKeyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("The plaintext to encrypt cannot be null or empty.", nameof(plaintext));
            }

            if (!File.Exists(toStoreKeyFilePath)) {
                throw new ArgumentException("The key file path must be a valid, non-empty string.", nameof(toStoreKeyFilePath));
            }

            if (!File.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file path must be a valid, non-empty string.", nameof(encryptedFilePath));
            }

            using var fileStream = new FileStream(encryptedFilePath, FileMode.Create);
            using var aes = Aes.Create();

            File.WriteAllBytes(toStoreKeyFilePath, aes.Key);
            fileStream.Write(aes.IV, 0, aes.IV.Length);

            using var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using var writer = new StreamWriter(cryptoStream);

            writer.Write(plaintext);
        }

        public string AESDecryptFromFile(string encryptedFilePath, string keyFilePath) {
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

        public void RSAEncryptWithNewKeysAndSaveToFile(string plaintext, string toStorePrivateKeyFilePath, string toStorePublicKeyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("The plaintext to encrypt cannot be null or empty.", nameof(plaintext));
            }

            if (!File.Exists(toStorePrivateKeyFilePath)) {
                throw new ArgumentException("The private key file path must be a valid, non-empty string.", nameof(toStorePrivateKeyFilePath));
            }

            if (!File.Exists(toStorePublicKeyFilePath)) {
                throw new ArgumentException("The public key file path must be a valid, non-empty string.", nameof(toStorePublicKeyFilePath));
            }

            if (!File.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file path must be a valid, non-empty string.", nameof(encryptedFilePath));
            }

            using var rsa = RSA.Create();

            File.WriteAllBytes(toStorePrivateKeyFilePath, rsa.ExportRSAPrivateKey());
            File.WriteAllBytes(toStorePublicKeyFilePath, rsa.ExportRSAPublicKey());

            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.Pkcs1);
            File.WriteAllBytes(encryptedFilePath, encryptedData);
        }

        public void RSAEncryptWithExistKeyAndSaveToFile(string plaintext, string publicKeyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("The plaintext to encrypt cannot be null or empty.", nameof(plaintext));
            }

            if (!File.Exists(publicKeyFilePath)) {
                throw new ArgumentException("The public key file path must be a valid, non-empty string.", nameof(publicKeyFilePath));
            }

            if (!File.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file path must be a valid, non-empty string.", nameof(encryptedFilePath));
            }

            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(File.ReadAllBytes(publicKeyFilePath), out _);

            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.Pkcs1);
            File.WriteAllBytes(encryptedFilePath, encryptedData);
        }

        public string RSADecryptFromFile(string encryptedFilePath, string privateKeyFilePath) {
            if (!File.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file path must be a valid, non-empty string.", nameof(encryptedFilePath));
            }

            if (!File.Exists(privateKeyFilePath)) {
                throw new ArgumentException("The private key file path must be a valid, non-empty string.", nameof(privateKeyFilePath));
            }

            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(File.ReadAllBytes(privateKeyFilePath), out _);

            byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);
            byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
