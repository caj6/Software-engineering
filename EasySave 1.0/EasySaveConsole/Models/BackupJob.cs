namespace BackupApp.Models
{
    public class BackupJob
    {
        public string Name { get; set; }

        public int Id { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string Mode { get; set; }
        public string LogFormat { get; set; }
        public string Status { get; set; } 


        public BackupJob(int id, string name, string source, string destination, string mode, string logFormat)
        {
            Id = id;
            Name = name;
            SourcePath = source;
            DestinationPath = destination;
            Mode = mode.ToLower();
            LogFormat = logFormat.ToLower();
            Status = "pending";
        }

    }
}
