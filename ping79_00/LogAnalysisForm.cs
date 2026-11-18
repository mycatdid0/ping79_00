using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ping79_00
{
    public partial class LogAnalysisForm : Form
    {
        private string folder = LogFolderSettings.DefaultFolder;
        private int loadVersion;
        private DateTime? latestWriteTimeUtc;
        private readonly Timer checkTimer;
        private readonly Timer blinkTimer;
        private readonly Color reloadBackColor;

        public LogAnalysisForm()
        {
            InitializeComponent();
            Icon = Program.WindowIcon;
            components.Add(folderDialog);
            reloadBackColor = reloadMenu.BackColor;
            checkTimer = new Timer(components) { Interval = 5000 };
            blinkTimer = new Timer(components) { Interval = 500 };
            checkTimer.Tick += CheckTimer_Tick;
            blinkTimer.Tick += BlinkTimer_Tick;
        }

        private async void LogAnalysisForm_Shown(object sender, EventArgs e)
        {
            try { folder = LogFolderSettings.Read(); }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(this, "폴더 설정을 읽거나 저장하지 못했습니다.\r\n" + ex.Message,
                    "로그 분석", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            await ReloadAsync();
        }

        private async void Reload_Click(object sender, EventArgs e) { await ReloadAsync(); }

        private async void ChangeFolder_Click(object sender, EventArgs e)
        {
            folderDialog.SelectedPath = folder;
            if (folderDialog.ShowDialog(this) != DialogResult.OK) return;
            try { LogFolderSettings.Save(folderDialog.SelectedPath); }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(this, "폴더 설정을 저장하지 못했습니다.\r\n" + ex.Message,
                    "로그 분석", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            folder = folderDialog.SelectedPath;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            int version = ++loadVersion;
            string loadingFolder = folder;
            checkTimer.Stop();
            SetReloadAlert(false);
            latestWriteTimeUtc = null;
            folderStatus.Text = loadingFolder + " — 로딩 중...";
            ClearResults();
            try
            {
                DateTime? loadedWriteTimeUtc = null;
                List<string> results = await Task.Run(() =>
                    LogAnalysis.LoadWithTimestamp(loadingFolder, out loadedWriteTimeUtc));
                if (IsDisposed || Disposing || version != loadVersion) return;
                resultsPanel.SuspendLayout();
                try
                {
                    foreach (string text in results)
                    {
                        var box = new TextBox
                        {
                            Multiline = true,
                            ReadOnly = true,
                            WordWrap = false,
                            ScrollBars = ScrollBars.Horizontal,
                            BackColor = SystemColors.Window,
                            Text = text,
                            AccessibleName = text.Split('\r', '\n')[0]
                        };
                        // Include room for the border and horizontal scrollbar.
                        box.Height = checked(box.Lines.Length * box.Font.Height +
                            SystemInformation.HorizontalScrollBarHeight + 8);
                        resultsPanel.Controls.Add(box);
                    }
                    LayoutResults();
                    resultsPanel.AutoScrollPosition = Point.Empty;
                }
                finally { resultsPanel.ResumeLayout(); }
                folderStatus.Text = loadingFolder + " — " + results.Count + "개 파일" +
                    (results.Count == 0 ? " (파일 없음)" : "");
                latestWriteTimeUtc = loadedWriteTimeUtc;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException ||
                ex is ArgumentException || ex is NotSupportedException)
            {
                if (IsDisposed || Disposing || version != loadVersion) return;
                folderStatus.Text = loadingFolder + " — 로딩 실패";
                MessageBox.Show(this, "로그를 로딩하지 못했습니다.\r\n" + ex.Message,
                    "로그 분석", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (!IsDisposed && !Disposing && version == loadVersion)
                    checkTimer.Start();
            }
        }

        private async void CheckTimer_Tick(object sender, EventArgs e)
        {
            checkTimer.Stop();
            int version = loadVersion;
            string checkingFolder = folder;
            DateTime? displayedWriteTimeUtc = latestWriteTimeUtc;
            try
            {
                bool hasNewerFile = await Task.Run(() =>
                    LogAnalysis.HasNewerFile(checkingFolder, displayedWriteTimeUtc));
                if (!IsDisposed && !Disposing && version == loadVersion)
                    SetReloadAlert(hasNewerFile);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException ||
                ex is ArgumentException || ex is NotSupportedException)
            {
                // Retry on the next tick if the folder is temporarily unavailable.
            }
            finally
            {
                if (!IsDisposed && !Disposing && version == loadVersion)
                    checkTimer.Start();
            }
        }

        private void SetReloadAlert(bool hasNewerFile)
        {
            if (hasNewerFile)
            {
                if (blinkTimer.Enabled) return;
                reloadMenu.BackColor = Color.Gold;
                blinkTimer.Start();
            }
            else
            {
                blinkTimer.Stop();
                reloadMenu.BackColor = reloadBackColor;
            }
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            reloadMenu.BackColor = reloadMenu.BackColor == reloadBackColor
                ? Color.Gold : reloadBackColor;
        }

        private void ClearResults()
        {
            while (resultsPanel.Controls.Count > 0) resultsPanel.Controls[0].Dispose();
            resultsPanel.AutoScrollMinSize = Size.Empty;
            resultsPanel.AutoScrollPosition = Point.Empty;
        }

        private void ResultsPanel_SizeChanged(object sender, EventArgs e) { LayoutResults(); }

        private void LayoutResults()
        {
            int height = resultsPanel.Padding.Vertical;
            foreach (Control box in resultsPanel.Controls) height += box.Height + 8;
            int width = Math.Max(1, resultsPanel.Width - resultsPanel.Padding.Horizontal -
                (height > resultsPanel.ClientSize.Height ? SystemInformation.VerticalScrollBarWidth : 0));
            int y = resultsPanel.Padding.Top + resultsPanel.AutoScrollPosition.Y;
            foreach (Control box in resultsPanel.Controls)
            {
                box.SetBounds(resultsPanel.Padding.Left, y, width, box.Height);
                y += box.Height + 8;
            }
            resultsPanel.AutoScrollMinSize = new Size(0, height);
        }
    }
}
