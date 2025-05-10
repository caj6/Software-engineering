using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackupApp.Models
{
    public class BackupManager
    {
        private List<BackupJob> _jobs = new();
        private int _nextId = 1;

        public List<BackupJob> GetAllJobs() => _jobs;

        public BackupJob? GetJobById(int id) => _jobs.FirstOrDefault(j => j.Id == id);

        public void AddJob(string name, string source, string destination, string mode, string logFormat)
        {
            _jobs.Add(new BackupJob(_nextId++, name, source, destination, mode, logFormat));
        }


        public bool DeleteJob(int id)
        {
            var job = GetJobById(id);
            return job != null && _jobs.Remove(job);
        }

        public bool EditJobMode(int id, string newMode)
        {
            var job = GetJobById(id);
            if (job == null) return false;
            job.Mode = newMode.ToLower();
            return true;
        }

        public bool ExecuteJob(int id)
        {
            var job = GetJobById(id);
            if (job == null) return false;

            // Create log directories
            string logRoot = Path.Combine(job.DestinationPath, "logs");
            string dailyFolder = Path.Combine(logRoot, "daily");
            string statusFolder = Path.Combine(logRoot, "status");

            Directory.CreateDirectory(dailyFolder);
            Directory.CreateDirectory(statusFolder);

            // Define log file paths
            string date = DateTime.Now.ToString("yyyyMMdd");
            string dailyLogPath = Path.Combine(dailyFolder, $"{job.Id}_{date}.json");
            string statusLogPath = Path.Combine(statusFolder, $"{job.Id}_{date}.json");

            var stopwatch = new System.Diagnostics.Stopwatch();
            int totalFiles = 0;
            long totalSize = 0;
            double totalTime = 0;

            foreach (var filePath in Directory.GetFiles(job.SourcePath, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(job.SourcePath, filePath);
                string destFile = Path.Combine(job.DestinationPath, relativePath);

                bool shouldCopy = job.Mode == "full" || !File.Exists(destFile);
                if (!shouldCopy) continue;

                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);

                stopwatch.Restart();
                File.Copy(filePath, destFile, true);
                stopwatch.Stop();

                var fileInfo = new FileInfo(filePath);
                long size = fileInfo.Length;
                double timeTaken = stopwatch.Elapsed.TotalMilliseconds;

                totalFiles++;
                totalSize += size;
                totalTime += timeTaken;

                var log = new LogEntry(
                    name: job.Name,
                    source: filePath,
                    target: destFile,
                    destPath: job.DestinationPath,
                    size: size,
                    transferTime: timeTaken
                );

                string json = System.Text.Json.JsonSerializer.Serialize(log);
                File.AppendAllText(dailyLogPath, json + "," + Environment.NewLine);
            }

            // Write job infos to status log
            var status = new
            {
                Name = job.Name,
                TotalFiles = totalFiles,
                TotalSize = totalSize,
                TotalTime = Math.Round(totalTime, 3),
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };

            string statusJson = System.Text.Json.JsonSerializer.Serialize(status);
            File.AppendAllText(statusLogPath, statusJson + Environment.NewLine);

            // Update job state
            job.Status = "executed";

            return true;
        }

    }
}
