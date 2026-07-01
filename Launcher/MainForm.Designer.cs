namespace PVSmartLauncher;

using System.Drawing;
using System.Windows.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblCurrentFile;
    private Label lblStatus;
    private ProgressBar progressBar;
    private ListBox lstLog;
    private NotifyIcon trayIcon;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.lblCurrentFile = new Label();
        this.lblStatus = new Label();
        this.progressBar = new ProgressBar();
        this.lstLog = new ListBox();
        this.trayIcon = new NotifyIcon(this.components);

        this.SuspendLayout();

        // lblCurrentFile
        this.lblCurrentFile.AutoSize = true;
        this.lblCurrentFile.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        this.lblCurrentFile.ForeColor = Color.White;
        this.lblCurrentFile.Location = new Point(20, 20);
        this.lblCurrentFile.Name = "lblCurrentFile";
        this.lblCurrentFile.Size = new Size(150, 25);
        this.lblCurrentFile.Text = "Waiting for file...";

        // lblStatus
        this.lblStatus.AutoSize = true;
        this.lblStatus.Font = new Font("Segoe UI", 9F);
        this.lblStatus.ForeColor = Color.LightGray;
        this.lblStatus.Location = new Point(20, 60);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new Size(60, 20);
        this.lblStatus.Text = "Status: Idle";

        // progressBar
        this.progressBar.Location = new Point(20, 95);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new Size(540, 10);
        this.progressBar.Style = ProgressBarStyle.Marquee;

        // lstLog
        this.lstLog.BackColor = Color.FromArgb(30, 30, 30);
        this.lstLog.ForeColor = Color.LimeGreen;
        this.lstLog.FormattingEnabled = true;
        this.lstLog.ItemHeight = 15;
        this.lstLog.Location = new Point(20, 120);
        this.lstLog.Name = "lstLog";
        this.lstLog.Size = new Size(540, 200);
        this.lstLog.Font = new Font("Consolas", 9F);
        this.lstLog.BorderStyle = BorderStyle.None;

        // trayIcon
        this.trayIcon.Text = "PV Smart Launcher";
        this.trayIcon.Visible = true;
        this.trayIcon.Icon = SystemIcons.Application;
        this.trayIcon.DoubleClick += new EventHandler(this.TrayIcon_DoubleClick);

        // MainForm
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.BackColor = Color.FromArgb(45, 45, 48); // Dark Theme Core
        this.ClientSize = new Size(580, 350);
        this.Controls.Add(this.lstLog);
        this.Controls.Add(this.progressBar);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.lblCurrentFile);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "MainForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "PV Smart Launcher";
        this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
        this.Resize += new EventHandler(this.MainForm_Resize);
        
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}