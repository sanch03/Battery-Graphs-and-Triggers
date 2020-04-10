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
using Newtonsoft.Json;

namespace Battery_Monitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        static class lis
        {

            public static string output = Battery_Monitor.Properties.Settings.Default.test;
            public static List<List<int>> matrix = JsonConvert.DeserializeObject<List<List<int>>>(output);
           // public static List<List<int>> matrix = new List<List<int>>();
            public static int final;
        }
        public MainWindow()
        {
            InitializeComponent();
            //System.Windows.MessageBox.Show("Started");
            //  lis.matrix.Clear();
            //string output = Battery_Monitor.Properties.Settings.Default.test;
            //List<List<int>> Product = JsonConvert.DeserializeObject<List<List<int>>>(output);

            addtolist();

            test();

            RankGraph.Series = graph.Series;


            

            Vis.counter = 0;
            PowerManager.RemainingChargePercentChanged += PowerManager_RemainingChargePercentChanged;
            PowerManager.BatteryStatusChanged += PowerManager_RemainingChargePercentChanged;
            PowerManager.EnergySaverStatusChanged += PowerManager_RemainingChargePercentChanged;
           // PowerManager.RemainingDischargeTimeChanged += PowerManager_RemainingDischargeTimeChanged;



        }


        void addtolist()
        {
            if (lis.matrix[0][0] == 0)
            {
                lis.matrix.Clear();
            }
            //Battery_Monitor.Properties.Settings.Default.test = "[[0,0]]";
            //Battery_Monitor.Properties.Settings.Default.Save();
            //System.Windows.MessageBox.Show(lis.output.ToString());
            // System.Windows.MessageBox.Show(lis.matrix.Count.ToString());
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            int count2 = lis.matrix.Count;
            //System.Windows.MessageBox.Show(PowerManager.);
            if (lis.matrix.Count > 0)
            {
                if (lis.matrix[count2 - 1][1] >= 90 && lis.matrix[count2 - 1][1] > PowerManager.RemainingChargePercent)
                {
                    lis.matrix.Clear();
                }
            }
            int count = lis.matrix.Count;

            lis.matrix.Add(new List<int>());
            lis.matrix[count].Add(secondsSinceEpoch);
            lis.matrix[count].Add(PowerManager.RemainingChargePercent);

            
            //if (SystemInformation.PowerStatus.BatteryChargeStatus = 8)
            lis.final = secondsSinceEpoch+SystemInformation.PowerStatus.BatteryLifeRemaining;

            string output = JsonConvert.SerializeObject(lis.matrix);
            //System.Windows.MessageBox.Show(output);
            Battery_Monitor.Properties.Settings.Default.test = output;
            Battery_Monitor.Properties.Settings.Default.Save();

            System.Diagnostics.Debug.WriteLine(output);
            test();
        }

        static class graph
        {

            // Container.LogList.Add
            public static SeriesCollection Series = new SeriesCollection
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
            
        }
        void test() {
            graph.Series[0].Values.Clear();
            graph.Series[1].Values.Clear();

            for (int i = 0; i < lis.matrix.Count; i++)
            {
                int x = lis.matrix[i][0];
                int y = lis.matrix[i][1];
                graph.Series[0].Values.Add(new ObservablePoint(x, y));
                //System.Diagnostics.Debug.WriteLine("x" + i.ToString() + "=" + x.ToString() + " y" + i.ToString() + "=" + y.ToString());
            }
            List<string> powerval3 = SystemInformation.PowerStatus.BatteryChargeStatus.ToString().Split(',').ToList<string>();
                graph.Series[1].Values.Add(new ObservablePoint(lis.matrix[lis.matrix.Count - 1][0], lis.matrix[lis.matrix.Count - 1][1]));
                graph.Series[1].Values.Add(new ObservablePoint(lis.final, 0));

            //modifying any series values will also animate and update the chart
            //Series[3].Values.Add(5d);

            //DataContext = this;

            

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
            System.Diagnostics.Debug.WriteLine("perchange");
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
            System.Diagnostics.Debug.WriteLine("power-ref");



            PowerStatus power = SystemInformation.PowerStatus;
            test();
            //Series[0].Values.Add(new ObservablePoint(160, 80));

            //System.Windows.MessageBox.Show(power.PowerLineStatus.ToString());

            // Battery Remaining
            int th;
            int tmin;
            string hval;
            string tval;

            TimeSpan tsince = TimeSpan.FromSeconds(lis.matrix[lis.matrix.Count - 1][0] - lis.matrix[0][0]);
            List<string> ttime = tsince.TotalHours.ToString().Split('.').ToList<string>();

            if (tsince.TotalHours < 1)
            {
                this.Dispatcher.Invoke(() =>
                {
                    timesince.Text = tsince.Minutes + " minutes ago";
                });

            }
            else
            {
            
            th = Int32.Parse(ttime[0]);
            if (ttime.Count == 2)
            {
                tmin = (int)(Convert.ToDouble("." + ttime[1]) * 60);
            }
            else
            {
                tmin = 0;
            }

            if (th == 1)
            {
                hval = " hour ";

            }
            else
            {
                hval = " hours ";
            }
            if (tmin == 1)
            {
                tval = " minute ";
            }
            else
            {
                tval = " minutes ";
            }
            this.Dispatcher.Invoke(() =>
            {
                if (tmin == 0)
                {
                    timesince.Text = th.ToString() + hval + "ago";
                }
                else
                {
                    timesince.Text = th.ToString() + hval + tmin.ToString() + tval + "ago";
                }
                // timeremaining.Text = string.Format("{0:00}h {1:00}m", (int)tremaining.TotalHours, tremaining.Minutes);
            });
            
        }
            ttime.Clear();









            int secondsRemaining = power.BatteryLifeRemaining;
            if (secondsRemaining >= 0)
            {
                
                TimeSpan tremaining = TimeSpan.FromSeconds(secondsRemaining);
             //   System.Windows.MessageBox.Show(tremaining.TotalHours.ToString());
                if (tremaining.TotalHours < 1)
                {
                     this.Dispatcher.Invoke(() =>
                     {
                         timeremaining.Text = tremaining.Minutes + " minutes";
                     });
                   
                }
                else
                {
                    ttime = tremaining.TotalHours.ToString().Split('.').ToList<string>();
                    th = Int32.Parse(ttime[0]);
                    if (ttime.Count == 2) {
                        tmin = (int)(Convert.ToDouble("."+ttime[1])*60);
                    } else
                    {
                        tmin = 0;
                    }

                   if (th == 1)
                   {
                      hval = " hour ";
                        
                    } else
                    {
                        hval = " hours ";
                    }
                    if (tmin == 1)
                    {
                        tval = " minute ";
                    } else
                    {
                        tval = " minutes ";
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        if (tmin == 0)
                        {
                            timeremaining.Text = th.ToString() + hval + "remaining";
                        } else
                        {
                            timeremaining.Text = th.ToString() + hval + tmin.ToString() + tval + "remaining";
                        }
                       // timeremaining.Text = string.Format("{0:00}h {1:00}m", (int)tremaining.TotalHours, tremaining.Minutes);
                    });
                    
                }
                //timeremaining.Text = string.Format("{0} min", secondsRemaining / 60);
                //timeremaining.Text = string.Format("{0} sec", secondsRemaining);
            }
            else
            {
                if (PowerManager.RemainingChargePercent == 100)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if (SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Online")
                        {
                            timeremaining.Text = "Plugged In\nFully Charged";
                        }
                        else
                        {
                            timeremaining.Text = "Fully Charged";
                        }
                    });
                    
                } else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        timeremaining.Text = "Plugged In\nCharging";
                    });
                    
                }
                List<string> powerval2 = power.BatteryChargeStatus.ToString().Split(',').ToList<string>();
                //System.Windows.MessageBox.Show(powerval2[1].ToString());
                if (powerval2.Count == 1 && SystemInformation.PowerStatus.PowerLineStatus.ToString() != "Online")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        timeremaining.Text = "Discharging";
                    });
                    
                }

            }

            // Battery Status
            // percentage.Text = power.BatteryChargeStatus.ToString();
            List<string> powerval = power.BatteryChargeStatus.ToString().Split(',').ToList<string>();
            string[] nbat = { "\uE850", "\uE851", "\uE852", "\uE853", "\uE854", "\uE855", "\uE856", "\uE857", "\uE858", "\uE859", "\uE83F" };
            string[] cbat = { "\uE85A", "\uE85B", "\uE85C", "\uE85D", "\uE85E", "\uE85F", "\uE860", "\uE861", "\uE862", "\uE83E", "\uEA93" };
            string[] sbat = { "\uE863", "\uE864", "\uE865", "\uE866", "\uE867", "\uE868", "\uE869", "\uE86A", "\uE86B", "\uEA94", "\uEA95" };
            string bval = "nbat";
            //string[] fbat = nbat;
            if (PowerManager.EnergySaverStatus.ToString() == "On")
            {
                bval = "sbat";
                //fbat = sbat;
            }
            if (powerval.Count > 1 && powerval[1].ToString() == " Charging")
            {
                bval = "cbat";
                
            }
            if (SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Online")
            {
                bval = "cbat";
            }

                // System.Diagnostics.Debug.WriteLine(powerval[1].ToString());

                decimal batper10 = PowerManager.RemainingChargePercent / 10;
            int batlvl = Convert.ToInt32(Math.Floor(batper10));
          //  System.Windows.MessageBox.Show(SystemInformation.PowerStatus.PowerLineStatus.ToString());
            this.Dispatcher.Invoke(() =>
            {
                percentage.Text = PowerManager.RemainingChargePercent.ToString() + "%";
                if (bval == "nbat")
                {
                    baticon.Text = nbat[batlvl];
                }
                else if (bval == "sbat")
                {
                    baticon.Text = sbat[batlvl];

                }
                else if (bval == "cbat")
                {
                    baticon.Text = cbat[batlvl];
                }
                
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
    }

    
    }


