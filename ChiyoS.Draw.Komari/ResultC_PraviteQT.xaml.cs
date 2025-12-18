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
using System.Windows.Media.Animation;
namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// ResultC_PraviteQT.xaml 的交互逻辑
    /// </summary>
    public partial class ResultC_PraviteQT : UserControl
    {
        public ResultC_PraviteQT(string id,string name, string index) 
        { 
            InitializeComponent();
            InitializeComponent();
            this.Tbk_Name.Text = name;
            this.Tbk_Num.Text = index;
            Storyboard storyboard = FindResource("Stbd_1") as Storyboard;
            storyboard.Begin(Gd_1);
        }
    }
}
