namespace DataED.PresentationLayer.Commands {
    public abstract class Command {
        public string Name { get; }

        protected Command(string name) {
            Name = name;
        }

        public abstract void Execute();
    }
}
