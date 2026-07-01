namespace PVSmartLauncher;

using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ServerSync
{
    private DateTime _serverFileOriginalTime;

    public string CopyToServerCache(string serverPath)
    {
        if (!File.Exists(serverPath))
        {
            throw new FileNotFoundException("Server file is missing or inaccessible.");
        }

        Directory.CreateDirectory(Settings.Current.WorkingFolder);
        string fileName = Path.GetFileName(serverPath);
        string localPath = Path.Combine(Settings.Current.WorkingFolder, fileName);

        _serverFileOriginalTime = File.GetLastWriteTimeUtc(serverPath);

        File.Copy(serverPath, localPath, true);
        Utilities.RemoveMarkOfTheWeb(localPath);
        
        Logger.Log("Copied to local cache", original: serverPath, local: localPath);
        return localPath;
    }

    public async Task SyncBackWithRecoveryAsync(string serverPath, string localPath, Action<string> updateStatusCallback)
    {
        if (!Settings.Current.AutoSync) return;

        bool synchronized = false;

        while (!synchronized)
        {
            try
            {
                updateStatusCallback("Checking server state...");
                
                // Smart Conflict Detection
                if (File.Exists(serverPath))
                {
                    DateTime currentServerTime = File.GetLastWriteTimeUtc(serverPath);
                    if (currentServerTime > _serverFileOriginalTime && !Settings.Current.AutoOverwrite)
                    {
                        updateStatusCallback("Conflict detected! Waiting for user input.");
                        DialogResult result = MessageBox.Show(
                            "The server file has been modified by someone else while you were editing.\n\nDo you want to overwrite the server file?",
                            "Conflict Detected",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            string backupPath = Path.Combine(Path.GetDirectoryName(serverPath)!, Path.GetFileNameWithoutExtension(serverPath) + "_Recovered" + Path.GetExtension(serverPath));
                            File.Copy(localPath, backupPath, false);
                            Logger.Log("Saved as copy due to conflict", local: localPath, original: backupPath);
                            break;
                        }
                    }
                }

                updateStatusCallback("Syncing to server...");
                File.Copy(localPath, serverPath, true);
                Logger.Log("Successfully synced to server", original: serverPath, local: localPath);
                synchronized = true;

                if (Settings.Current.DeleteTempAfterSync)
                {
                    File.Delete(localPath);
                    Logger.Log("Temporary file deleted.", local: localPath);
                }
            }
            catch (IOException ioEx)
            {
                updateStatusCallback($"Network/Lock error. Retrying in {Settings.Current.RetryIntervalSeconds}s...");
                Logger.Log("Sync failed. Retrying.", original: serverPath, error: ioEx.Message);
                await Task.Delay(Settings.Current.RetryIntervalSeconds * 1000);
            }
            catch (Exception ex)
            {
                Logger.Log("Critical Sync Error", original: serverPath, error: ex.Message);
                MessageBox.Show($"Critical error during sync: {ex.Message}");
                break;
            }
        }
    }
}