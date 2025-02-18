using DataED.ApplicationLayer;

namespace DataEDNUnitTests {
    [TestFixture]
    public class CryptoService_AES_Tests {
        private ICryptoService _cryptoService;

        private string _plaintext;
        private string _keyFilePath;
        private string _encryptedFilePath;

        private string _invalidFilePath;
        private bool _isInvalidFileExists;

        public CryptoService_AES_Tests() {
            _cryptoService = new CryptoService(new FileService());
            _plaintext = "The quick brown fox jumps over the lazy dog.";
            _invalidFilePath = Path.Combine(Path.GetTempPath(), "nonexistentDirectory", "nonexistentFile.nonexistentExtension");
            _isInvalidFileExists = File.Exists(_invalidFilePath);
        }

        [SetUp]
        public void SetUp() {
            _keyFilePath = Path.GetTempFileName();
            _encryptedFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown() {
            if (File.Exists(_keyFilePath)) { 
                File.Delete(_keyFilePath);
            }

            if (File.Exists(_encryptedFilePath)) {
                File.Delete(_encryptedFilePath);
            }
        }

        // AESEncryptAndSaveToFile()
        [Test]
        public void AESEncryptAndSaveToFile_NullPlaintext_ThrowsArgumentException() {
            string expectedParamName = "plaintext";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESEncryptAndSaveToFile(null, _keyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESEncryptAndSaveToFile", "plaintext is null")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESEncryptAndSaveToFile_EmptyPlaintext_ThrowsArgumentException() {
            string expectedParamName = "plaintext";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESEncryptAndSaveToFile("", _keyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESEncryptAndSaveToFile", "plaintext is empty")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESEncryptAndSaveToFile_InvalidKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "keyFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESEncryptAndSaveToFile(_plaintext, _invalidFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESEncryptAndSaveToFile", "key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESEncryptAndSaveToFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESEncryptAndSaveToFile(_plaintext, _keyFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESEncryptAndSaveToFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        // AESDecryptFromFile()
        [Test]
        public void AESDecryptFromFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESDecryptFromFile(_invalidFilePath, _keyFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESDecryptFromFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESDecryptFromFile_InvalidKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "keyFilePath";

            Assert.That(_isInvalidFileExists, Is.False, CryptoErrorMessages.InvalidFilePathError);
            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESDecryptFromFile(_encryptedFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESDecryptFromFile", "key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        // AESEncryptAndSaveToFile() & AESDecryptFromFile()
        [Test]
        public void AESEncryptAndSaveToFile_AESDecryptFromFile_ValidInput_CreatesEncryptedFileAndKeyFileAndDecryptedCorrectly() {
            _cryptoService.AESEncryptAndSaveToFile(_plaintext, _keyFilePath, _encryptedFilePath);
            string decrypted = _cryptoService.AESDecryptFromFile(_encryptedFilePath, _keyFilePath);

            Assert.That(decrypted, Is.EqualTo(_plaintext), CryptoErrorMessages.DecryptionMismatchError);
        }
    }
}
