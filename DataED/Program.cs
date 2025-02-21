using DataED.ApplicationLayer;
using DataED.PresentationLayer;
using DataED.PresentationLayer.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

namespace DataED {
    public class Program {
        static void Main(string[] args) {
            IServiceCollection services = new ServiceCollection(); 

            services.AddSingleton<Invoker>();
            services.AddTransient<IFileSystem, FileSystem>();
            services.AddTransient<IFile, FileWrapper>();
            services.AddTransient<ICryptoService, CryptoService>();
            services.AddSingleton<UIHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var cryptoService = serviceProvider.GetRequiredService<ICryptoService>();
            var invoker = serviceProvider.GetRequiredService<Invoker>()
                .AddCommand(new AESEncryptAndSaveToFileCommand("Encrypt console input using AES and save to a file", cryptoService))
                .AddCommand(new AESDecryptFromFileCommand("Decrypt an AES-encrypted file and display the output", cryptoService))
                .AddCommand(new RSAEncryptWithNewKeysAndSaveToFileCommand("Generate new RSA keys and encrypt console input", cryptoService))
                .AddCommand(new RSAEncryptWithExistKeyAndSaveToFileCommand("Encrypt console input using an existing RSA public key", cryptoService))
                .AddCommand(new RSADecryptFromFileCommand("Decrypt an RSA-encrypted file using the private key and display the output", cryptoService));

            var UIHandler = serviceProvider.GetRequiredService<UIHandler>();
            UIHandler.Start();
        }
    }
}
