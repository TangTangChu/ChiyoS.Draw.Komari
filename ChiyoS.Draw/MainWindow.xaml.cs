using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using HandyControl.Controls;
using System.Collections;
using System.Timers;
namespace ChiyoS.Draw
{
/// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        RandomF randomF = new RandomF();
        Root root = new Root();
        System.Timers.Timer timer1 = new System.Timers.Timer();
        int xhn = 0;
        int gt = 0;
        ArrayList al = new ArrayList();
        
        public MainWindow()
        {
            InitializeComponent();

            timer1.Interval = 20;
            timer1.Enabled = false;
            timer1.Elapsed += XHNum;
            _ = Task.Run(new Action(() =>
              {
                  this.Dispatcher.Invoke(new Action(() =>
                  {
                      string str;
                      try
                      {
                          str = File.ReadAllText(@"D:\ChiyoS\Data\students.json");
                          root = JsonConvert.DeserializeObject<Root>(str);
                      }
                      catch (Exception ex)
                      {
                          this.Tbk_Info.Text = "加载Json数据时出错，请确认Json文件正常！";
                          this.Tbk_Num.Text = "程序错误";
                          Growl.Error("加载Json数据失败！");
                          this.Ldc_l1.Visibility = Visibility.Hidden;
                          //Im.Opacity = 0.8;
                          HandyControl.Controls.MessageBox.Error(ex.Message, "错误");                                                             
                          return;
                      }                     
                      this.Ldc_l1.Visibility = Visibility.Hidden;
                      this.Tbk_Info.Text = "就绪";
                      this.Btn_DrawS.IsEnabled = true;
                      this.Tbk_Num.Text = "就绪";
                      this.Nud_n1.Maximum = root.students.Count;
                      //HandyControl.Controls.MessageBox.Show("当前载入 "+root.title);
                      //Dialog.Show("当前载入 " + root.title);
                      Growl.Success("程序已就绪");
                      Growl.Info("当前载入 " + root.title);
                  }));
              }));
        }
        /// <summary>
        /// 抽签-自动模式
        /// </summary>
        private void ChouQian_Auto() 
        {
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Btn_DrawS.IsEnabled = false;
                    this.Stp_s1.Children.Clear();
                    this.Ldc_l1.Visibility = Visibility.Visible;
                    this.Btn_DrawS.IsEnabled = false;
                    this.Tbk_Info.Text = "执行中...";
                    this.Tbk_Num.Text = "随机数生成中...";
                    int a = (int)Nud_n1.Value;
                    int[] arr = randomF.GenerateUniqueRandom(1, root.students.Count, a);
                    Console.WriteLine("---[自动模式] 生成的随机数---");
                    Console.WriteLine(string.Join("\n", arr));

                    try
                    {
                        AddCh(arr);
                        //this.Tbk_C.Text = string.Format("共 {0} 个",arr.Length);
                        this.Tbk_Num.Text = "抽取完成";
                        this.Tbk_Info.Text = "[自动模式] 任务结束";
                        this.Ldc_l1.Visibility = Visibility.Hidden;
                        this.Btn_DrawS.IsEnabled = true;
                        SetBtnDS(1);
                        
                        
                        Growl.Success("[自动模式] 抽取完成");
                    }
                    catch (Exception ex)
                    {
                        this.Tbk_Info.Text = "An error has occurred, please check!";
                        this.Tbk_Num.Text = "[自动模式] 错误！";
                        //this.Btn_Restart.IsEnabled = true;
                        Growl.Error("[自动模式] 程序执行时遇到错误！");
                        this.Ldc_l1.Visibility = Visibility.Hidden;
                        this.Btn_DrawS.IsEnabled = true;
                        SetBtnDS(1);
                        HandyControl.Controls.MessageBox.Error(ex.Message, "错误");
                        return;
                    }
                    //this.Btn_Restart.IsEnabled = true;
                }));
            });
        }
        /// <summary>
        /// 抽签-手动模式
        /// </summary>
        private void ChouQian_Manual()
        {
            Task.Run(() =>
              {
                  this.Dispatcher.Invoke(() =>
                  {
                      this.Nud_n1.IsEnabled = false;
                      this.BtnG.IsEnabled = false;
                      this.Btn_DrawS.IsEnabled = false;
                      this.Nud_n2.IsEnabled = false;                     
                      this.Ldc_l1.Visibility = Visibility.Visible;
                      this.Stp_s1.Children.Clear();
                      this.Tbk_C.Text = string.Format("共 {0} 个", Stp_s1.Children.Count);
                      this.Tbk_Num.Text = "";
                      this.Btn_DrawS.Click -= Btn_DrawS_Click;
                      this.Btn_DrawS.Click += Btn_DrawS_OClick;
                      this.Tbk_C.Text = "";

                      SetBtnDS(2);
                      xhn = 1;
                      gt = 0;
                      
                      timer1.Enabled = true;
                      timer1.Interval = Nud_n2.Value;
                      timer1.Start();
                      al.Clear();
                      this.Btn_DrawS.IsEnabled = true;
                      this.Btn_Cancle.IsEnabled = true;
                      this.Tbk_Info.Text = string.Format("[手动模式] 本次将抽取{0}位，请点击抽取按钮自主抽取！", this.Nud_n1.Value);

                  });
              });
        }
              
        private void XHNum(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (xhn <= root.students.Count)
                {
                    this.Tbk_Num.Text = xhn.ToString();
                    xhn++;
                }
                else
                {
                    xhn = 1;
                    this.Tbk_Num.Text = xhn.ToString();
                }
            }));
            
        }

        private void AddCh(int[] arr)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    string num = (i + 1).ToString();
                    
                    if (root.students[arr[i] - 1].s == "b")
                    {
                       
                        Stp_s1.Children.Add(new UserControl2(arr[i].ToString(), root.students[arr[i] - 1].name, (Stp_s1.Children.Count+1).ToString()));
                    }
                    else
                    {
                        Stp_s1.Children.Add(new UserControl1(arr[i].ToString(), root.students[arr[i] - 1].name, (Stp_s1.Children.Count + 1).ToString()));
                    }
                    this.Tbk_C.Text = string.Format("共 {0} 个", Stp_s1.Children.Count);
                    double d = Sve_s1.ActualWidth;
                    Sve_s1.ScrollToHorizontalOffset(d);
                    
                }
            }));
        }

        private void Btn_DrawS_OClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (gt < this.Nud_n1.Value)
            {
                int s1 = int.Parse(Tbk_Num.Text);
                Console.Write("抽取到目标 {0} 号，查重中...",s1);
                foreach(int i in al)
                {
                    if (i == s1)
                    {
                        Growl.Warning("抽取目标有重复，已舍弃！\n请继续抽取！");
                        Console.WriteLine("[目标 {0} 号 重复][执行操作：舍弃]",s1);
                        this.Tbk_Info.Text = string.Format("[目标 {0} 号 已重复] [执行操作：舍弃]", s1);
                        return;
                    }
                }
                Console.WriteLine("[未重复]");
                this.Tbk_Info.Text = string.Format("[目标 {0} 号 检验成功]，还剩 {1} 个", s1,Nud_n1.Value-gt-1);
                al.Add(s1);
                int[] ar = new int[] { s1 };
                AddCh(ar);
                gt++;
                
            }
            if(gt >= Nud_n1.Value)
            {
                StopMualTask("[手动模式] 抽取完成!","抽取完成");
                Ldc_l1.Visibility = Visibility.Hidden;
            }
        }

        private void StopMualTask(string it,string nmt)
        {
            timer1.Stop();
            timer1.Enabled = false;
            this.Btn_DrawS.IsEnabled = false;
            this.Nud_n1.IsEnabled = true;
            this.Nud_n2.IsEnabled = true;
            this.BtnG.IsEnabled = true;
            this.Tbk_Info.Text = it;
            this.Tbk_Num.Text = nmt;
            this.Btn_DrawS.Click -= Btn_DrawS_OClick;
            this.Btn_DrawS.Click += Btn_DrawS_Click;
            this.Btn_DrawS.IsEnabled = true;
            this.Btn_Cancle.IsEnabled = false;
            this.Ldc_l1.Visibility = Visibility.Hidden;
            SetBtnDS(1);
        }

        private void Btn_DrawS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rbtn_R1.IsChecked == true)
            {
                ChouQian_Auto();
            }
            else if(Rbtn_R2.IsChecked == true)
            {
                ChouQian_Manual();
            }
            else
            {

            }
        }
        private void Rbtn_R1_Click(object sender, RoutedEventArgs e)
        {
            SetBtnDS(0);
            //Im.Opacity = 0.9;
            this.Nud_n2.Visibility = Visibility.Hidden;
            this.Btn_Cancle.Visibility = Visibility.Hidden;
        }

        private void Rbtn_R2_Click(object sender, RoutedEventArgs e)
        {
            SetBtnDS(0);
            //Im.Opacity = 0.82;
            this.Nud_n2.Visibility = Visibility.Visible;
            this.Btn_Cancle.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 设置 DrawS按钮样式
        /// </summary>
        /// <param name="b">0表示初始状态（开始），1表示“重新开始”，2表示“抽取”</param>
        private void SetBtnDS(int b)
        {
            if (b == 0)
            {
                Btn_DrawS.Content = "\ue62f 开始";
                Btn_DrawS.Style = (Style)this.FindResource("ButtonSuccess");

            }
            else if (b == 1)
            {
                Btn_DrawS.Content = "\ue65b 重新开始";
                Btn_DrawS.Style = (Style)this.FindResource("ButtonPrimary");
            }
            else if (b == 2)
            {
                Btn_DrawS.Content = "\ue650 选取";
                Btn_DrawS.Style = (Style)this.FindResource("ButtonInfo");
            }
        }

        private void MWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.timer1.Stop();
                this.timer1.Enabled = false;
            }
            catch
            {

            }
            
        }

        private void Btn_Cancle_Click(object sender, RoutedEventArgs e)
        {
            StopMualTask("[手动模式] 已终止Task","已终止！");
            Ldc_l1.Visibility = Visibility.Hidden;
            
        }

        private void Btn_ScvLeft_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                 Sve_s1.ScrollToHorizontalOffsetWithAnimation(Sve_s1.HorizontalOffset - 105);
            }));
        }

        private void Btn_ScvRight_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                  Sve_s1.ScrollToHorizontalOffsetWithAnimation(Sve_s1.HorizontalOffset + 105);
            }));
        }
    }
}