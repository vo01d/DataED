using DataED.ApplicationLayer;

namespace DataED.PresentationLayer.Commands {
    public abstract class CryptoCommand : Command {
        protected readonly ICryptoService _cryptoService;

        protected CryptoCommand(string name, ICryptoService cryptoService) : base(name) {
            _cryptoService = cryptoService;
        }
    }
}
