using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ping79_00
{
    public partial class Form1 : Form
    {
        private string targetIp = "192.168.137.79";
        private bool monitoring, pending, exiting, disposed;
        private int generation;
        private readonly Icon idle = MakeIcon(Color.Gray);
        private readonly Icon success = MakeIcon(Color.RoyalBlue);
        private readonly Icon failure = MakeIcon(Color.Red);
        public Form1()
        {
            InitializeComponent();
            ipInput.Text = targetIp;
            tray.Icon = idle;
            tray.Visible = true;
            SetStatus(idle, "중지됨");
            UpdateButtons();
        }

        private void Apply_Click(object sender, EventArgs e) { ApplyIp(); }
        private void Start_Click(object sender, EventArgs e) { if (ApplyIp()) StartMonitoring(); }
        private void Stop_Click(object sender, EventArgs e) { StopMonitoring(); }
        private void Exit_Click(object sender, EventArgs e) { ExitApplication(); }
        private void EditIp_Click(object sender, EventArgs e) { ShowMain(true); }
        private void StartMenu_Click(object sender, EventArgs e) { StartMonitoring(); }
        private void Tray_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) ShowMain(false);
        }
        private async void Timer_Tick(object sender, EventArgs e) { await CheckAsync(); }
        private void Form1_Shown(object sender, EventArgs e) { StartMonitoring(); }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exiting && e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; Hide(); }
        }
        private bool ApplyIp()
        {
            IPAddress address;
            if (!IPAddress.TryParse(ipInput.Text.Trim(), out address))
            {
                MessageBox.Show(this, "올바른 IP 주소를 입력하세요.", "IP 변경", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ipInput.Focus();
                ipInput.SelectAll();
                return false;
            }
            ipInput.Text = address.ToString();
            if (targetIp == ipInput.Text) return true;
            targetIp = ipInput.Text;
            generation++;
            SetStatus(idle, monitoring ? "검사 중" : "중지됨");
            if (monitoring) _ = CheckAsync();
            return true;
        }

        private void StartMonitoring()
        {
            if (monitoring) return;
            monitoring = true;
            generation++;
            SetStatus(idle, "검사 중");
            UpdateButtons();
            timer.Start();
            _ = CheckAsync();
        }

        private void StopMonitoring()
        {
            monitoring = false;
            generation++;
            timer.Stop();
            SetStatus(idle, "중지됨");
            UpdateButtons();
        }

        private async Task CheckAsync()
        {
            if (!monitoring || pending || disposed) return;
            pending = true;
            int version = generation;
            bool ok = false;
            long ms = 0;
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(targetIp, 900);
                    ok = reply.Status == IPStatus.Success;
                    ms = reply.RoundtripTime;
                }
            }
            catch (PingException) { }
            catch (System.Net.Sockets.SocketException) { }
            catch (InvalidOperationException) { }
            finally { pending = false; }
            // Ignore results from checks preceding Stop, IP changes, or shutdown.
            if (disposed || exiting || !monitoring || version != generation) return;
            SetStatus(ok ? success : failure, ok ? "성공 (" + ms + " ms)" : "실패");
        }

        private void SetStatus(Icon icon, string text)
        {
            tray.Icon = icon;
            string tooltip = targetIp + " - " + text;
            tray.Text = tooltip.Length > 63 ? tooltip.Substring(0, 63) : tooltip;
            status.Text = "IP: " + targetIp + "\r\n상태: " + text;
            status.ForeColor = icon == success ? Color.RoyalBlue : icon == failure ? Color.Firebrick : Color.DimGray;
        }

        private void UpdateButtons()
        {
            start.Enabled = startMenu.Enabled = !monitoring;
            stop.Enabled = stopMenu.Enabled = monitoring;
        }

        private void ShowMain(bool edit)
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            if (edit) { ipInput.Focus(); ipInput.SelectAll(); }
        }

        private void ExitApplication()
        {
            exiting = true;
            StopMonitoring();
            Close();
        }

        private void DisposeStatusResources()
        {
            if (disposed) return;
            disposed = true;
            monitoring = false;
            generation++;
            if (timer != null) timer.Stop();
            if (tray != null) { tray.Visible = false; tray.Icon = null; }
            idle.Dispose();
            success.Dispose();
            failure.Dispose();
        }

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        private static Icon MakeIcon(Color color)
        {
            using (var bitmap = new Bitmap(32, 32))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var brush = new SolidBrush(color))
            using (var border = new Pen(Color.White, 2))
            {
                graphics.Clear(Color.Transparent);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.FillEllipse(brush, 3, 3, 26, 26);
                graphics.DrawEllipse(border, 3, 3, 26, 26);
                IntPtr handle = bitmap.GetHicon();
                try { using (var icon = Icon.FromHandle(handle)) return (Icon)icon.Clone(); }
                finally { DestroyIcon(handle); }
            }
        }
    }
}
