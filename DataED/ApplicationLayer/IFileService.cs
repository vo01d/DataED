namespace DataED.ApplicationLayer {
    public interface IFileService {
        bool Exists(string? path);
        void WriteAllBytes(string path, byte[] bytes);
        byte[] ReadAllBytes(string path);
    }
}
