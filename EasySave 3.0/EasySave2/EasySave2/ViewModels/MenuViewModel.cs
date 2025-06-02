using System.Collections.ObjectModel;
using EasySave2.Models;
using EasySave2.Views;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using EasySave2.Services;
using MessageBox = System.Windows.MessageBox;

namespace EasySave2.ViewModels
{
    public class MenuViewModel
    {
        public ObservableCollection<BackupJob> BackupJobs { get; set; } = new();
        public ObservableCollection<BackupJob> SelectedJobs { get; set; } = new();

        public MenuViewModel()
        {
            var loadedJobs = JobStorageService.LoadJobs();
            foreach (var job in loadedJobs)
                BackupJobs.Add(job);
        }


        public void AddJob()
        {
            var dialog = new AddJobDialog();
            bool? result = dialog.ShowDialog();

            if (result == true && dialog.Result != null)
            {
                dialog.Result.Id = BackupJobs.Count + 1;
                BackupJobs.Add(dialog.Result);
                JobStorageService.SaveJobs(BackupJobs);

            }
        }

        public void ExecuteJob()
        {
            if (SelectedJobs == null || SelectedJobs.Count == 0)
            {
                MessageBox.Show("Please select one or more jobs.");
                return;
            }

            foreach (var job in SelectedJobs.ToList())
            {
                if (!string.IsNullOrWhiteSpace(job.SoftwarePackage))
                {
                    string softwareName = Path.GetFileNameWithoutExtension(job.SoftwarePackage)?.ToLower();
                    var running = Process.GetProcessesByName(softwareName);

                    if (running.Length > 0)
                    {
                        MessageBox.Show(
                            $"Blocked software detected: '{job.SoftwarePackage}'.\n" +
                            $"Job '{job.Name}' will not start until it's closed.",
                            "Blocked Execution",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    }
                }
                Task.Run(() => job.Execute());
                job.Status = "running";
            }
        }

        public void DeleteJob()
        {
            if (SelectedJobs != null && SelectedJobs.Count > 0)
            {
                foreach (var job in SelectedJobs.ToList())
                    BackupJobs.Remove(job);
                JobStorageService.SaveJobs(BackupJobs);

            }
        }

        public void EditJob()
        {
            var job = SelectedJobs.FirstOrDefault();
            if (job == null)
            {
                MessageBox.Show("Select a job to edit.");
                return;
            }

            var dialog = new AddJobDialog(job);
            bool? result = dialog.ShowDialog();

            if (result == true && dialog.Result != null)
            {
                job.Name = dialog.Result.Name;
                job.SourcePath = dialog.Result.SourcePath;
                job.DestinationPath = dialog.Result.DestinationPath;
                job.Mode = dialog.Result.Mode;
                job.UseEncryption = dialog.Result.UseEncryption;
                job.LogFormat = dialog.Result.LogFormat;
                job.SoftwarePackage = dialog.Result.SoftwarePackage;
            }
            JobStorageService.SaveJobs(BackupJobs);

        }
    }
}
