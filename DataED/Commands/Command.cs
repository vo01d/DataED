namespace DataED.Commands {
    abstract class Command {
        public string Category { get; }
        public string Name { get; }

        protected Command(string category, string name) {
            Category = category;
            Name = name;
        }

        public abstract void Execute();
    }
}
