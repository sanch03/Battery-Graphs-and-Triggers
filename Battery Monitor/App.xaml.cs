using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Windows.System.Power;
using Application = System.Windows.Application;
using FontStyle = System.Drawing.FontStyle;

namespace Battery_Monitor
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool _isExit;
        private NotifyIcon _notifyIcon;


        protected override void OnStartup(StartupEventArgs e)
        {
            _notifyIcon = new NotifyIcon();
            var str = PowerManager.RemainingChargePercent.ToString();
            if (PowerManager.RemainingChargePercent != 100)
            {
                var fontToUse = new Font("Microsoft Sans Serif", 16, FontStyle.Regular, GraphicsUnit.Pixel);
                Brush brushToUse = new SolidBrush(Color.White);
                var bitmapText = new Bitmap(16, 16);
                var g = Graphics.FromImage(bitmapText);

                IntPtr hIcon;

                g.Clear(Color.Transparent);
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString(str, fontToUse, brushToUse, -4, -2);
                hIcon = bitmapText.GetHicon();
                _notifyIcon.Icon = Icon.FromHandle(hIcon);
            }
            else
            {
                var fontToUse = new Font("Trebuchet MS", 12, FontStyle.Regular, GraphicsUnit.Pixel);
                Brush brushToUse = new SolidBrush(Color.White);
                var bitmapText = new Bitmap(16, 16);
                var g = Graphics.FromImage(bitmapText);

                IntPtr hIcon;

                g.Clear(Color.Transparent);
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString(str, fontToUse, brushToUse, -3, 0);
                hIcon = bitmapText.GetHicon();
                _notifyIcon.Icon = Icon.FromHandle(hIcon);
            }


            //DestroyIcon(hIcon.ToInt32);

            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;


            _notifyIcon.Click += (s, args) => ShowMainWindow();

            //_notifyIcon.Icon = Battery_Monitor.Properties.Resources.ea93_Sm6_icon;
            _notifyIcon.Visible = true;

            CreateContextMenu();
            PowerManager.RemainingChargePercentChanged += ChargeChange;
            tooltipchange();
        }

        private void tooltipchange()
        {
            int th;
            int tmin;
            var power = SystemInformation.PowerStatus;
            var secondsRemaining = power.BatteryLifeRemaining;
            if (secondsRemaining >= 0)
            {
                var tremaining = TimeSpan.FromSeconds(secondsRemaining);
                //   System.Windows.MessageBox.Show(tremaining.TotalHours.ToString());
                if (tremaining.TotalHours < 1)
                {
                    Dispatcher.Invoke(() =>
                    {
                        _notifyIcon.Text = tremaining.Minutes + " minutes (" + PowerManager.RemainingChargePercent +
                                           "%) remaining";
                    });
                }
                else
                {
                    var ttime = tremaining.TotalHours.ToString().Split('.').ToList();
                    ttime = tremaining.TotalHours.ToString().Split('.').ToList();
                    th = int.Parse(ttime[0]);
                    if (ttime.Count == 2)
                        tmin = (int) (Convert.ToDouble("." + ttime[1]) * 60);
                    else
                        tmin = 0;


                    Dispatcher.Invoke(() =>
                    {
                        _notifyIcon.Text = th + " hr " + tmin + " min (" + PowerManager.RemainingChargePercent +
                                           "%) remaining";

                        // timeremaining.Text = string.Format("{0:00}h {1:00}m", (int)tremaining.TotalHours, tremaining.Minutes);
                    });
                }
            }
        }

        public void ChargeChange(object sender, object e)
        {
            var str = PowerManager.RemainingChargePercent.ToString();
            if (PowerManager.RemainingChargePercent != 100)
            {
                var fontToUse = new Font("Microsoft Sans Serif", 16, FontStyle.Regular, GraphicsUnit.Pixel);
                Brush brushToUse = new SolidBrush(Color.White);
                var bitmapText = new Bitmap(16, 16);
                var g = Graphics.FromImage(bitmapText);

                IntPtr hIcon;

                g.Clear(Color.Transparent);
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString(str, fontToUse, brushToUse, -4, -2);
                hIcon = bitmapText.GetHicon();
                _notifyIcon.Icon = Icon.FromHandle(hIcon);
            }
            else
            {
                var fontToUse = new Font("Trebuchet MS", 12, FontStyle.Regular, GraphicsUnit.Pixel);
                Brush brushToUse = new SolidBrush(Color.White);
                var bitmapText = new Bitmap(16, 16);
                var g = Graphics.FromImage(bitmapText);

                IntPtr hIcon;

                g.Clear(Color.Transparent);
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawString(str, fontToUse, brushToUse, -3, 0);
                hIcon = bitmapText.GetHicon();
                _notifyIcon.Icon = Icon.FromHandle(hIcon);
            }

            // _notifyIcon.Text = "ToolTipText";
            tooltipchange();
        }

        public void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
                new ContextMenuStrip();
            // _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow.Hide();
            //MainWindow.
            // System.Windows.Forms.Application.Exit();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized) MainWindow.WindowState = WindowState.Normal;
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
    }
}