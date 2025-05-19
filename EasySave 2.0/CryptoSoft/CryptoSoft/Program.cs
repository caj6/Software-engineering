using System;
using System.IO;
using System.Diagnostics;
using System.Text;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.Error.WriteLine("Usage: CryptoSoft.exe <encrypt|decrypt> <source> <destination>");
            return -1;
        }

        string mode = args[0].ToLower();
        string sourcePath = args[1];
        string destPath = args[2];

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine("Source file not found.");
            return -1;
        }

        if (mode != "encrypt" && mode != "decrypt")
        {
            Console.Error.WriteLine("Invalid mode. Use 'encrypt' or 'decrypt'.");
            return -1;
        }

        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            byte[] data = File.ReadAllBytes(sourcePath);
            byte[] key = Encoding.UTF8.GetBytes("EASYSAVE_KEY");
            byte[] output = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                output[i] = (byte)(data[i] ^ key[i % key.Length]);
            }

            File.WriteAllBytes(destPath, output);

            stopwatch.Stop();
            long operationTime = stopwatch.ElapsedMilliseconds;

            return (int)operationTime;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during {mode}: " + ex.Message);
            return -1; 
        }
    }
}