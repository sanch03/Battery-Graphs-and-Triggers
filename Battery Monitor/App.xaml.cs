using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.System.Power;

namespace Battery_Monitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;
        protected override void OnStartup(StartupEventArgs e)
        {
            string str = PowerManager.RemainingChargePercent.ToString();
            Font fontToUse = new Font("Microsoft Sans Serif", 16, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brushToUse = new SolidBrush(Color.White);
            Bitmap bitmapText = new Bitmap(16, 16);
            Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            g.Clear(Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(str, fontToUse, brushToUse, -4, -1);
            hIcon = (bitmapText.GetHicon());
            
            //DestroyIcon(hIcon.ToInt32);
            
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Click += (s, args) => ShowMainWindow();
             _notifyIcon.Icon = System.Drawing.Icon.FromHandle(hIcon);
            //_notifyIcon.Icon = Battery_Monitor.Properties.Resources.ea93_Sm6_icon;
            _notifyIcon.Visible = true;

            CreateContextMenu();
            PowerManager.RemainingChargePercentChanged += ChargeChange;

        }
        public void ChargeChange(object sender, object e)
        {
            string str = PowerManager.RemainingChargePercent.ToString();
            Font fontToUse = new Font("Microsoft Sans Serif", 16, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brushToUse = new SolidBrush(Color.White);
            Bitmap bitmapText = new Bitmap(16, 16);
            Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            g.Clear(Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(str, fontToUse, brushToUse, -4, -1);
            hIcon = (bitmapText.GetHicon());
            _notifyIcon.Icon = System.Drawing.Icon.FromHandle(hIcon);
        }
        public void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
                MainWindow.Activate();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
               MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }
        public void test()
        {
            MessageBox.Show("hi");
        }
    }
}
