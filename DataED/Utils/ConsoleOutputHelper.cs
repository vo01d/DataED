using DataED.PresentationLayer.Commands;

namespace DataED.Utils
{
    static class ConsoleOutputHelper {
        public static void WriteCommandsMenu(IEnumerable<Command> commands) {
            Console.WriteLine("Commands menu: ");
            foreach (var (command, index) in commands.Select((query, index) => (query, index))) {
                Console.WriteLine($"{index + 1}. {command.Name}");
            }
        }
    }
}
