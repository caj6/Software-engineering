using EasySave2.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Xml.Linq;

namespace EasySave2.Models
{
    public class BackupJob : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _sourcePath;
        public string SourcePath
        {
            get => _sourcePath;
            set { _sourcePath = value; OnPropertyChanged(nameof(SourcePath)); }
        }

        private string _destinationPath;
        public string DestinationPath
        {
            get => _destinationPath;
            set { _destinationPath = value; OnPropertyChanged(nameof(DestinationPath)); }
        }

        private string _mode;
        public string Mode
        {
            get => _mode;
            set { _mode = value; OnPropertyChanged(nameof(Mode)); }
        }

        private string _status = "pending";
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        private bool _useEncryption;
        public bool UseEncryption
        {
            get => _useEncryption;
            set { _useEncryption = value; OnPropertyChanged(nameof(UseEncryption)); }
        }

        private string _logFormat = "json";
        public string LogFormat
        {
            get => _logFormat;
            set
            {
                if (_logFormat != value)
                {
                    _logFormat = value;
                    OnPropertyChanged(nameof(LogFormat));
                }
            }
        }

        private string? _extensionsToEncrypt;
        public string? ExtensionsToEncrypt
        {
            get => _extensionsToEncrypt;
            set { _extensionsToEncrypt = value; OnPropertyChanged(nameof(ExtensionsToEncrypt)); }
        }

        public string? SoftwarePackage { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Execute()
        {
            if (!Directory.Exists(SourcePath))
            {
                MessageBox.Show($"Source folder not found: {SourcePath}");
                return;
            }

            string[] files = Directory.GetFiles(SourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length;
            long totalSize = files.Sum(f => new FileInfo(f).Length);
            int filesLeft = totalFiles;
            long remainingSize = totalSize;

            string logRoot = Path.Combine(DestinationPath, "logs");
            string dailyFolder = Path.Combine(logRoot, "daily");
            string statusFolder = Path.Combine(logRoot, "status");

            Directory.CreateDirectory(dailyFolder);
            Directory.CreateDirectory(statusFolder);

            string date = DateTime.Now.ToString("yyyyMMdd");
            string ext = LogFormat == "xml" ? "xml" : "json";
            string dailyLogPath = Path.Combine(dailyFolder, $"{Id}_{date}.{ext}");
            string statusLogPath = Path.Combine(statusFolder, $"{Id}_{date}.{ext}");

            var stopwatch = new Stopwatch();
            double totalTime = 0;

            string[] extensions = (ExtensionsToEncrypt ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                             .Select(e => e.Trim().ToLower())
                                                             .ToArray();

            foreach (var file in files)
            {
                string relativePath = Path.GetRelativePath(SourcePath, file);
                string destFile = Path.Combine(DestinationPath, relativePath);
                long fileSize = new FileInfo(file).Length;

                bool shouldCopy = Mode == "full" || !File.Exists(destFile);
                if (!shouldCopy)
                {
                    filesLeft--;
                    remainingSize -= fileSize;
                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);

                double transferTime;
                stopwatch.Restart();

                try
                {
                    string fileExt = Path.GetExtension(file).ToLower();

                    bool shouldEncrypt = UseEncryption &&
                                         (extensions.Contains("*") || extensions.Contains(fileExt));

                    if (shouldEncrypt)
                    {
                        bool success = CryptoService.Encrypt(file, destFile, out transferTime);
                        if (!success)
                        {
                            transferTime = -1;
                        }
                    }
                    else
                    {
                        File.Copy(file, destFile, true);
                        stopwatch.Stop();
                        transferTime = stopwatch.Elapsed.TotalMilliseconds;
                    }
                }
                catch
                {
                    stopwatch.Stop();
                    transferTime = -1;
                }

                totalTime += transferTime > 0 ? transferTime : 0;

                // Daily log
                if (LogFormat == "xml")
                {
                    var xmlEntry = new XElement("LogEntry",
                        new XElement("Name", Name),
                        new XElement("FileSource", file),
                        new XElement("FileTarget", destFile),
                        new XElement("DestPath", DestinationPath),
                        new XElement("FileSize", fileSize),
                        new XElement("FileTransferTime", transferTime),
                        new XElement("Time", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                    );
                    File.AppendAllText(dailyLogPath, xmlEntry + Environment.NewLine);
                }
                else
                {
                    var dailyEntry = new
                    {
                        Name,
                        FileSource = file,
                        FileTarget = destFile,
                        DestPath = DestinationPath,
                        FileSize = fileSize,
                        FileTransferTime = transferTime,
                        time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    };
                    File.AppendAllText(dailyLogPath, JsonSerializer.Serialize(dailyEntry) + Environment.NewLine);
                }

                // Status log
                double progression = 100.0 * (totalFiles - filesLeft + 1) / totalFiles;
                if (LogFormat == "xml")
                {
                    var xmlStatus = new XElement("Status",
                        new XElement("Name", Name),
                        new XElement("State", "ACTIVE"),
                        new XElement("TotalFilesToCopy", totalFiles),
                        new XElement("TotalFilesSize", totalSize),
                        new XElement("NbFilesLeftToDo", filesLeft - 1),
                        new XElement("RemainingFilesSize", remainingSize - fileSize),
                        new XElement("SourceFilePath", file),
                        new XElement("TargetFilePath", destFile),
                        new XElement("Progression", Math.Round(progression, 2))
                    );
                    File.WriteAllText(statusLogPath, xmlStatus + Environment.NewLine);
                }
                else
                {
                    var status = new
                    {
                        Name,
                        State = "ACTIVE",
                        TotalFilesToCopy = totalFiles,
                        TotalFilesSize = totalSize,
                        NbFilesLeftToDo = filesLeft - 1,
                        RemainingFilesSize = remainingSize - fileSize,
                        SourceFilePath = file,
                        TargetFilePath = destFile,
                        Progression = Math.Round(progression, 2)
                    };
                    File.WriteAllText(statusLogPath, JsonSerializer.Serialize(status));
                }

                filesLeft--;
                remainingSize -= fileSize;
            }

            // Final status log
            if (LogFormat == "xml")
            {
                var finalXml = new XElement("Status",
                    new XElement("Name", Name),
                    new XElement("State", "END"),
                    new XElement("TotalFilesToCopy", totalFiles),
                    new XElement("TotalFilesSize", totalSize),
                    new XElement("NbFilesLeftToDo", 0),
                    new XElement("RemainingFilesSize", 0),
                    new XElement("SourceFilePath", ""),
                    new XElement("TargetFilePath", ""),
                    new XElement("Progression", 100.0)
                );
                File.WriteAllText(statusLogPath, finalXml + Environment.NewLine);
            }
            else
            {
                var finalStatus = new
                {
                    Name,
                    State = "END",
                    TotalFilesToCopy = totalFiles,
                    TotalFilesSize = totalSize,
                    NbFilesLeftToDo = 0,
                    RemainingFilesSize = 0,
                    SourceFilePath = "",
                    TargetFilePath = "",
                    Progression = 100.0
                };
                File.WriteAllText(statusLogPath, JsonSerializer.Serialize(finalStatus));
            }
            Status = "executed";
        }
    }
}
