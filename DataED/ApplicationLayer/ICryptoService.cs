namespace DataED.ApplicationLayer {
    public interface ICryptoService {
        void AESEncryptAndSaveToFile(string plaintext, string toStoreKeyFilePath, string encryptedFilePath);
        string AESDecryptFromFile(string encryptedFilePath, string keyFilePath);
        void RSAEncryptWithNewKeysAndSaveToFile(string plaintext, string toStorePrivateKeyFilePath, string toStorePublicKeyFilePath, string encryptedFilePath);
        void RSAEncryptWithExistKeyAndSaveToFile(string plaintext, string publicKeyFilePath, string encryptedFilePath);
        string RSADecryptFromFile(string encryptedFilePath, string privateKeyFilePath);
    }
}
