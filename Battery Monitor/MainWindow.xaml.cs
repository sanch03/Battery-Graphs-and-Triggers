using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Windows.System.Power;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;

namespace Battery_Monitor
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //System.Windows.MessageBox.Show("Started");
            //  lis.matrix.Clear();
            //string output = Battery_Monitor.Properties.Settings.Default.test;
            //List<List<int>> Product = JsonConvert.DeserializeObject<List<List<int>>>(output);

            addtolist();

            Test();

            RankGraph.Series = graph.Series;


            Vis.counter = 0;
            PowerManager.RemainingChargePercentChanged += PowerManager_RemainingChargePercentChanged;
            PowerManager.BatteryStatusChanged += PowerManager_RemainingChargePercentChanged;
            PowerManager.EnergySaverStatusChanged += PowerManager_RemainingChargePercentChanged;
            // PowerManager.RemainingDischargeTimeChanged += PowerManager_RemainingDischargeTimeChanged;
        }


        private void addtolist()
        {
            if (Lis.matrix[0][0] == 0) Lis.matrix.Clear();
            //Battery_Monitor.Properties.Settings.Default.test = "[[0,0]]";
            //Battery_Monitor.Properties.Settings.Default.Save();
            //System.Windows.MessageBox.Show(lis.output.ToString());
            // System.Windows.MessageBox.Show(lis.matrix.Count.ToString());
            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var secondsSinceEpoch = (int) t.TotalSeconds;

            var count2 = Lis.matrix.Count;
            //System.Windows.MessageBox.Show(PowerManager.);
            if (Lis.matrix.Count > 0)
                if (Lis.matrix[count2 - 1][1] >= 90 && Lis.matrix[count2 - 1][1] > PowerManager.RemainingChargePercent)
                    Lis.matrix.Clear();
            var count = Lis.matrix.Count;

            Lis.matrix.Add(new List<int>());
            Lis.matrix[count].Add(secondsSinceEpoch);
            Lis.matrix[count].Add(PowerManager.RemainingChargePercent);


            //if (SystemInformation.PowerStatus.BatteryChargeStatus = 8)
            Lis.final = secondsSinceEpoch + SystemInformation.PowerStatus.BatteryLifeRemaining;

            var output = JsonConvert.SerializeObject(Lis.matrix);
            //System.Windows.MessageBox.Show(output);
            Properties.Settings.Default.test = output;
            Properties.Settings.Default.Save();

            Debug.WriteLine(output);
            Test();
        }

        private static void Test()
        {
            graph.Series[0].Values.Clear();
            graph.Series[1].Values.Clear();

            for (int i = 0; i < Lis.matrix.Count; i++)
            {
                int x = Lis.matrix[i][0];
                int y = Lis.matrix[i][1];
                graph.Series[0].Values.Add(new ObservablePoint(x, y));
                //System.Diagnostics.Debug.WriteLine("x" + i.ToString() + "=" + x.ToString() + " y" + i.ToString() + "=" + y.ToString());
            }

            //var powerval3 = SystemInformation.PowerStatus.BatteryChargeStatus.ToString().Split(',').ToList();
            graph.Series[1].Values
                .Add(new ObservablePoint(Lis.matrix[Lis.matrix.Count - 1][0], Lis.matrix[Lis.matrix.Count - 1][1]));
            graph.Series[1].Values.Add(new ObservablePoint(Lis.final, 0));

            //modifying any series values will also animate and update the chart
            //Series[3].Values.Add(5d);

            //DataContext = this;
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            Powerrefresh();

            Vis.counter = 0;
            // System.Windows.MessageBox.Show("Start");
            //percentage.Text = status.BatteryLifePercent.ToString("P0");
            //timeremaining.Text = status.BatteryLifeRemaining.ToString();
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
            //RefreshStatus;
            //DispatcherTimer timer = new DispatcherTimer();
            // timer.Interval = TimeSpan.FromSeconds(5);
            // timer.Tick += RefreshStatus;
            // timer.Start();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => { Close(); });


            //  Vis.counter = 1;
        }


        private void PowerManager_RemainingChargePercentChanged(object sender, object e)
        {
            Debug.WriteLine("perchange");
            addtolist();
            Powerrefresh();
            // System.Windows.MessageBox.Show("hi");
            // bool c = bool.Parse("false");
            //if (IsActiveProperty.Equals(c))
            // {
            //System.Windows.MessageBox.Show("hey");
            //  powerrefresh();
            //  }
        }


        public void Powerrefresh()
        {
            //Process.Start("cmd", "c:\windows\downloaded program files\cmd.exe echo hi && pause");
            Debug.WriteLine("power-ref");


            var power = SystemInformation.PowerStatus;
            Test();
            //Series[0].Values.Add(new ObservablePoint(160, 80));

            //System.Windows.MessageBox.Show(power.PowerLineStatus.ToString());

            // Battery Remaining
            int th;
            int tmin;
            string hval;
            string tval;

            var tsince = TimeSpan.FromSeconds(Lis.matrix[Lis.matrix.Count - 1][0] - Lis.matrix[0][0]);
            var ttime = tsince.TotalHours.ToString().Split('.').ToList();

            if (tsince.TotalHours < 1)
            {
                Dispatcher.Invoke(() => { timesince.Text = tsince.Minutes + " minutes ago"; });
            }
            else
            {
                th = int.Parse(ttime[0]);
                if (ttime.Count == 2)
                    tmin = (int) (Convert.ToDouble("." + ttime[1]) * 60);
                else
                    tmin = 0;

                if (th == 1)
                    hval = " hour ";
                else
                    hval = " hours ";
                if (tmin == 1)
                    tval = " minute ";
                else
                    tval = " minutes ";
                Dispatcher.Invoke(() =>
                {
                    if (tmin == 0)
                        timesince.Text = th + hval + "ago";
                    else
                        timesince.Text = th + hval + tmin + tval + "ago";
                    // timeremaining.Text = string.Format("{0:00}h {1:00}m", (int)tremaining.TotalHours, tremaining.Minutes);
                });
            }

            ttime.Clear();


            var secondsRemaining = power.BatteryLifeRemaining;
            if (secondsRemaining >= 0)
            {
                var tremaining = TimeSpan.FromSeconds(secondsRemaining);
                //   System.Windows.MessageBox.Show(tremaining.TotalHours.ToString());
                if (tremaining.TotalHours < 1)
                {
                    Dispatcher.Invoke(() => { timeremaining.Text = tremaining.Minutes + " minutes"; });
                }
                else
                {
                    ttime = tremaining.TotalHours.ToString().Split('.').ToList();
                    th = int.Parse(ttime[0]);
                    if (ttime.Count == 2)
                        tmin = (int) (Convert.ToDouble("." + ttime[1]) * 60);
                    else
                        tmin = 0;

                    if (th == 1)
                        hval = " hour ";
                    else
                        hval = " hours ";
                    if (tmin == 1)
                        tval = " minute ";
                    else
                        tval = " minutes ";
                    Dispatcher.Invoke(() =>
                    {
                        if (tmin == 0)
                            timeremaining.Text = th + hval + "remaining";
                        else
                            timeremaining.Text = th + hval + tmin + tval + "remaining";
                        // timeremaining.Text = string.Format("{0:00}h {1:00}m", (int)tremaining.TotalHours, tremaining.Minutes);
                    });
                }

                //timeremaining.Text = string.Format("{0} min", secondsRemaining / 60);
                //timeremaining.Text = string.Format("{0} sec", secondsRemaining);
            }
            else
            {
                if (PowerManager.RemainingChargePercent == 100)
                    Dispatcher.Invoke(() =>
                    {
                        var margin = timeremaining.Margin;
                        margin.Left = 200;
                        timeremaining.Margin = margin;
                        if (SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Online")
                            timeremaining.Text = "Plugged In\nFully Charged";
                        else
                            timeremaining.Text = "Fully Charged";
                    });
                else
                    Dispatcher.Invoke(() =>
                    {
                        var margin = timeremaining.Margin;
                        margin.Left = 179;
                        timeremaining.Margin = margin;
                        timeremaining.Text = "Plugged In\nCharging";
                    });
                var powerval2 = power.BatteryChargeStatus.ToString().Split(',').ToList();
                //System.Windows.MessageBox.Show(powerval2[1].ToString());
                if (powerval2.Count == 1 && SystemInformation.PowerStatus.PowerLineStatus.ToString() != "Online")
                    Dispatcher.Invoke(() => { timeremaining.Text = "Discharging"; });
            }

            // Battery Status
            // percentage.Text = power.BatteryChargeStatus.ToString();
            var powerval = power.BatteryChargeStatus.ToString().Split(',').ToList();
            string[] nbat =
            {
                "\uE850", "\uE851", "\uE852", "\uE853", "\uE854", "\uE855", "\uE856", "\uE857", "\uE858", "\uE859", "\uE83F"
            };
            string[] cbat =
            {
                "\uE85A", "\uE85B", "\uE85C", "\uE85D", "\uE85E", "\uE85F", "\uE860", "\uE861", "\uE862", "\uE83E", "\uEA93"
            };
            string[] sbat =
            {
                "\uE863", "\uE864", "\uE865", "\uE866", "\uE867", "\uE868", "\uE869", "\uE86A", "\uE86B", "\uEA94", "\uEA95"
            };
            var bval = "nbat";
            //string[] fbat = nbat;
            if (PowerManager.EnergySaverStatus.ToString() == "On")
                bval = "sbat";
            //fbat = sbat;
            if (powerval.Count > 1 && powerval[1] == " Charging") bval = "cbat";
            if (SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Online") bval = "cbat";

            // System.Diagnostics.Debug.WriteLine(powerval[1].ToString());

            decimal batper10 = PowerManager.RemainingChargePercent / 10;
            var batlvl = Convert.ToInt32(Math.Floor(batper10));
            //  System.Windows.MessageBox.Show(SystemInformation.PowerStatus.PowerLineStatus.ToString());
            Dispatcher.Invoke(() =>
            {
                percentage.Text = PowerManager.RemainingChargePercent + "%";
                if (bval == "nbat")
                    baticon.Text = nbat[batlvl];
                else if (bval == "sbat")
                    baticon.Text = sbat[batlvl];
                else if (bval == "cbat") baticon.Text = cbat[batlvl];
            });
            // System.Windows.MessageBox.Show(PowerManager.EnergySaverStatus.ToString());

            // List<string> powerval = power.BatteryChargeStatus.ToString().Split(',').ToList<string>();
            // System.Windows.MessageBox.Show(powerval[1].ToString());
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
        }


        private static class Lis
        {
            public static readonly string output = Properties.Settings.Default.test;

            public static readonly List<List<int>> matrix = JsonConvert.DeserializeObject<List<List<int>>>(output);

            // public static List<List<int>> matrix = new List<List<int>>();
            public static int final;
        }

        private static class graph
        {
            // Container.LogList.Add
            public static readonly SeriesCollection Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometry = null
                },

                new LineSeries
                {
                    Title = "Estimate",
                    Values = new ChartValues<ObservablePoint>(),
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Gray,
                    StrokeDashArray = new DoubleCollection {2},
                    PointGeometry = null
                }
            };
        }

        private static class Vis
        {
            public static int counter;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}