namespace PVSmartLauncher;

using System.IO;
using System.Text.Json;
using PVSmartLauncher.Models;

public static class Settings
{
    private static readonly string SettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PVSmartLauncher", "settings.json");
    public static SettingsModel Current { get; private set; } = new SettingsModel();

    public static void Load()
    {
        try
        {
            if (File.Exists(SettingsFile))
            {
                string json = File.ReadAllText(SettingsFile);
                var loaded = JsonSerializer.Deserialize<SettingsModel>(json);
                if (loaded != null) Current = loaded;
            }
            else
            {
                Save(); // Create default
            }
        }
        catch (Exception ex)
        {
            Logger.Log("Settings Load Failed", error: ex.Message);
        }
    }

    public static void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile)!);
            string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
        catch (Exception ex)
        {
            Logger.Log("Settings Save Failed", error: ex.Message);
        }
    }
}