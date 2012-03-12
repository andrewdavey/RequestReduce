using System.Collections.Generic;
using System.IO;

namespace Spriting
{
    public interface IFileWrapper
    {
        void Save(string content, string fileName);
        void Save(byte[] content, string fileName);
        void DeleteFile(string path);
        bool FileExists(string path);
        byte[] GetFileBytes(string path);
    }
}