using DataED.ApplicationLayer;
using DataED.PresentationLayer;
using DataED.PresentationLayer.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DataED {
    public class Program {
        static void Main(string[] args) {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<Invoker>();
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddSingleton<UIHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var cryptoService = serviceProvider.GetRequiredService<ICryptoService>();
            var invoker = serviceProvider.GetRequiredService<Invoker>()
                .AddCommand(new AESConsoleEncryptAndSaveToFileCommand("Encrypt console input using AES and save to a file", cryptoService))
                .AddCommand(new AESDecryptFromFileCommand("Decrypt an AES-encrypted file and display the output", cryptoService))
                .AddCommand(new RSAConsoleEncryptWithNewKeysAndSaveToFileCommand("Generate new RSA keys and encrypt console input", cryptoService))
                .AddCommand(new RSAConsoleEncryptWithExistKeyAndSaveToFileCommand("Encrypt console input using an existing RSA public key", cryptoService))
                .AddCommand(new RSADecryptFromFileCommand("Decrypt an RSA-encrypted file using the private key and display the output", cryptoService));

            var UIHandler = serviceProvider.GetRequiredService<UIHandler>();
            UIHandler.Start();
        }
    }
}
