using DataED.PresentationLayer.Commands;

namespace DataED.PresentationLayer {
    public class Invoker {
        public List<Command> Commands { get; } = [];
        public int CommandsCount => Commands.Count;

        public Invoker AddCommand(Command command) {
            Commands.Add(command);
            return this;
        }

        public void ExecuteCommand(int commandNumber) {
            Commands[commandNumber - 1].Execute();
        }
    }
}
