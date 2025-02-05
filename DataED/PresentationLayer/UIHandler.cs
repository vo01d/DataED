using DataED.Utils;

namespace DataED.PresentationLayer {
    public class UIHandler {
        private readonly Invoker _invoker;
        public UIHandler(Invoker invoker) {
            _invoker = invoker;
        }
        public void Start() {
            ConsoleOutputHelper.WriteCommandsMenu(_invoker.Commands);
            Console.WriteLine();

            while (true) {
                Console.Write("Enter command number: ");
                string userInput = Console.ReadLine() ?? throw new ArgumentNullException();

                int queryNumber;
                try {
                    queryNumber = ToInt32Parser.ParseInRange(userInput, 1, _invoker.CommandsCount);
                }
                catch (FormatException) {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                    continue;
                }
                catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is OverflowException) {
                    Console.WriteLine($"Unknown command! Please enter a number in range from 1 to {_invoker.CommandsCount}.");
                    continue;
                }

                _invoker.ExecuteCommand(queryNumber);
            }
        }
    }
}
