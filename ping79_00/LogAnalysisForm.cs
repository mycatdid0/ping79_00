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

        public LogAnalysisForm()
        {
            InitializeComponent();
            components.Add(folderDialog);
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
            folderStatus.Text = loadingFolder + " — 로딩 중...";
            ClearResults();
            try
            {
                List<string> results = await Task.Run(() => LogAnalysis.Load(loadingFolder));
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
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException ||
                ex is ArgumentException || ex is NotSupportedException)
            {
                if (IsDisposed || Disposing || version != loadVersion) return;
                folderStatus.Text = loadingFolder + " — 로딩 실패";
                MessageBox.Show(this, "로그를 로딩하지 못했습니다.\r\n" + ex.Message,
                    "로그 분석", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
