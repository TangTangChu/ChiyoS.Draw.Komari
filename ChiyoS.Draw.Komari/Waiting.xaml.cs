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
using System.Timers;
namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// Waiting.xaml 的交互逻辑
    /// </summary>
    public partial class Waiting : Window
    {
        System.Timers.Timer timer1 = new System.Timers.Timer();
        int i = 0;
        public Waiting()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Elapsed += wc;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            timer1.Start();
        }

        private void wc(object sender, ElapsedEventArgs e)
        {
            i++;
            if (i < 5)
            {
                Console.WriteLine("C1");
            }
            else
            {
                timer1.Stop();
                
                this.Close();
            }
        }
        }
}
