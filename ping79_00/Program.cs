using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ping79_00
{
    internal static class Program
    {
        internal static Icon WindowIcon { get; private set; }

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var stream = typeof(Program).Assembly.GetManifestResourceStream("ping79_00.PingMonitor.ico"))
            using (var icon = new Icon(stream))
            {
                WindowIcon = icon;
                Application.Run(new Form1());
            }
        }
    }
}
