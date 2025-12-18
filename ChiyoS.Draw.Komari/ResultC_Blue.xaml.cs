using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// ResultC_Blue.xaml 的交互逻辑
    /// </summary>
    public partial class ResultC_Blue : UserControl
    {
        public ResultC_Blue(string id, string name, string num)
        {
            InitializeComponent();
            this.Tbk_Name.Text = name;
            this.Tbk_ID.Text = id;
            this.Tbk_Num.Text = num;
            Storyboard storyboard = FindResource("Stbd_1") as Storyboard;
            storyboard.Begin(Bd_1);
        }
        
    }
}
