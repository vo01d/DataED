using DataED.ApplicationLayer;
using NSubstitute;
using System.IO.Abstractions;

namespace DataEDNUnitTests {
    [TestFixture]
    public class CryptoService_RSA_Tests {
        private IFile _fileManager;
        private ICryptoService _cryptoService;

        private const string _plaintext = "The quick brown fox jumps over the lazy dog.";
        private const string _privateKeyFilePath = "This is a valid private key file path.";
        private const string _publicKeyFilePath = "This is a valid public key file path.";
        private const string _encryptedFilePath = "This is a valid encrypted file path.";
        private const string _invalidFilePath = "This is an invalid file path.";

        private readonly (string PrivateKeyFilePath, string PublicKeyFilePath, string EncryptedFilePath) _testFilePaths = (
            PrivateKeyFilePath: "..\\..\\..\\TestData\\privateKey_RSA.txt",
            PublicKeyFilePath: "..\\..\\..\\TestData\\publicKey_RSA.txt",
            EncryptedFilePath: "..\\..\\..\\TestData\\encrypted_RSA.txt"
        );
        private readonly (byte[] PrivateKey, byte[] PublicKey, byte[] Encrypted) _testEncryptionData;

        public CryptoService_RSA_Tests() {
            _testEncryptionData.PrivateKey = File.ReadAllBytes(_testFilePaths.PrivateKeyFilePath);
            _testEncryptionData.PublicKey = File.ReadAllBytes(_testFilePaths.PublicKeyFilePath);
            _testEncryptionData.Encrypted = File.ReadAllBytes(_testFilePaths.EncryptedFilePath);
        }

        [SetUp]
        public void SetUp() {
            _fileManager = Substitute.For<IFile>();
            _cryptoService = new CryptoService(_fileManager);

            _fileManager.Exists(_privateKeyFilePath).Returns(true);
            _fileManager.Exists(_publicKeyFilePath).Returns(true);
            _fileManager.Exists(_encryptedFilePath).Returns(true);
            _fileManager.Exists(_invalidFilePath).Returns(false);
        }

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

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _invalidFilePath, _publicKeyFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "private key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_InvalidPublicKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "publicKeyFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _privateKeyFilePath, _invalidFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "public key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _privateKeyFilePath, _publicKeyFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithNewKeysAndSaveToFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithNewKeysAndSaveToFile_ValidInputs_WritesExpectedData() {
            _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(_plaintext, _privateKeyFilePath, _publicKeyFilePath, _encryptedFilePath);

            _fileManager.Received().WriteAllBytes(_privateKeyFilePath, Arg.Any<byte[]>());
            _fileManager.Received().WriteAllBytes(_publicKeyFilePath, Arg.Any<byte[]>());
            _fileManager.Received().WriteAllBytes(_encryptedFilePath, Arg.Any<byte[]>());
        }

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

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithExistKeyAndSaveToFile(_plaintext, _invalidFilePath, _encryptedFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithExistKeyAndSaveToFile", "public key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithExistKeyAndSaveToFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSAEncryptWithExistKeyAndSaveToFile(_plaintext, _publicKeyFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSAEncryptWithExistKeyAndSaveToFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSAEncryptWithExistKeyAndSaveToFile_ValidInputs_WritesExpectedData() {
            _fileManager.ReadAllBytes(_publicKeyFilePath).Returns(_testEncryptionData.PublicKey);

            _cryptoService.RSAEncryptWithExistKeyAndSaveToFile(_plaintext, _publicKeyFilePath, _encryptedFilePath);

            _fileManager.Received().WriteAllBytes(_encryptedFilePath, Arg.Any<byte[]>());
        }

        [Test]
        public void RSADecryptFromFile_InvalidEncryptedFilePath_ThrowsArgumentException() {
            string expectedParamName = "encryptedFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSADecryptFromFile(_invalidFilePath, _privateKeyFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSADecryptFromFile", "encrypted file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSADecryptFromFile_InvalidPrivateKeyFilePath_ThrowsArgumentException() {
            string expectedParamName = "privateKeyFilePath";

            var ex = Assert.Throws<ArgumentException>(
                () => _cryptoService.RSADecryptFromFile(_encryptedFilePath, _invalidFilePath),
                CryptoErrorMessages.MethodShouldThrowArgumentException("RSADecryptFromFile", "private key file path is invalid")
            );
            Assert.That(ex.ParamName, Is.EqualTo(expectedParamName), CryptoErrorMessages.ExpectedExceptionParameter(expectedParamName));
        }

        [Test]
        public void RSADecryptFromFile_ValidInputs_ReturnsOriginalPlaintext() {
            _fileManager.ReadAllBytes(_privateKeyFilePath).Returns(_testEncryptionData.PrivateKey);
            _fileManager.ReadAllBytes(_encryptedFilePath).Returns(_testEncryptionData.Encrypted);

            string decrypted = _cryptoService.RSADecryptFromFile(_encryptedFilePath, _privateKeyFilePath);

            Assert.That(decrypted, Is.EqualTo(_plaintext), CryptoErrorMessages.DecryptionMismatchError);
        }
    }
}
