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
    /// UserControl3.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl3 : UserControl
    {
        public UserControl3(string name,string sex,string num,int cvid)
        {
            InitializeComponent();
            Tbk_Name.Text = name;
            Tbk_Number.Text = num; 
            ImageBrush berriesBrush = new ImageBrush();
            berriesBrush.Stretch = Stretch.UniformToFill;
            if (sex == "b")
            {
                Border_1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00A5F1"));
                berriesBrush.ImageSource = new BitmapImage(new Uri(string.Format("D:\\ChiyoS\\source\\cv\\a{0}.jpg",cvid.ToString()), UriKind.Absolute));
                Border_1.Background = berriesBrush;
            }
            else
            {
                Border_1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF72283"));
                berriesBrush.ImageSource = new BitmapImage(new Uri(string.Format("D:\\ChiyoS\\source\\cv\\b{0}.png", cvid.ToString()), UriKind.Absolute));
                Border_1.Background = berriesBrush;
            }
        }
    }
}
