using DataED.ApplicationLayer;
using System.Security.Cryptography;

namespace DataEDNUnitTests {
    [TestFixture]
    public class CryptoService_RSA_Tests {
        private IFileService _fileService;
        private ICryptoService _cryptoService;

        private string _plaintext;
        private string _privateKeyFilePath;
        private string _publicKeyFilePath;
        private string _encryptedFilePath;

        private string _invalidFilePath;
        private bool _isInvalidFileExists;

        public CryptoService_RSA_Tests() {
            _fileService = new FileService();
            _cryptoService = new CryptoService(_fileService);
            _plaintext = "The quick brown fox jumps over the lazy dog.";
            _invalidFilePath = Path.Combine(Path.GetTempPath(), "nonexistentDirectory", "nonexistentFile.nonexistentExtension");
            _isInvalidFileExists = File.Exists(_invalidFilePath);
        }

        [SetUp]
        public void SetUp() {
            _cryptoService = new CryptoService(_fileService);
            _plaintext = "The quick brown fox jumps over the lazy dog.";
            _privateKeyFilePath = Path.GetTempFileName();
            _publicKeyFilePath = Path.GetTempFileName();
            _encryptedFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown() {
            if (File.Exists(_privateKeyFilePath)) {
                File.Delete(_privateKeyFilePath);
            }

            if (File.Exists(_publicKeyFilePath)) {
                File.Delete(_publicKeyFilePath);
            }

            if (File.Exists(_encryptedFilePath)) {
                File.Delete(_encryptedFilePath);
            }
        }

        // RSAEncryptWithNewKeysAndSaveToFile()
        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_NullPlaintext_ThrowsArgumentException() {
            string expectedParamName = "plaintext";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(null, _privateKeyFilePath, _publicKeyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "plaintext is null")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_EmptyPlaintext_ThrowsArgumentException() {
            string expectedParamName = "plaintext";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile("", _privateKeyFilePath, _publicKeyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "plaintext is empty")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_InvalidPrivateKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "privateKeyFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _invalidFilePath, _publicKeyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "private key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_InvalidPublicKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "publicKeyFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _privateKeyFilePath, _invalidFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "public key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _privateKeyFilePath, _publicKeyFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        // RSAEncryptWithExistKeyAndSaveToFile()
        [Test]
        public void RSAEncryptWithExistKeyAndSaveToFile_NullPlaintext_ThrowsArgumentException() {
            string expectedParamName = "plaintext";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithExistKeyAndSaveToFile(null, _publicKeyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithExistKeyAndSaveToFile", "plaintext is null")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithExistKeyAndSaveToFile_EmptyPlaintext_ThrowsArgumentException() {
            string expectedParamName = "plaintext";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithExistKeyAndSaveToFile("", _publicKeyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithExistKeyAndSaveToFile", "plaintext is empty")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithExistKeyAndSaveToFile_InvalidPublicKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "publicKeyFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithExistKeyAndSaveToFile(_plaintext, _invalidFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithExistKeyAndSaveToFile", "public key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithExistKeyAndSaveToFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithExistKeyAndSaveToFile(_plaintext, _publicKeyFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithExistKeyAndSaveToFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        // RSADecryptFromFile()
        [Test]
        public void RSADecryptFromFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSADecryptFromFile(_invalidFilePath, _privateKeyFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSADecryptFromFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSADecryptFromFile_InvalidPrivateKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "privateKeyFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSADecryptFromFile(_encryptedFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSADecryptFromFile", "private key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        // RSAEncryptWithNewKeysAndSaveToFile() & RSADecryptFromFile()
        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_RSADecryptFromFile_ValidInput_CreatesEncryptedFileAndKeyFilesAndDecryptedCorrectly() {
            _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _privateKeyFilePath, _publicKeyFilePath, _encryptedFilePath);
            string decrypted = _cryptoService.RSADecryptFromFile(_encryptedFilePath, _privateKeyFilePath);

            Assert.That(decrypted, Is.EqualTo(_plaintext), CryptoErrorMessages.DecryptionMismatchError);
        }

        // RSAEncryptWithExistKeyAndSaveToFile() & RSADecryptFromFile()
        [Test]
        public void RSAEncryptWithExistKeyAndSaveToFile_RSADecryptFromFile_ValidInput_CreatesEncryptedFileAndKeyFilesAndDecryptedCorrectly() {
            using var rsa = RSA.Create();
            _fileService.WriteAllBytes(_privateKeyFilePath, rsa.ExportRSAPrivateKey());
            _fileService.WriteAllBytes(_publicKeyFilePath, rsa.ExportRSAPublicKey());

            _cryptoService.RSAEncryptWithExistKeyAndSaveToFile(_plaintext, _publicKeyFilePath, _encryptedFilePath);
            string decrypted = _cryptoService.RSADecryptFromFile(_encryptedFilePath, _privateKeyFilePath);

            Assert.That(decrypted, Is.EqualTo(_plaintext), CryptoErrorMessages.DecryptionMismatchError);
        }
    }
}
