using DataED.ApplicationLayer;

namespace DataEDNUnitTests {
    public class CryptoService_AES_Tests {
        private CryptoService _cryptoService = new CryptoService();
        private string _plaintext;
        private string _keyFilePath;
        private string _encryptedFilePath;

        [SetUp]
        public void SetUp() {
            _plaintext = "Test text";
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

        // AESEncryptAndSaveToFile & AESDecryptFromFile
        [Test]
        public void AESEncryptAndSaveToFile_AESDecryptFromFile_ValidInput_CreatesEncryptedFileAndKeyFileAndDecryptedCorrectly() {
            _cryptoService.AESEncryptAndSaveToFile(_plaintext, _keyFilePath, _encryptedFilePath);

            Assert.That(File.Exists(_keyFilePath), Is.True, "Key file was not created.");
            Assert.That(File.Exists(_encryptedFilePath), Is.True, "Encrypted file was not created.");
            Assert.That(File.ReadAllBytes(_keyFilePath).Length, Is.GreaterThan(0), "Key file is empty.");
            Assert.That(File.ReadAllBytes(_encryptedFilePath).Length, Is.GreaterThan(0), "Encrypted file is empty.");
            Assert.That(_cryptoService.AESDecryptFromFile(_encryptedFilePath, _keyFilePath), Is.EqualTo(_plaintext), "Decrypted text does not match original plaintext.");
        }

        // AESEncryptAndSaveToFile 
        [Test]
        public void AESEncryptAndSaveToFile_NullPlaintext_ThrowsArgumentException() {
            var ex = Assert.Throws<ArgumentException>(() => _cryptoService.AESEncryptAndSaveToFile(null, _keyFilePath, _encryptedFilePath));
            Assert.That(ex.ParamName, Is.EqualTo("plaintext"));
        }

        [Test]
        public void AESEncryptAndSaveToFile_EmptyPlaintext_ThrowsArgumentException() {
            var ex = Assert.Throws<ArgumentException>(() => _cryptoService.AESEncryptAndSaveToFile("", _keyFilePath, _encryptedFilePath));
            Assert.That(ex.ParamName, Is.EqualTo("plaintext"));
        }

        [Test]
        public void AESEncryptAndSaveToFile_InvalidKeyFilePath_ThrowsArgumentException() {
            string invalidKeyFilepath = Path.Combine(Path.GetTempPath(), "nonexistent_directory", "key.key");

            var ex = Assert.Throws<ArgumentException>(() => _cryptoService.AESEncryptAndSaveToFile(_plaintext, invalidKeyFilepath, _encryptedFilePath));
            Assert.That(ex.ParamName, Is.EqualTo("toStoreKeyFilePath"));
        }

        [Test]
        public void AESEncryptAndSaveToFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string invalidEncryptedFilepath = Path.Combine(Path.GetTempPath(), "nonexistent_directory", "encrypted.encrypted");

            var ex = Assert.Throws<ArgumentException>(() => _cryptoService.AESEncryptAndSaveToFile(_plaintext, _keyFilePath, invalidEncryptedFilepath));
            Assert.That(ex.ParamName, Is.EqualTo("encryptedFilePath"));
        }

        // AESDecryptFromFile
        [Test]
        public void AESDecryptFromFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string invalidEncryptedFilepath = Path.Combine(Path.GetTempPath(), "nonexistent_directory", "encrypted.encrypted");

            var ex = Assert.Throws<ArgumentException>(() => _cryptoService.AESDecryptFromFile(invalidEncryptedFilepath, _keyFilePath));
            Assert.That(ex.ParamName, Is.EqualTo("encryptedFilePath"));
        }

        [Test]
        public void AESDecryptFromFile_InvalidKeyFilePath_ThrowsArgumentException() {
            string invalidKeyFilepath = Path.Combine(Path.GetTempPath(), "nonexistent_directory", "key.key");

            var ex = Assert.Throws<ArgumentException>(() => _cryptoService.AESDecryptFromFile(_encryptedFilePath, invalidKeyFilepath));
            Assert.That(ex.ParamName, Is.EqualTo("keyFilePath"));
        }

        // How to check incorrect decryption?
        // 1. Empty file or key
        // 2. Incorrect key, another key...
    }
}
