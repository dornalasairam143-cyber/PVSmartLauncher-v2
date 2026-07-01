namespace PVSmartLauncher;

using System.Windows.Forms;

public partial class MainForm : Form
{
    private string _targetFilePath = string.Empty;

    public MainForm(string targetFilePath)
    {
        InitializeComponent();
        Settings.Load();
        Logger.Initialize(Settings.Current.LogFolder);
        Logger.OnLogAdded += AppendLog;
        
        _targetFilePath = targetFilePath;
        
        if (Settings.Current.DarkMode) ApplyDarkTheme();

        this.Shown += MainForm_Shown;
    }

    private void ApplyDarkTheme()
    {
        this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        lblCurrentFile.ForeColor = System.Drawing.Color.WhiteSmoke;
        lblStatus.ForeColor = System.Drawing.Color.LightGray;
    }

    private void AppendLog(string message)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(AppendLog), message);
            return;
        }
        lstLog.Items.Add(message);
        lstLog.TopIndex = lstLog.Items.Count - 1;
    }

    private void UpdateStatus(string status, bool isMarquee = true)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string, bool>(UpdateStatus), status, isMarquee);
            return;
        }
        lblStatus.Text = $"Status: {status}";
        progressBar.Style = isMarquee ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
        if (!isMarquee) progressBar.Value = 100;
    }

    private async void MainForm_Shown(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_targetFilePath))
        {
            UpdateStatus("Ready. Awaiting files.", false);
            return;
        }

        lblCurrentFile.Text = Path.GetFileName(_targetFilePath);
        
        try
        {
            ServerSync syncManager = new ServerSync();
            
            UpdateStatus("Copying to local cache...");
            string localPath = syncManager.CopyToServerCache(_targetFilePath);

            UpdateStatus("Waiting for application...");
            this.WindowState = FormWindowState.Minimized; // Hide to tray while working

            await AppMonitor.OpenAndWaitAsync(localPath);

            this.WindowState = FormWindowState.Normal;
            UpdateStatus("Application closed. Syncing back to server...");
            
            await syncManager.SyncBackWithRecoveryAsync(_targetFilePath, localPath, s => UpdateStatus(s));

            UpdateStatus("Complete.", false);
            await Task.Delay(2000);
            Application.Exit();
        }
        catch (Exception ex)
        {
            UpdateStatus("Error occurred.", false);
            MessageBox.Show(ex.Message, "PVSmartLauncher Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void MainForm_Resize(object? sender, EventArgs e)
    {
        if (this.WindowState == FormWindowState.Minimized)
        {
            this.Hide();
            trayIcon.Visible = true;
        }
    }

    private void TrayIcon_DoubleClick(object? sender, EventArgs e)
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        Logger.OnLogAdded -= AppendLog;
    }
}