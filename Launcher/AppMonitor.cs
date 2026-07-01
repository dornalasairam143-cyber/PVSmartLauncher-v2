namespace PVSmartLauncher;

using System.Diagnostics;
using System.Threading.Tasks;

public static class AppMonitor
{
    public static async Task OpenAndWaitAsync(string localPath)
    {
        Logger.Log("Opening application", local: localPath);
        
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = localPath,
                UseShellExecute = true
            };
            
            Process? p = Process.Start(psi);

            // Office apps (Excel/Word) often route to an existing instance and close immediately.
            // Therefore, we cannot just rely on WaitForExitAsync(). We must check file locks.
            
            await Task.Delay(3000); // Give application time to lock the file
            
            Logger.Log("Monitoring file lock...", local: localPath);
            
            while (true)
            {
                if (!Utilities.IsFileLocked(localPath))
                {
                    // Confirm it's truly unlocked (debouncing close actions)
                    await Task.Delay(2000);
                    if (!Utilities.IsFileLocked(localPath))
                    {
                        break;
                    }
                }
                await Task.Delay(2000);
            }
            
            Logger.Log("Application closed, file released.", local: localPath);
        }
        catch (Exception ex)
        {
            Logger.Log("Application Crash/Launch Error", local: localPath, error: ex.Message);
            throw;
        }
    }
}