using System;
using System.IO;
using System.Text.Json;

public static class Settings
{
    private const string SettingsFilePath = "settings.json";

    public static int LargeFileThresholdKB { get; set; } = 10000;

    public static string GlobalLogFormat { get; set; } = "json";
    public static string GlobalEncryptionExtensions { get; set; } = "*";
    public static string GlobalPriorityExtensions { get; set; } = "";


    public static void Save()
    {
        try
        {
            var settings = new
            {
                GlobalLogFormat,
                GlobalEncryptionExtensions,
                LargeFileThresholdKB,
                GlobalPriorityExtensions
            };


            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving settings: " + ex.Message);
        }
    }

    public static void Load()
    {
        try
        {
            if (!File.Exists(SettingsFilePath)) return;

            var json = File.ReadAllText(SettingsFilePath);
            var loaded = JsonSerializer.Deserialize<SettingsData>(json);

            if (loaded != null)
            {
                GlobalLogFormat = loaded.GlobalLogFormat ?? "json";
                GlobalEncryptionExtensions = loaded.GlobalEncryptionExtensions ?? "*";
                LargeFileThresholdKB = loaded.LargeFileThresholdKB ?? 10000;
                GlobalPriorityExtensions = loaded.GlobalPriorityExtensions ?? "";
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading settings: " + ex.Message);
        }
    }

    private class SettingsData
    {
        public string? GlobalLogFormat { get; set; }
        public string? GlobalEncryptionExtensions { get; set; }
        public int? LargeFileThresholdKB { get; set; }
        public string? GlobalPriorityExtensions { get; set; }
    }

}
