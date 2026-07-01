namespace PVSmartLauncher.Models;

public class SettingsModel
{
    public string WorkingFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PVSmartLauncher", "WorkingFiles");
    public string LogFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PVSmartLauncher", "Logs");
    public int RetryIntervalSeconds { get; set; } = 30;
    public bool DeleteTempAfterSync { get; set; } = true;
    public bool DarkMode { get; set; } = true;
    public bool AutoSync { get; set; } = true;
    public bool AutoOverwrite { get; set; } = false;
}

public class LogEntry
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string OriginalPath { get; set; } = string.Empty;
    public string LocalPath { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ErrorDetails { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Status} | Src: {OriginalPath} | Lcl: {LocalPath} | Err: {ErrorDetails}";
    }
}