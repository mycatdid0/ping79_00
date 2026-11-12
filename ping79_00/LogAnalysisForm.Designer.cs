namespace ping79_00
{
    partial class LogAnalysisForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.changeFolderMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.resultsPanel = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.folderStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.mainMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // mainMenu
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.changeFolderMenu, this.reloadMenu });
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Size = new System.Drawing.Size(1100, 24);
            this.changeFolderMenu.Name = "changeFolderMenu";
            this.changeFolderMenu.Text = "폴더 변경...";
            this.changeFolderMenu.Click += new System.EventHandler(this.ChangeFolder_Click);
            this.reloadMenu.Name = "reloadMenu";
            this.reloadMenu.Text = "재로딩";
            this.reloadMenu.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.reloadMenu.Click += new System.EventHandler(this.Reload_Click);
            // resultsPanel
            this.resultsPanel.AutoScroll = true;
            this.resultsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsPanel.Font = new System.Drawing.Font("Consolas", 10F);
            this.resultsPanel.Name = "resultsPanel";
            this.resultsPanel.Padding = new System.Windows.Forms.Padding(8);
            this.resultsPanel.TabIndex = 0;
            this.resultsPanel.SizeChanged += new System.EventHandler(this.ResultsPanel_SizeChanged);
            // statusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.folderStatus });
            this.statusStrip.Name = "statusStrip";
            this.folderStatus.Name = "folderStatus";
            this.folderStatus.Spring = true;
            this.folderStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.folderDialog.Description = "분석할 로그 폴더를 선택하세요.";
            this.folderDialog.ShowNewFolderButton = false;
            // LogAnalysisForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 750);
            this.Controls.Add(this.resultsPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(600, 350);
            this.Name = "LogAnalysisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "로그 분석";
            this.Shown += new System.EventHandler(this.LogAnalysisForm_Shown);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem changeFolderMenu;
        private System.Windows.Forms.ToolStripMenuItem reloadMenu;
        private System.Windows.Forms.Panel resultsPanel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel folderStatus;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
    }
}
