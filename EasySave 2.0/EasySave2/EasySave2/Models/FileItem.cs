using System.IO;

namespace EasySave2.Models
{
    public class FileItem : Item
    {
        public string SourcePath { get; }
        public string DestinationPath { get; }

        public FileItem(string sourcePath, string destinationPath)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            Name = Path.GetFileName(sourcePath);
            Type = Path.GetExtension(sourcePath);
            Size = new FileInfo(sourcePath).Length;
        }

        public override void Get()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DestinationPath)!);
            File.Copy(SourcePath, DestinationPath, true);
        }
    }
}
