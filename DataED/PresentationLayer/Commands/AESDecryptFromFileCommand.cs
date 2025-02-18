using DataED.ApplicationLayer;

namespace DataED.PresentationLayer.Commands {
    public class AESDecryptFromFileCommand : CryptoCommand  {
        public AESDecryptFromFileCommand(string name, ICryptoService cryptoService) : base(name, cryptoService) {
        }

        public override void Execute() {
            Console.WriteLine("Enter the filepath where the encryption key is stored: ");
            string keyFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(keyFilePath),
                "The encryption key filepath cannot be null.");

            Console.WriteLine("Enter the filepath of the encrypted data: ");
            string encryptedFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(encryptedFilePath),
                "The encrypted data filepath cannot be null.");

            string decrypted = string.Empty;
            try {
                decrypted = _cryptoService.AESDecryptFromFile(encryptedFilePath, keyFilePath);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Plain text: {decrypted}");
        }
    }
}
