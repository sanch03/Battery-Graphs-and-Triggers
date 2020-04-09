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
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Battery_Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static class lis
        {
            public static List<List<int>> matrix = new List<List<int>>();
            public static int final;
        }
        public MainWindow()
        {
        InitializeComponent();
            lis.matrix.Clear();
            addtolist();

            test();






            Vis.counter = 0;
            PowerManager.RemainingChargePercentChanged += PowerManager_RemainingChargePercentChanged;


            


        }


        void addtolist()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            int count = lis.matrix.Count;
            //System.Windows.MessageBox.Show(PowerManager.);
            
            lis.matrix.Add(new List<int>());
            lis.matrix[count].Add(secondsSinceEpoch);
            lis.matrix[count].Add(PowerManager.RemainingChargePercent);

            
            //if (SystemInformation.PowerStatus.BatteryChargeStatus = 8)
            lis.final = secondsSinceEpoch+SystemInformation.PowerStatus.BatteryLifeRemaining;
        }

        void test()
        {

            // Container.LogList.Add
            SeriesCollection Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<ObservablePoint>() {

 },
                    PointGeometry = null

                },

                new LineSeries
                {
                    Title = "Estimate",
                    Values = new ChartValues<ObservablePoint>() {

 },
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Stroke = System.Windows.Media.Brushes.Gray,
                    StrokeDashArray = new DoubleCollection { 2 },
                    PointGeometry = null

                }

            };
            for (int i = 0; i < lis.matrix.Count; i++)
            {
                int x = lis.matrix[i][0];
                int y = lis.matrix[i][1];
                Series[0].Values.Add(new ObservablePoint(x, y));
                System.Diagnostics.Debug.WriteLine("x" + i.ToString() + "=" + x.ToString() + " y" + i.ToString() + "=" + y.ToString());
            }
            
            Series[1].Values.Add(new ObservablePoint(lis.matrix[lis.matrix.Count-1][0], lis.matrix[lis.matrix.Count-1][1]));
            Series[1].Values.Add(new ObservablePoint(lis.final, 0));

            //modifying any series values will also animate and update the chart
            //Series[3].Values.Add(5d);

            //DataContext = this;

            RankGraph.Series = Series;

        }

                static class Vis
        {
            public static int counter;
        }
             private void Window_Loaded(object sender, EventArgs e)
        {
            powerrefresh();
            Vis.counter = 0;
           // System.Windows.MessageBox.Show("Start");
            PowerStatus status = SystemInformation.PowerStatus;
           //percentage.Text = status.BatteryLifePercent.ToString("P0");
            //timeremaining.Text = status.BatteryLifeRemaining.ToString();
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
            //RefreshStatus;
            //DispatcherTimer timer = new DispatcherTimer();
           // timer.Interval = TimeSpan.FromSeconds(5);
           // timer.Tick += RefreshStatus;
           // timer.Start();






        }
        void Window_Deactivated(object sender, EventArgs e)
        {

            this.Close();
            Vis.counter = 1;
            
        }


        void RefreshStatus(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(lis.matrix[0][0]);
            System.Diagnostics.Debug.WriteLine(lis.matrix[0][1]);
            if (lis.matrix.Count > 1)
            {
                System.Diagnostics.Debug.WriteLine(lis.matrix[1][0]);
                System.Diagnostics.Debug.WriteLine(lis.matrix[1][1]);
            }
            if (Vis.counter == 1)
            {
               // DispatcherTimer timer = (DispatcherTimer)sender;
               // timer.Stop();
               // System.Windows.MessageBox.Show("Stop");
              
            }
            powerrefresh();
            
        }

        private void PowerManager_RemainingChargePercentChanged(object sender, object e)
        {
            addtolist();
            powerrefresh();
            // System.Windows.MessageBox.Show("hi");
           // bool c = bool.Parse("false");
            //if (IsActiveProperty.Equals(c))
           // {
                //System.Windows.MessageBox.Show("hey");
              //  powerrefresh();
          //  }
            
 
        }
        public void powerrefresh()
        {
            PowerStatus power = SystemInformation.PowerStatus;
            test();
            //Series[0].Values.Add(new ObservablePoint(160, 80));
            

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
                if (PowerManager.RemainingChargePercent == 100)
                {
                    timeremaining.Text = "Fully Charged";
                } else
                {
                    timeremaining.Text = "Charging";
                }
            }

            // Battery Status
            // percentage.Text = power.BatteryChargeStatus.ToString();

            percentage.Text = PowerManager.RemainingChargePercent.ToString()+"%";
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {

        }
    }

    
    }


