namespace Spritastic.Utilities
{
    public interface IFileWrapper
    {
        void Save(string content, string fileName);
        void Save(byte[] content, string fileName);
        bool FileExists(string path);
        byte[] GetFileBytes(string path);
    }
}