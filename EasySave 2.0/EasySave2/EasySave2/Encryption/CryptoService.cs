using System.Diagnostics;
using System.IO;

namespace EasySave2.Services
{
    public static class CryptoService
    {
        private const string CryptoSoftPath = "CryptoSoft.exe";

        public static bool Encrypt(string sourcePath, string destinationPath, out double encryptionTime)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                string? destDir = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrWhiteSpace(destDir))
                    Directory.CreateDirectory(destDir);

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = CryptoSoftPath,
                        Arguments = $"encrypt \"{sourcePath}\" \"{destinationPath}\"",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false
                    }
                };
                process.Start();
                process.WaitForExit();

                stopwatch.Stop();
                encryptionTime = stopwatch.Elapsed.TotalMilliseconds;

                return process.ExitCode == 0 && File.Exists(destinationPath);
            }
            catch
            {
                encryptionTime = -1;
                return false;
            }
        }



    }
}
