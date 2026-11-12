namespace ping79_00
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeStatusResources();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.apply = new System.Windows.Forms.Button();
            this.editIpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.Button();
            this.exitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ipInput = new System.Windows.Forms.ComboBox();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.logAnalysisMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.start = new System.Windows.Forms.Button();
            this.startMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.Label();
            this.stop = new System.Windows.Forms.Button();
            this.stopMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(275, 18);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(110, 30);
            this.apply.TabIndex = 1;
            this.apply.Text = "IP 변경";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // editIpMenu
            // 
            this.editIpMenu.Name = "editIpMenu";
            this.editIpMenu.Size = new System.Drawing.Size(126, 22);
            this.editIpMenu.Text = "IP 변경";
            this.editIpMenu.Click += new System.EventHandler(this.EditIp_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(270, 65);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(110, 30);
            this.exit.TabIndex = 4;
            this.exit.Text = "종료";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // exitMenu
            // 
            this.exitMenu.Name = "exitMenu";
            this.exitMenu.Size = new System.Drawing.Size(126, 22);
            this.exitMenu.Text = "Exit";
            this.exitMenu.Click += new System.EventHandler(this.Exit_Click);
            // 
            // ipInput
            // 
            this.ipInput.AccessibleName = "검사할 IP 주소";
            this.ipInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.ipInput.FormattingEnabled = true;
            this.ipInput.MaxDropDownItems = 10;
            this.ipInput.Location = new System.Drawing.Point(20, 20);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(240, 21);
            this.ipInput.TabIndex = 0;
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(123, 6);
            // 
            // logAnalysisMenu
            // 
            this.logAnalysisMenu.Name = "logAnalysisMenu";
            this.logAnalysisMenu.Size = new System.Drawing.Size(126, 22);
            this.logAnalysisMenu.Text = "로그 분석";
            this.logAnalysisMenu.Click += new System.EventHandler(this.LogAnalysis_Click);
            // 
            // exitSeparator
            // 
            this.exitSeparator.Name = "exitSeparator";
            this.exitSeparator.Size = new System.Drawing.Size(123, 6);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(20, 65);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(110, 30);
            this.start.TabIndex = 2;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.Start_Click);
            // 
            // startMenu
            // 
            this.startMenu.Name = "startMenu";
            this.startMenu.Size = new System.Drawing.Size(126, 22);
            this.startMenu.Text = "Start";
            this.startMenu.Click += new System.EventHandler(this.StartMenu_Click);
            // 
            // status
            // 
            this.status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.status.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.status.Location = new System.Drawing.Point(20, 115);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(360, 60);
            this.status.TabIndex = 5;
            this.status.Text = "ip:192.168.137.79";
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(145, 65);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(110, 30);
            this.stop.TabIndex = 3;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // stopMenu
            // 
            this.stopMenu.Name = "stopMenu";
            this.stopMenu.Size = new System.Drawing.Size(126, 22);
            this.stopMenu.Text = "Stop";
            this.stopMenu.Click += new System.EventHandler(this.Stop_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // tray
            // 
            this.tray.ContextMenuStrip = this.trayMenu;
            this.tray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tray_MouseClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editIpMenu,
            this.startMenu,
            this.stopMenu,
            this.menuSeparator,
            this.logAnalysisMenu,
            this.exitSeparator,
            this.exitMenu});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(127, 126);
            // 
            // Form1
            // 
            this.AcceptButton = this.apply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 190);
            this.Controls.Add(this.ipInput);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.start);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.status);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ping 상태 모니터";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.ToolStripMenuItem editIpMenu;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.ToolStripMenuItem exitMenu;
        private System.Windows.Forms.ComboBox ipInput;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private System.Windows.Forms.ToolStripMenuItem logAnalysisMenu;
        private System.Windows.Forms.ToolStripSeparator exitSeparator;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.ToolStripMenuItem startMenu;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.ToolStripMenuItem stopMenu;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.NotifyIcon tray;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
    }
}
