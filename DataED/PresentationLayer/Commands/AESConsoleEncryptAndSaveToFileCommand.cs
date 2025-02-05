using DataED.ApplicationLayer;

namespace DataED.PresentationLayer.Commands {
    public class AESConsoleEncryptAndSaveToFileCommand : CryptoCommand {
        public AESConsoleEncryptAndSaveToFileCommand(string name, ICryptoService cryptoService) : base(name, cryptoService) {
        }

        public override void Execute() {
            Console.WriteLine("Enter the plaintext to encrypt: ");
            string plaintext = Console.ReadLine() ?? throw new ArgumentNullException(nameof(plaintext),
                "The plaintext cannot be null.");

            Console.WriteLine("Enter the filepath to save the encryption key: ");
            string keyFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(keyFilePath),
                "The encryption key filepath cannot be null.");

            Console.WriteLine("Enter the filepath to save the encrypted data: ");
            string encryptedFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(encryptedFilePath),
                "The encrypted data filepath cannot be null.");

            _cryptoService.AESEncryptAndSaveToFile(plaintext, keyFilePath, encryptedFilePath);
        }
    }
}
