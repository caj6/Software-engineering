using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using EasySaveRemoteConsole.Models;

namespace EasySaveRemoteConsole
{
    public partial class RemoteConsole : Window
    {
        private readonly SocketClient _client = new();
        private readonly ObservableCollection<BackupJob> _jobs = new();

        public RemoteConsole()
        {
            InitializeComponent();
            JobGrid.ItemsSource = _jobs;

            _client.OnJobListReceived += UpdateJobList;
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            string ip = IpAddressBox.Text;
            int port = int.Parse(PortBox.Text);
            await _client.ConnectAsync(ip, port);
        }

        private void UpdateJobList(string json)
        {
            var jobs = JsonSerializer.Deserialize<BackupJob[]>(json);
            if (jobs == null) return;

            Dispatcher.Invoke(() =>
            {
                _jobs.Clear();
                foreach (var job in jobs)
                    _jobs.Add(job);
            });
        }

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is BackupJob job)
            {
                await _client.SendCommandAsync("play", job.Id);
            }
        }


        private async void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is BackupJob job)
            {
                Console.WriteLine($"|| Pause clicked for job {job.Id}");
                await _client.SendCommandAsync("pause", job.Id);
            }
                
        }

        private async void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is BackupJob job)
            {
                Console.WriteLine($" Stop clicked for job {job.Id}");
                await _client.SendCommandAsync("stop", job.Id);
            }
                
        }



    }
}
