using EasySave2.Models;
using EasySave2.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EasySave2.Views
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();


        }


        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.AddJob();
            }
        }
        private void ExecuteJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.ExecuteJob();
            }
        }
        private void DeleteJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
                vm.DeleteJob();
        }

        private void EditJob_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
                vm.EditJob();
        }

        private void JobGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MenuViewModel vm)
            {
                vm.SelectedJobs.Clear();
                foreach (var item in JobGrid.SelectedItems)
                {
                    if (item is BackupJob job)
                        vm.SelectedJobs.Add(job);
                }
            }
        }

        private void PauseJob_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is BackupJob job)
                job.IsPaused = true;
        }

        private void ResumeJob_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is BackupJob job)
            {
                if (job.Status == "pending")
                {
                    Task.Run(() => job.Execute()); // Start the job
                    job.Status = "running";
                }
                else
                {
                    job.IsPaused = false; // Resume if paused
                }
            }
        }


        private void StopJob_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is BackupJob job)
                job.IsStopped = true;
        }
        private void OpenGeneralSettings_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GeneralSettingsDialog();
            dialog.ShowDialog();
        }









    }
}
