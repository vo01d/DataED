using DataED.Commands;

namespace DataED.Utils {
    static class ConsoleOutputHelper {
        public static void WriteCommandsMenu(IEnumerable<Command> commands) {
            Console.WriteLine("Commands menu: ");
            int commandNumber = 1;
            foreach (var categoryGroup in commands.GroupBy(command => command.Category)) {
                Console.WriteLine($"{categoryGroup.Key}: ");
                foreach (var command in categoryGroup.Select(command => command)) {
                    Console.WriteLine($"{commandNumber++}. {command.Name}");
                }
            }
        }
    }
}
