using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// ResultC_Pink.xaml 的交互逻辑
    /// </summary>
    public partial class ResultC_Pink : UserControl
    {
        public ResultC_Pink(string id, string name, string num)
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
