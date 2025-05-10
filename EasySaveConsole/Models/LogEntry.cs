using System;

namespace BackupApp.Models
{
    public class LogEntry
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string DestPath { get; set; }
        public long FileSize { get; set; }
        public double FileTransferTime { get; set; }
        public string Time { get; set; }

        public LogEntry(string name, string source, string target, string destPath, long size, double transferTime)
        {
            Name = name;
            FileSource = source;
            FileTarget = target;
            DestPath = destPath;
            FileSize = size;
            FileTransferTime = transferTime;
            Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
