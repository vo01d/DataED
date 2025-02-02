using DataED.ApplicationLayer;

namespace DataED.PresentationLayer.Commands {
    class RSADecryptFromFileCommand : CryptoCommand {
        public RSADecryptFromFileCommand(string name, ICryptoService cryptoService) : base(name, cryptoService) {
        }

        public override void Execute() {
            Console.WriteLine("Enter the filepath where the encryption RSA private key is stored: ");
            string privateKeyFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(privateKeyFilePath),
                "The encryption RSA private key filepath cannot be null.");

            Console.WriteLine("Enter the filepath of the encrypted data: ");
            string encryptedFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(encryptedFilePath),
                "The encrypted data filepath cannot be null.");

            Console.WriteLine($"Plain text: {_cryptoService.RSADecryptFromFile(encryptedFilePath, privateKeyFilePath)}");
        }
    }
}
