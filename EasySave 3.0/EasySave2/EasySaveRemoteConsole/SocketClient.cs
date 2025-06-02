using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySaveRemoteConsole
{
    public class SocketClient
    {
        private TcpClient? _client;
        private NetworkStream? _stream;

        public event Action<string>? OnJobListReceived;

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            _stream = _client.GetStream();

            
            _ = Task.Run(ReadFromServer);
        }

        private async Task ReadFromServer()
        {
            using var reader = new StreamReader(_stream!, Encoding.UTF8);
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line != null)
                    OnJobListReceived?.Invoke(line);
            }
        }

        public async Task SendCommandAsync(string action, int jobId)
        {
            if (_stream == null)
            {
                return;
            }

            var cmd = new { action, jobId };
            string json = JsonSerializer.Serialize(cmd);
            

            byte[] data = Encoding.UTF8.GetBytes(json + "\n");
            await _stream.WriteAsync(data, 0, data.Length);
        }




    }
}
