using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.System.Power;
using Windows.Graphics.Display;

namespace Battery_Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
               
                public MainWindow()
        {
           
            InitializeComponent();
            Vis.counter = 0;
            PowerManager.RemainingChargePercentChanged += PowerManager_RemainingChargePercentChanged;
            
        }
                static class Vis
        {
            public static int counter;
        }
             private void Window_Loaded(object sender, EventArgs e)
        {
            Vis.counter = 0;
           // System.Windows.MessageBox.Show("Start");
            PowerStatus status = SystemInformation.PowerStatus;
           //percentage.Text = status.BatteryLifePercent.ToString("P0");
            //timeremaining.Text = status.BatteryLifeRemaining.ToString();
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
            //RefreshStatus;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += RefreshStatus;
            timer.Start();
            
        }
        void Window_Deactivated(object sender, EventArgs e)
        {

            this.Close();
            Vis.counter = 1;
            
        }

        void RefreshStatus(object sender, EventArgs e)
        {
            if(Vis.counter == 1)
            {
                DispatcherTimer timer = (DispatcherTimer)sender;
                timer.Stop();
               // System.Windows.MessageBox.Show("Stop");
              
            }
            powerrefresh();
            
        }

        private void PowerManager_RemainingChargePercentChanged(object sender, object e)
        {
            // System.Windows.MessageBox.Show("hi");
            bool c = bool.Parse("false");
            if (IsActiveProperty.Equals(c))
            {
                System.Windows.MessageBox.Show("hey");
                powerrefresh();
            }
            
 
        }
        public void powerrefresh()
        {
            PowerStatus power = SystemInformation.PowerStatus;

            // Battery Remaining
            int secondsRemaining = power.BatteryLifeRemaining;
            if (secondsRemaining >= 0)
            {
                TimeSpan remaining = TimeSpan.FromSeconds(secondsRemaining);
                if (remaining.TotalHours < 1)
                {
                    timeremaining.Text = remaining.Minutes + " minutes";
                }
                else
                {
                    timeremaining.Text = string.Format("{0:00}h {1:00}m", (int)remaining.TotalHours, remaining.Minutes);
                }
                //timeremaining.Text = string.Format("{0} min", secondsRemaining / 60);
                //timeremaining.Text = string.Format("{0} sec", secondsRemaining);
            }
            else
            {
                timeremaining.Text = string.Empty;
            }

            // Battery Status
            // percentage.Text = power.BatteryChargeStatus.ToString();

            percentage.Text = PowerManager.RemainingChargePercent.ToString();
        }

    }
}