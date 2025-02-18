namespace DataED.ApplicationLayer {
    public class FileService : IFileService {
        public bool Exists(string? path) {
            return File.Exists(path);
        }

        public void WriteAllBytes(string path, byte[] bytes) {
            File.WriteAllBytes(path, bytes);
        }

        public byte[] ReadAllBytes(string path) {
            return File.ReadAllBytes(path);
        }
    }
}
