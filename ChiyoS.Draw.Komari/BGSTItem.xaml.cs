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
    /// BGSTItem.xaml 的交互逻辑
    /// </summary>
    public partial class BGSTItem : UserControl
    {
        string picp;
        string tit;
        string pth;
        int cot;
        bool isc;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pic">图片Path</param>
        /// <param name="title">标题</param>
        /// <param name="pt">文件架Path</param>
        /// <param name="ct">合计数量</param>
        /// <param name="isCollection">是否为集合</param>
        public BGSTItem(string pic,string title,string pt,int ct,bool isCollection)
        {
            InitializeComponent();
            picp = pic;
            tit = title;
            pth = pt;
            cot = ct;
            isc = isCollection;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Img_1.Source = new BitmapImage(new Uri(picp));
            Tbk_1.Text = tit;
            Tbk_count.Text = cot.ToString();
            Tbk_path.Text = pth;
            if(isc) { Tbk_isCol.Visibility = Visibility.Visible; }
        }
    }
}
