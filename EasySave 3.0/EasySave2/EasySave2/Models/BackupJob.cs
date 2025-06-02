using EasySave2.Cryptosoft;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Xml.Linq;
using System.Threading.Tasks;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;


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
        public string? PriorityExtensions { get; set; }

        private static readonly object LargeFileLock = new();
        private static bool IsLargeFileBeingTransferred = false;

        private static readonly object PriorityLock = new();
        private static readonly HashSet<string> GlobalPriorityFiles = new();


        // Added Actions 

        private bool _isPaused;
        public bool IsPaused
        {
            get => _isPaused;
            set { _isPaused = value; OnPropertyChanged(nameof(IsPaused)); }
        }

        private bool _isStopped;
        public bool IsStopped
        {
            get => _isStopped;
            set { _isStopped = value; OnPropertyChanged(nameof(IsStopped)); }
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(nameof(Progress)); }
        }




        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Helper function 

        private bool IsBlockingSoftwareRunning()
        {
            if (string.IsNullOrWhiteSpace(SoftwarePackage))
                return false;

            try
            {
                string processName = Path.GetFileNameWithoutExtension(SoftwarePackage).ToLower();
                return Process.GetProcessesByName(processName).Any();
            }
            catch
            {
                return false;
            }
        }
        private bool _alertShownForSoftware = false;



        public async void Execute()
        {
            Application.Current.Dispatcher.Invoke(() => Status = "running");
            Application.Current.Dispatcher.Invoke(() => Console.WriteLine($"JOB {Id} IS EXECUTING..."));

            if (!Directory.Exists(SourcePath))
            {
                Application.Current.Dispatcher.Invoke(() => Status = "failed");
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

            string[] encryptionExtensions = (ExtensionsToEncrypt ?? "*")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim().ToLower())
                .ToArray();

            string[] priorityExtensions = (Settings.GlobalPriorityExtensions ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim().ToLower())
                .ToArray();

            // Register global priority files before starting
            lock (PriorityLock)
            {
                foreach (var file in files)
                {
                    string extFile = Path.GetExtension(file).ToLower();
                    if (priorityExtensions.Contains(extFile))
                        GlobalPriorityFiles.Add(file);
                }
            }

            foreach (var file in files)
            {
                if (IsStopped)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => Status = "stopped");
                    return;
                }

                // Pause if user toggled or software is detected
                while (IsPaused || IsBlockingSoftwareRunning())
                {
                    if (IsBlockingSoftwareRunning())
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            Status = "paused (software detected)";
                            if (!_alertShownForSoftware)
                            {
                                MessageBox.Show(
                                    $"The application '{SoftwarePackage}' is currently running.\n" +
                                    $"The backup job '{Name}' is paused.\n\n" +
                                    $"Please close it to allow the job to continue.",
                                    "Blocking Software Detected",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning
                                );
                                _alertShownForSoftware = true;
                            }
                        });
                    }

                    if (IsStopped)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() => Status = "stopped");
                        return;
                    }

                    await Task.Delay(1000);
                }

                // Reset alert flag
                if (_alertShownForSoftware && !IsBlockingSoftwareRunning())
                {
                    _alertShownForSoftware = false;
                    System.Windows.Application.Current.Dispatcher.Invoke(() => Status = "running");
                }

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

                string fileExt = Path.GetExtension(file).ToLower();
                bool isPriorityFile = priorityExtensions.Contains(fileExt);
                bool shouldEncrypt = UseEncryption && (encryptionExtensions.Contains("*") || encryptionExtensions.Contains(fileExt));

                // Wait if it's a non-priority file but there are still priority files to do
                while (!isPriorityFile && GlobalPriorityFiles.Count > 0)
                {
                    await Task.Delay(200);
                }

                // Large file lock
                if (fileSize > Settings.LargeFileThresholdKB * 1024)
                {
                    while (true)
                    {
                        lock (LargeFileLock)
                        {
                            if (!IsLargeFileBeingTransferred)
                            {
                                IsLargeFileBeingTransferred = true;
                                break;
                            }
                        }
                        await Task.Delay(200);
                    }
                }

                double transferTime;
                stopwatch.Restart();

                try
                {
                    if (shouldEncrypt)
                    {
                        bool success = CryptoService.Encrypt(file, destFile, out transferTime);
                        if (!success) transferTime = -1;
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

                // After copying a priority file, remove it from the global list
                if (isPriorityFile)
                {
                    lock (PriorityLock)
                    {
                        GlobalPriorityFiles.Remove(file);
                    }
                }

                // Daily log writing
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
                        Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    };
                    File.AppendAllText(dailyLogPath, JsonSerializer.Serialize(dailyEntry) + Environment.NewLine);
                }

                // Release large file lock
                if (fileSize > Settings.LargeFileThresholdKB * 1024)
                {
                    lock (LargeFileLock)
                    {
                        IsLargeFileBeingTransferred = false;
                    }
                }

                double progression = 100.0 * (totalFiles - filesLeft + 1) / totalFiles;
                System.Windows.Application.Current.Dispatcher.Invoke(() => Progress = Math.Round(progression, 2));

                // Status log writing
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

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Progress = 100.0;
                Status = "executed";
            });

            if (LogFormat == "xml")
            {
                var xmlFinal = new XElement("Status",
                    new XElement("Name", finalStatus.Name),
                    new XElement("State", finalStatus.State),
                    new XElement("TotalFilesToCopy", finalStatus.TotalFilesToCopy),
                    new XElement("TotalFilesSize", finalStatus.TotalFilesSize),
                    new XElement("NbFilesLeftToDo", finalStatus.NbFilesLeftToDo),
                    new XElement("RemainingFilesSize", finalStatus.RemainingFilesSize),
                    new XElement("SourceFilePath", finalStatus.SourceFilePath),
                    new XElement("TargetFilePath", finalStatus.TargetFilePath),
                    new XElement("Progression", finalStatus.Progression)
                );
                File.WriteAllText(statusLogPath, xmlFinal + Environment.NewLine);
            }
            else
            {
                File.WriteAllText(statusLogPath, JsonSerializer.Serialize(finalStatus));
            }
        }



    }
}
