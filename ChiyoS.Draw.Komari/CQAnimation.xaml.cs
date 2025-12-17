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
using System.Threading;
using System.Collections;
namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// CQAnimation.xaml 的交互逻辑
    /// </summary>
    public partial class CQAnimation : Window
    {
        System.Timers.Timer timer1 = new System.Timers.Timer();

        public CQAnimation()
        {
            InitializeComponent();
        }

        public void SetName(ArrayList names)
        {
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (string name in names)
                    {
                        Trs_1.Content = name;
                        Thread.Sleep(2000);
                    }
                }));
                
            });
            
                
        }
    }
}
