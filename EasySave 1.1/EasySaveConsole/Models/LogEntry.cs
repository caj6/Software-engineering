using System;
using System.Xml.Serialization;

namespace BackupApp.Models
{
    [Serializable]
    public class LogEntry
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string FileSource { get; set; }

        [XmlElement]
        public string FileTarget { get; set; }

        [XmlElement]
        public string DestPath { get; set; }

        [XmlElement]
        public long FileSize { get; set; }

        [XmlElement]
        public double FileTransferTime { get; set; }

        [XmlElement]
        public string Time { get; set; }

        public LogEntry() { }

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
