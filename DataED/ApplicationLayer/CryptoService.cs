using System.IO.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace DataED.ApplicationLayer {
    public class CryptoService : ICryptoService {
        private IFile _fileManager;

        public CryptoService(IFile fileManager) {
            _fileManager = fileManager;
        }

        public void AESEncryptAndSaveToFile(string plaintext, string keyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("Plaintext for encryption cannot be null, empty, or consist only of whitespace.", nameof(plaintext));
            }

            if (!_fileManager.Exists(keyFilePath)) {
                throw new ArgumentException("The key file was not found at the specified path or the path is invalid.", nameof(keyFilePath));
            }

            if (!_fileManager.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            using var aes = Aes.Create();
            using var encryptor = aes.CreateEncryptor();

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plaintext); 
            byte[] encrypted = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            _fileManager.WriteAllBytes(keyFilePath, aes.Key); 
            _fileManager.WriteAllBytes(encryptedFilePath, [.. aes.IV, .. encrypted]);
        }

        public string AESDecryptFromFile(string encryptedFilePath, string keyFilePath) {
            if (!_fileManager.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            if (!_fileManager.Exists(keyFilePath)) {
                throw new ArgumentException("The key file was not found at the specified path or the path is invalid.", nameof(keyFilePath));
            }

            using var aes = Aes.Create();

            byte[] key = _fileManager.ReadAllBytes(keyFilePath); 
            byte[] encryptedFileData = _fileManager.ReadAllBytes(encryptedFilePath);
            byte[] IV = encryptedFileData.Take(aes.IV.Length).ToArray();
            byte[] encrypted = encryptedFileData.Skip(aes.IV.Length).ToArray();
            
            using var decryptor = aes.CreateDecryptor(key, IV);
            byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);

            return Encoding.UTF8.GetString(decrypted);
        }

        public void RSAEncryptWithNewKeysAndSaveToFile(string plaintext, string privateKeyFilePath, string publicKeyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("Plaintext for encryption cannot be null, empty, or consist only of whitespace.", nameof(plaintext));
            }

            if (!_fileManager.Exists(privateKeyFilePath)) {
                throw new ArgumentException("The private key file was not found at the specified path or the path is invalid.", nameof(privateKeyFilePath));
            }

            if (!_fileManager.Exists(publicKeyFilePath)) {
                throw new ArgumentException("The public key file was not found at the specified path or the path is invalid.", nameof(publicKeyFilePath));
            }

            if (!_fileManager.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            using var rsa = RSA.Create();

            _fileManager.WriteAllBytes(privateKeyFilePath, rsa.ExportRSAPrivateKey());
            _fileManager.WriteAllBytes(publicKeyFilePath, rsa.ExportRSAPublicKey());

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] encryptedData = rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);

            _fileManager.WriteAllBytes(encryptedFilePath, encryptedData);
        }

        public void RSAEncryptWithExistKeyAndSaveToFile(string plaintext, string publicKeyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("Plaintext for encryption cannot be null, empty, or consist only of whitespace.", nameof(plaintext));
            }

            if (!_fileManager.Exists(publicKeyFilePath)) {
                throw new ArgumentException("The public key file was not found at the specified path or the path is invalid.", nameof(publicKeyFilePath));
            }

            if (!_fileManager.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            using var rsa = RSA.Create();

            byte[] publicKey = _fileManager.ReadAllBytes(publicKeyFilePath);
            rsa.ImportRSAPublicKey(publicKey, out _);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] encryptedData = rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);

            _fileManager.WriteAllBytes(encryptedFilePath, encryptedData);
        }

        public string RSADecryptFromFile(string encryptedFilePath, string privateKeyFilePath) {
            if (!_fileManager.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            if (!_fileManager.Exists(privateKeyFilePath)) {
                throw new ArgumentException("The private key file was not found at the specified path or the path is invalid.", nameof(privateKeyFilePath));
            }

            using var rsa = RSA.Create();

            byte[] privateKey = _fileManager.ReadAllBytes(privateKeyFilePath);
            rsa.ImportRSAPrivateKey(privateKey, out _);

            byte[] encryptedData = _fileManager.ReadAllBytes(encryptedFilePath);
            byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
