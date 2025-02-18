namespace DataED.ApplicationLayer {
    public interface ICryptoService {
        void AESEncryptAndSaveToFile(string plaintext, string keyFilePath, string encryptedFilePath);
        string AESDecryptFromFile(string encryptedFilePath, string keyFilePath);
        void RSAEncryptWithNewKeysAndSaveToFile(string plaintext, string privateKeyFilePath, string publicKeyFilePath, string encryptedFilePath);
        void RSAEncryptWithExistKeyAndSaveToFile(string plaintext, string publicKeyFilePath, string encryptedFilePath);
        string RSADecryptFromFile(string encryptedFilePath, string privateKeyFilePath);
    }
}
