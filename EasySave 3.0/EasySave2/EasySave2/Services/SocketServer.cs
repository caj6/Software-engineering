using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EasySave2.ViewModels;

namespace EasySave2.Services
{
    public class SocketServer
    {
        private TcpListener? _listener;
        private readonly int _port;
        private readonly MenuViewModel _viewModel;
        private readonly List<TcpClient> _clients = new();

        public SocketServer(int port, MenuViewModel viewModel)
        {
            _port = port;
            _viewModel = viewModel;
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            Console.WriteLine($"SocketServer started on port {_port}");

            Task.Run(AcceptClients);
        }

        private async Task AcceptClients()
        {
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                lock (_clients)
                    _clients.Add(client);

                Console.WriteLine("Client connected.");
                _ = HandleClient(client);
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[4096];

            while (client.Connected)
            {
                
                await SendJobs(stream);

                
                if (stream.DataAvailable)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string commandJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    ProcessCommand(commandJson);
                }

                await Task.Delay(1000);
            }

            lock (_clients)
                _clients.Remove(client);
        }

        private async Task SendJobs(NetworkStream stream)
        {
            var jobsJson = JsonSerializer.Serialize(_viewModel.BackupJobs);
            byte[] data = Encoding.UTF8.GetBytes(jobsJson + "\n");
            await stream.WriteAsync(data, 0, data.Length);
        }

        private async void ProcessCommand(string json)
        {
            

            try
            {
                var cmd = JsonSerializer.Deserialize<Command>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (cmd == null) return;

                var job = _viewModel.BackupJobs.FirstOrDefault(j => j.Id == cmd.JobId);
                if (job == null)
                {
                    return;
                }

                switch (cmd.Action.ToLower())
                {
                    case "play":
                        Task.Run(() => job.Execute());
                        break;
                    case "pause":
                        job.IsPaused = true;
                        break;
                    case "stop":
                        job.IsStopped = true;
                        break;
                    default:
                        break;
                }

                NotifyAllClientsAsync();
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"❌ JSON ERROR: {ex.Message}");
            }
        }
        
        
        
        public class Command
        {
            [JsonPropertyName("action")]
            public string Action { get; set; }
            [JsonPropertyName("jobId")]
            public int JobId { get; set; }
        }



        public void NotifyAllClientsAsync()
        {
            var jobsJson = JsonSerializer.Serialize(_viewModel.BackupJobs);
            byte[] data = Encoding.UTF8.GetBytes(jobsJson + "\n");

            lock (_clients)
            {
                foreach (var client in _clients.ToList())
                {
                    if (client.Connected)
                    {
                        try
                        {
                            var stream = client.GetStream();
                            stream.Write(data, 0, data.Length);
                        }
                        catch
                        {
                            _clients.Remove(client);
                        }
                    }
                }
            }
        }

    }
}
