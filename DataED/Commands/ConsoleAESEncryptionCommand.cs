using DataED.Utils;

namespace DataED.Commands {
    class ConsoleAESEncryptionCommand : Command {
        public ConsoleAESEncryptionCommand(string category, string name) : base(category, name) {
        }

        // where to catch exeptions ???
        public override void Execute() {
            Console.WriteLine("Enter the plaintext to encrypt: ");
            string plaintext = Console.ReadLine() ?? throw new ArgumentNullException(nameof(plaintext), 
                "The plaintext cannot be null.");

            Console.WriteLine("Enter the file path to save the encryption key: ");
            string keyFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(keyFilePath),
                "The encryption key file path cannot be null.");

            Console.WriteLine("Enter the file path to save the encrypted data: ");
            string encryptedFilePath = Console.ReadLine() ?? throw new ArgumentNullException(nameof(encryptedFilePath),
                "The encrypted data file path cannot be null.");

            EDHelper.EncryptionToFile_AES(plaintext, keyFilePath, encryptedFilePath);
        }
    }
}
