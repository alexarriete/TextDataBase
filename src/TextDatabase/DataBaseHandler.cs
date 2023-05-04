using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDatabase
{
    public class DataBaseHandler
    {
        public static string CreateDataBase(string? appName)
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(localPath, appName, "TextData"));
            if (!dir.Exists)
                dir.Create();

            return dir.FullName;
        }

        public static void DropDataBase(string? appName)
        {
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(localPath, appName));
            if (dir.Exists)
                dir.Delete(true);
        }

        public static void CreateBackup(string appName, string path, bool addDate)
        {
            string dbName = addDate ? $"TextData{DateTime.Today.ToShortDateString().Replace("/", "-")}" : "TextData";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
                dir.Create();
            
            dir = new DirectoryInfo(Path.Combine(path, appName));
            if (!dir.Exists)
                dir.Create();

            string zippath = Path.Combine(dir.FullName, $"{dbName}.zip" );

            string sourceDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName, "TextData");
            ZipFile.CreateFromDirectory(sourceDir, zippath, CompressionLevel.Optimal, false);            
        }

        public static void RestoreBackup(string appName, string zippath)
        {
            string targetDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName, "TextData");
            if (zippath.EndsWith(".zip"))
                ZipFile.ExtractToDirectory(zippath, targetDir, true);
            else
            {
                DirectoryInfo dir = new DirectoryInfo(zippath);
                if (!dir.Exists)
                    throw new Exception($"Directory not found {dir.FullName}");

                FileInfo? filezip = dir.GetFiles()?.OrderByDescending(n => n.LastWriteTime).FirstOrDefault(x => x.Extension == ".zip");
                if (filezip != null)
                    ZipFile.ExtractToDirectory(filezip.FullName, targetDir, true);
                else
                    throw new Exception($"No .zip file found on {dir.FullName}");
            }
        }
    }
}
