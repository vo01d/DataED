using DataED.Utils;

namespace DataED.Commands {
    class AESDecryptionCommand : Command {
        public AESDecryptionCommand(string category, string name) : base(category, name) {
        }

        public override void Execute() {
            Console.WriteLine("Enter the file path where the encryption key is stored: ");
            string keyFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(keyFilePath),
                "The encryption key file path cannot be null.");

            Console.WriteLine("Enter the file path of the encrypted data: ");
            string encryptedFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(encryptedFilePath),
                "The encrypted data file path cannot be null.");

            Console.WriteLine($"Plain text: {EDHelper.DecryptionFromFileToString_AES(encryptedFilePath, keyFilePath)}");
        }
    }
}
