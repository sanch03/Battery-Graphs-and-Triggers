using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Battery_Monitor
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            
            InitializeComponent();
            


            

        }
        public void setest(string message)
        {
            //Color.FromRgb(255, 255, 255);
            // System.Windows.MessageBox.Show(message);
            var tb = new TextBlock();
            tb.Text = "hello";
            //tb.Foreground = "white";
            //tb.Width = 20;
            
            tb.Margin = new Thickness(20, 20, 20, 20);
            gridtest.Children.Add(tb);
        }


    }
}
