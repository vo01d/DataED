using DataED.ApplicationLayer;
using NSubstitute;
using System.IO.Abstractions;

namespace DataEDNUnitTests {
    [TestFixture]
    public class CryptoService_AES_Tests {
        private IFile _fileManager;
        private ICryptoService _cryptoService;

        private const string _plaintext = "The quick brown fox jumps over the lazy dog.";
        private const string _keyFilePath = "This is a valid key file path.";
        private const string _encryptedFilePath = "This is a valid encrypted file path.";
        private const string _invalidFilePath = "This is an invalid file path.";

        private readonly (string KeyFilePath, string EncryptedFilePath) _testFilePaths = (
            KeyFilePath: "..\\..\\..\\TestData\\key_AES.txt",
            EncryptedFilePath: "..\\..\\..\\TestData\\encrypted_AES.txt"
        );
        private readonly (byte[] Key, byte[] Encrypted) _testEncryptionData;

        public CryptoService_AES_Tests() {
            _testEncryptionData.Key = File.ReadAllBytes(_testFilePaths.KeyFilePath);
            _testEncryptionData.Encrypted = File.ReadAllBytes(_testFilePaths.EncryptedFilePath);
        }

        [SetUp]
        public void SetUp() {
            _fileManager = Substitute.For<IFile>();
            _cryptoService = new CryptoService(_fileManager);

            _fileManager.Exists(_keyFilePath).Returns(true);
            _fileManager.Exists(_encryptedFilePath).Returns(true);
            _fileManager.Exists(_invalidFilePath).Returns(false);
        }

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

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESEncryptAndSaveToFile(_plaintext, _invalidFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESEncryptAndSaveToFile", "key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESEncryptAndSaveToFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESEncryptAndSaveToFile(_plaintext, _keyFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESEncryptAndSaveToFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESEncryptAndSaveToFile_ValidInputs_WritesExpectedData() {
            _cryptoService.AESEncryptAndSaveToFile(_plaintext, _keyFilePath, _encryptedFilePath);

            _fileManager.Received().WriteAllBytes(_keyFilePath, Arg.Any<byte[]>());
            _fileManager.Received().WriteAllBytes(_encryptedFilePath, Arg.Any<byte[]>());
        }

        [Test]
        public void AESDecryptFromFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESDecryptFromFile(_invalidFilePath, _keyFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESDecryptFromFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESDecryptFromFile_InvalidKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "keyFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.AESDecryptFromFile(_encryptedFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("AESDecryptFromFile", "key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void AESDecryptFromFile_ValidInputs_ReturnsOriginalPlaintext() {
            _fileManager.ReadAllBytes(_keyFilePath).Returns(_testEncryptionData.Key);
            _fileManager.ReadAllBytes(_encryptedFilePath).Returns(_testEncryptionData.Encrypted);

            string decrypted = _cryptoService.AESDecryptFromFile(_encryptedFilePath, _keyFilePath);

            Assert.That(decrypted, Is.EqualTo(_plaintext), CryptoErrorMessages.DecryptionMismatchError);
        }
    }
}
