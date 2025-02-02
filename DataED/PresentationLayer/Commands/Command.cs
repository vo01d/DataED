namespace DataED.PresentationLayer.Commands {
    abstract class Command {
        public string Name { get; }

        protected Command(string name) {
            Name = name;
        }

        public abstract void Execute();
    }
}
