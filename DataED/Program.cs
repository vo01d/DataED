using DataED.Commands;
using DataED.Utils;

namespace DataED {
    class Program {
        static void Main(string[] args) {
            var invoker = new Invoker();

            invoker
                .AddCommand(new ConsoleAESEncryptionCommand("Encryption", "Encrypt console input"))
                .AddCommand(new AESDecryptionCommand("Decryption", "Decrypt file"));

            ConsoleOutputHelper.WriteCommandsMenu(invoker.Commands);
            Console.WriteLine();

            while (true) {
                Console.Write("Enter command number: ");
                string userInput = Console.ReadLine() ?? throw new ArgumentNullException();

                int queryNumber;
                try {
                    queryNumber = InputValidationHelper.ValidateInt32InRange(userInput, 1, invoker.CommandsCount);
                }
                catch (FormatException) {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                    continue;
                }
                catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is OverflowException) {
                    Console.WriteLine($"Unknown command! Please enter a number in range from 1 to {invoker.CommandsCount}.");
                    continue;
                }

                invoker.ExecuteCommand(queryNumber);
            }
        }
    }
}
