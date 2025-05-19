using System;
using System.Xml.Serialization;

namespace BackupApp.Models
{
    [Serializable]
    public class StatusLog
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public int TotalFiles { get; set; }

        [XmlElement]
        public long TotalSize { get; set; }

        [XmlElement]
        public double TotalTime { get; set; }

        [XmlElement]
        public string Time { get; set; }

        public StatusLog() { }

        public StatusLog(string name, int totalFiles, long totalSize, double totalTime, string time)
        {
            Name = name;
            TotalFiles = totalFiles;
            TotalSize = totalSize;
            TotalTime = totalTime;
            Time = time;
        }
    }
}
