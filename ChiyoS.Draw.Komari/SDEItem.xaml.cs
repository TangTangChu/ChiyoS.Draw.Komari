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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// SDEItem.xaml 的交互逻辑
    /// </summary>
    public partial class SDEItem : UserControl
    {
        public SDEItem(int index,string name,string gender)
        {
            InitializeComponent();
            Tbk_index.Text = index.ToString();
            Tbx_name.Text = name;
            if(gender == "b")
            {
                Cbx_Gender.SelectedIndex = 0;
                //Stp_1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF008FFA"));
            }
            else
            {
                Cbx_Gender.SelectedIndex = 1;
                //Stp_1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF14483"));
            }
        }
    }
}
