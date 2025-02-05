using DataED.ApplicationLayer;

namespace DataED.PresentationLayer.Commands {
    public class RSAConsoleEncryptWithNewKeysAndSaveToFileCommand : CryptoCommand {
        public RSAConsoleEncryptWithNewKeysAndSaveToFileCommand(string name, ICryptoService cryptoService) : base(name, cryptoService) {
        }

        public override void Execute() {
            Console.WriteLine("Enter the plaintext to encrypt: ");
            string plaintext = Console.ReadLine() ?? throw new ArgumentNullException(nameof(plaintext),
                "The plaintext cannot be null.");

            Console.WriteLine("Enter the filepath to save the RSA private encryption key: ");
            string privateKeyFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(privateKeyFilePath),
                "The encryption RSA private key filepath cannot be null.");

            Console.WriteLine("Enter the filepath to save the RSA public encryption key: ");
            string publicKeyFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(publicKeyFilePath),
                "The encryption RSA public key filepath cannot be null.");

            Console.WriteLine("Enter the filepath to save the encrypted data: ");
            string encryptedFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(encryptedFilePath),
                "The encrypted data filepath cannot be null.");

            _cryptoService.RSAEncryptWithNewKeysAndSaveToFile(plaintext, privateKeyFilePath, publicKeyFilePath, encryptedFilePath);
        }
    }
}
