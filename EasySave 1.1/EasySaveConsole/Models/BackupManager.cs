using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json; 
using item.model;

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

            string logRoot = Path.Combine(job.DestinationPath, "logs");
            string dailyFolder = Path.Combine(logRoot, "daily");
            string statusFolder = Path.Combine(logRoot, "status");

            Directory.CreateDirectory(dailyFolder);
            Directory.CreateDirectory(statusFolder);

            string date = DateTime.Now.ToString("yyyyMMdd");
            string dailyLogPath = Path.Combine(dailyFolder, $"{job.Id}_{date}.json");
            string statusLogPath = Path.Combine(statusFolder, $"{job.Id}_{date}.json");

            var stopwatch = new System.Diagnostics.Stopwatch();
            int totalFiles = 0;
            long totalSize = 0;
            double totalTime = 0;

            var dailyLogEntries = new List<LogEntry>();

            foreach (var filePath in Directory.GetFiles(job.SourcePath, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(job.SourcePath, filePath);
                string destFile = Path.Combine(job.DestinationPath, relativePath);

                bool shouldCopy = job.Mode == "full" || !System.IO.File.Exists(destFile);
                if (!shouldCopy) continue;

                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);

                stopwatch.Restart();
                System.IO.File.Copy(filePath, destFile, true);
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

                dailyLogEntries.Add(log);
            }

            var status = new StatusLog(
                job.Name,
                totalFiles,
                totalSize,
                Math.Round(totalTime, 3),
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                );

            if (job.LogFormat == "xml")
            {
                string dailyXmlPath = Path.ChangeExtension(dailyLogPath, ".xml");
                string statusXmlPath = Path.ChangeExtension(statusLogPath, ".xml");

                using (var stream = new FileStream(dailyXmlPath, FileMode.Create))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<LogEntry>));
                    xmlSerializer.Serialize(stream, dailyLogEntries);
                }

                using (var stream = new FileStream(statusXmlPath, FileMode.Create))
                {
                    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(status.GetType());
                    xmlSerializer.Serialize(stream, status);
                }
            }
            else
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string dailyJsonOutput = JsonSerializer.Serialize(dailyLogEntries, options);
                System.IO.File.WriteAllText(dailyLogPath, dailyJsonOutput);

                string statusJson = JsonSerializer.Serialize(status, options);
                System.IO.File.WriteAllText(statusLogPath, statusJson + Environment.NewLine);
            }

            job.Status = "executed";

            return true;
        }
        public void ExecuteJobs(List<int> ids)
        {
            foreach (var id in ids)
            {
                bool result = ExecuteJob(id);
                Console.WriteLine(result
                    ? $"Job {id} executed successfully."
                    : $"Job {id} not found or failed.");
            }
        }


    }
}