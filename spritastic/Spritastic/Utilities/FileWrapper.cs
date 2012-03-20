using System.IO;

namespace Spritastic.Utilities
{
    public class FileWrapper : IFileWrapper
    {
        public void Save(string content, string fileName)
        {
            File.WriteAllText(fileName, content);
        }

        public void Save(byte[] content, string fileName)
        {
            File.WriteAllBytes(fileName, content);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public byte[] GetFileBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}