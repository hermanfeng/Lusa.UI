using System.IO;
using System;

namespace CommonLibrary
{
    public class FileService
    {
        static Lazy<FileService> lazyer = new Lazy<FileService>(() => new FileService(),true);
        private FileService()
        {
            Directory.CreateDirectory(DataPath);
            Directory.CreateDirectory(TempPath);
        }

        public static FileService Instance 
        {
            get
            {
                return lazyer.Value;
            }
        }

        public string DataPath
        {
            get { return Environment.CurrentDirectory + @"\Data"; }
        }

        public string TempPath
        {
            get { return Environment.CurrentDirectory + @"\Data\Temp"; }
        }

        public bool IsExistFile(string relativeFilePath)
        {
            return File.Exists(GetAbsolutePath(relativeFilePath));
        }

        public string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(DataPath, relativePath);
        }

        public string EnsureAbsolutePath(string relativePath)
        {
            var path = Path.Combine(DataPath, relativePath);
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return path;
        }

        public string EnsureAbsoluteDirectory(string relativePath)
        {
            var path = Path.Combine(DataPath, relativePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public void WriteContent(string relativePath, string content)
        {
            var path = EnsureAbsolutePath(relativePath);
            File.WriteAllText(path, content);
        }

        public void AppendContent(string relativePath, string content)
        {
            var path = EnsureAbsolutePath(relativePath);
            File.AppendAllText(path, content + Environment.NewLine);
        }

        public void WriteContent(string relativePath, byte[] bytes)
        {
            var path = EnsureAbsolutePath(relativePath);
            File.WriteAllBytes(path, bytes);
        }


        public string ReadContent(string relativePath)
        {
            if (IsExistFile(relativePath))
            {
                return File.ReadAllText(GetAbsolutePath(relativePath));
            }
            return string.Empty;
        }
    }
}
