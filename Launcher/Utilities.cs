namespace PVSmartLauncher;

using System.Runtime.InteropServices;
using System.IO;

public static class Utilities
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool DeleteFile(string name);

    public static void RemoveMarkOfTheWeb(string filePath)
    {
        try
        {
            string zoneIdentifierPath = filePath + ":Zone.Identifier";
            DeleteFile(zoneIdentifierPath);
            Logger.Log("MOTW Removed", local: filePath);
        }
        catch (Exception ex)
        {
            Logger.Log("Failed to remove MOTW", local: filePath, error: ex.Message);
        }
    }

    public static bool IsFileLocked(string filePath)
    {
        try
        {
            using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
        }
        catch (IOException)
        {
            return true;
        }
        return false;
    }
}