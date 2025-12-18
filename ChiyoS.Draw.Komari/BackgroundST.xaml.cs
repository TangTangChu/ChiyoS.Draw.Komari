using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using HandyControl.Controls;
namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// BackgroundST.xaml 的交互逻辑
    /// </summary>
    public partial class BackgroundST : HandyControl.Controls.Window
    {
        string bgdict;
        ArrayList bgdics = new ArrayList();

        public BackgroundST(string bgp)
        {
            InitializeComponent();
            try
            {
                bgdict = bgp;
                string[] infs =  Directory.GetDirectories(bgdict);
                Console.WriteLine(string.Join("\n",infs));
                foreach (string file in infs)
                {
                    if (File.Exists(file + @"\bginfo.json"))
                    {
                        bgdics.Add(file);
                    }
                }
            }
            catch
            {
                Growl.Error("初始化失败");
            }
        }

        private void Btn_view_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_ApplyBg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(string file in bgdics) 
            {
                try
                {
                    IList<string> imgs = new List<string>();
                    string[] fls = Directory.GetFiles(file);
                    Console.WriteLine("fls:");
                    Console.WriteLine(string.Join("\n", fls));
                    foreach (string f in fls)
                    {
                        if (f.EndsWith(".jpg")|| f.EndsWith(".png")| f.EndsWith(".jpeg")| f.EndsWith(".gif")| f.EndsWith(".apng"))
                        {
                            Console.WriteLine("added:" + f);
                            imgs.Add(f);
                            
                        }
                    }
                    if (imgs != null)
                    {
                        if (imgs.Count() > 1)
                        {
                            Ltv_1.Items.Add(new BGSTItem(imgs[0],file.Substring(bgdict.Length + 1),file,imgs.Count, true));
                        }
                        else
                        {
                            Ltv_1.Items.Add(new BGSTItem(imgs[0], file.Substring(bgdict.Length + 1),file, imgs.Count,false));
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                catch
                {

                }
                
                
                
            }
        }
    }
}
