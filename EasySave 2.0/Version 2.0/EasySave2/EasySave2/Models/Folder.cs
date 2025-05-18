using System.Collections.Generic;
using System.IO;

namespace EasySave2.Models
{
    public class Folder
    {
        public string Path { get; }
        public List<FileItem> Files { get; private set; } = new();
        public int NbFiles => Files.Count;

        public Folder(string source, string destination)
        {
            Path = source;
            LoadFiles(source, destination);
        }

        private void LoadFiles(string source, string destinationRoot)
        {
            Files.Clear();
            var allFiles = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
            foreach (var file in allFiles)
            {
                var relativePath = System.IO.Path.GetRelativePath(source, file);
                var destination = System.IO.Path.Combine(destinationRoot, relativePath);
                Files.Add(new FileItem(file, destination));
            }
        }
    }
}
