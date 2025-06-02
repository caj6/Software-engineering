using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EasySave2.Models;

namespace EasySave2.Services
{
    public static class JobStorageService
    {
        private const string SaveFile = "backupJobs.json";

        public static List<BackupJob> LoadJobs()
        {
            if (!File.Exists(SaveFile))
                return new List<BackupJob>();

            try
            {
                var json = File.ReadAllText(SaveFile);
                var jobs = JsonSerializer.Deserialize<List<BackupJob>>(json);
                foreach (var job in jobs ?? new List<BackupJob>())
                {
                    job.Status = "pending";
                    job.IsPaused = false;
                    job.IsStopped = false;
                    job.Progress = 0;
                }
                return jobs ?? new List<BackupJob>();
            }
            catch
            {
                return new List<BackupJob>();
            }
        }

        public static void SaveJobs(IEnumerable<BackupJob> jobs)
        {
            var json = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SaveFile, json);
        }
    }
}
