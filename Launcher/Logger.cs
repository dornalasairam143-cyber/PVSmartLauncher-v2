namespace PVSmartLauncher;

using System.IO;
using PVSmartLauncher.Models;

public static class Logger
{
    private static string _logDirectory = string.Empty;
    public static event Action<string>? OnLogAdded;

    public static void Initialize(string logDir)
    {
        _logDirectory = logDir;
        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }
    }

    public static void Log(string status, string original = "", string local = "", string error = "")
    {
        var entry = new LogEntry
        {
            OriginalPath = original,
            LocalPath = local,
            Status = status,
            ErrorDetails = error
        };

        string logMessage = entry.ToString();
        OnLogAdded?.Invoke(logMessage);

        try
        {
            string file = Path.Combine(_logDirectory, $"Log_{DateTime.Now:yyyyMMdd}.txt");
            File.AppendAllText(file, logMessage + Environment.NewLine);
        }
        catch
        {
            // Failsafe: if we can't write to disk, we rely on the UI log window.
        }
    }
}