using System.Security.Cryptography;
using System.Text;

namespace DataED.ApplicationLayer {
    public class CryptoService : ICryptoService {
        private IFileService _fileService;

        public CryptoService(IFileService fileService) {
            _fileService = fileService;
        }

        public void AESEncryptAndSaveToFile(string plaintext, string keyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("Plaintext for encryption cannot be null, empty, or consist only of whitespace.", nameof(plaintext));
            }

            if (!_fileService.Exists(keyFilePath)) {
                throw new ArgumentException("The key file was not found at the specified path or the path is invalid.", nameof(keyFilePath));
            }

            if (!_fileService.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            using var aes = Aes.Create();

            using var encryptor = aes.CreateEncryptor();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plaintext); 
            byte[] encrypted = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            _fileService.WriteAllBytes(keyFilePath, aes.Key); 
            _fileService.WriteAllBytes(encryptedFilePath, [.. aes.IV, .. encrypted]);
        }

        public string AESDecryptFromFile(string encryptedFilePath, string keyFilePath) {
            if (!_fileService.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            if (!_fileService.Exists(keyFilePath)) {
                throw new ArgumentException("The key file was not found at the specified path or the path is invalid.", nameof(keyFilePath));
            }

            using var aes = Aes.Create();

            byte[] key = File.ReadAllBytes(keyFilePath); 
            byte[] encryptedFileData = File.ReadAllBytes(encryptedFilePath);
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

            if (!_fileService.Exists(privateKeyFilePath)) {
                throw new ArgumentException("The private key file was not found at the specified path or the path is invalid.", nameof(privateKeyFilePath));
            }

            if (!_fileService.Exists(publicKeyFilePath)) {
                throw new ArgumentException("The public key file was not found at the specified path or the path is invalid.", nameof(publicKeyFilePath));
            }

            if (!_fileService.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            using var rsa = RSA.Create();

            _fileService.WriteAllBytes(privateKeyFilePath, rsa.ExportRSAPrivateKey());
            _fileService.WriteAllBytes(publicKeyFilePath, rsa.ExportRSAPublicKey());

            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.Pkcs1);
            _fileService.WriteAllBytes(encryptedFilePath, encryptedData);
        }

        public void RSAEncryptWithExistKeyAndSaveToFile(string plaintext, string publicKeyFilePath, string encryptedFilePath) {
            if (string.IsNullOrWhiteSpace(plaintext)) {
                throw new ArgumentException("Plaintext for encryption cannot be null, empty, or consist only of whitespace.", nameof(plaintext));
            }

            if (!_fileService.Exists(publicKeyFilePath)) {
                throw new ArgumentException("The public key file was not found at the specified path or the path is invalid.", nameof(publicKeyFilePath));
            }

            if (!_fileService.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(_fileService.ReadAllBytes(publicKeyFilePath), out _);

            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.Pkcs1);
            _fileService.WriteAllBytes(encryptedFilePath, encryptedData);
        }

        public string RSADecryptFromFile(string encryptedFilePath, string privateKeyFilePath) {
            if (!_fileService.Exists(encryptedFilePath)) {
                throw new ArgumentException("The encrypted file was not found at the specified path or the path is invalid.", nameof(encryptedFilePath));
            }

            if (!_fileService.Exists(privateKeyFilePath)) {
                throw new ArgumentException("The private key file was not found at the specified path or the path is invalid.", nameof(privateKeyFilePath));
            }

            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(_fileService.ReadAllBytes(privateKeyFilePath), out _);

            byte[] encryptedData = _fileService.ReadAllBytes(encryptedFilePath);
            byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
