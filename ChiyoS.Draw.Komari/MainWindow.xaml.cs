using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Threading;
using System.Linq;
using System.Windows.Media.Animation;
using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ChiyoS.Draw.Komari
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        RandomF randomF = new RandomF();
        RandomF2 randomF2 = new RandomF2();
        
        Root root = new Root();
        Root_bg root_bg = new Root_bg();
        System.Timers.Timer timer1 = new System.Timers.Timer();
        System.Timers.Timer timer_start = new System.Timers.Timer();
        System.Timers.Timer timer_xh = new System.Timers.Timer();
        System.Timers.Timer timer_bggd = new System.Timers.Timer();
        System.Timers.Timer timer_showwarn = new System.Timers.Timer();
        System.Timers.Timer timer_dynafreq = new System.Timers.Timer();
        int stid = 0;
        int xhn = 0;
        int gt = 0;
        int xhn2 = 0;
        int gt2 = 0;
        int bgidz = 0;
        int bgid = 0;
        int wshow = 4;
        int dfreq = 0;
        int startid = 1;
        int endid = 55;
        int[] dfreqs;
        bool isCanxh = false;

        ArrayList al = new ArrayList();
        ArrayList Everyone = new ArrayList();
        ArrayList boys = new ArrayList();
        ArrayList girls = new ArrayList();
        ArrayList seletlist = new ArrayList();
        public MainWindow()
        {
            InitializeComponent();          
        }

        private void MWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer1.Interval = 13;
            timer1.Enabled = false;
            timer1.Elapsed += XHNum;
            timer_xh.Interval = 13;
            timer_xh.Enabled = false;
            timer_xh.Elapsed += XHName;
            timer_showwarn.Interval = 1000;
            timer_showwarn.Enabled = false;
            timer_showwarn.Elapsed += Showwarn;
            timer_start.Interval = 1000;
            timer_start.Enabled = true;
            timer_start.Elapsed += StartDelay;
            timer_start.Start();
            timer_dynafreq.Interval = 200;
            timer_dynafreq.Enabled = false;
            timer_dynafreq.Elapsed += Dynafreq;
            Task.Run(new Action(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    string str;
                    try
                    {
                        //加载学号表
                        str = File.ReadAllText(@"D:\ChiyoS\Data\students.json");
                        root = JsonConvert.DeserializeObject<Root>(str);
                    }
                    catch (Exception ex)
                    {
                        timer_start.Stop();
                        timer_start.Enabled = false;
                        Growl.Error("加载Json数据失败！");
                        Btn_PZ_DrawS.IsEnabled = false;
                        Btn_PZ_ToRST.IsEnabled = false;
                        Btn_Enter.IsEnabled = true;
                        Btn_Enter.Visibility = Visibility.Visible;
                        Tbk_PZ_Info.Visibility = Visibility.Visible;
                        Tbk_wel.Text = "少女祈祷失败";
                        Tbk_wel2.Text = "加载Json数据失败！";
                        Pgb_wel.Visibility = Visibility.Hidden;
                        HandyControl.Controls.MessageBox.Error(ex.Message, "错误");
                        return;
                    }

                    Nud_cqrs.Maximum = root.students.Count;
                    Nud_endid.Maximum = root.students.Count;
                    try
                    {
                        foreach (Students itm in root.students)
                        {
                            Everyone.Add(itm.name);
                            if (itm.s == "b")
                            {
                                boys.Add(itm.name);
                            }
                            else
                            {
                                girls.Add(itm.name);
                            }
                        }
                    }
                    catch
                    {
                    }
                    /**
                    try
                    {
                        str = File.ReadAllText(@"D:\ChiyoS\Data\bg.json");
                        root_bg = JsonConvert.DeserializeObject<Root_bg>(str);
                        timer_bggd.Interval = root_bg.interval;
                        timer_bggd.Elapsed += Bggd;
                        timer_bggd.Enabled = true;
                        timer_bggd.Start();
                    }
                    catch
                    {
                        Growl.Warning("背景轮换列表bg.json加载失败");
                    }
                    **/
                    Growl.Info("当前载入:" + root.title);
                    Title = "ChiyoS.Draw.Komari|Version:2.9|当前载入配置文件:" + root.title;
                }));
            }));
        }
        private void StartDelay(object sender, ElapsedEventArgs e)
        {
            if (stid == 2)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    timer_start.Stop();
                    timer_start.Enabled = false;
                    Tbk_wel.Text = "Ciallo～(∠·ω< )⌒★";
                    Tbk_wel2.Visibility = Visibility.Hidden;
                    Pgb_wel.Visibility = Visibility.Hidden;
                    Btn_Enter.IsEnabled = true;
                    Btn_Enter.Visibility = Visibility.Visible;
                }));
            }
            else
            {
                stid++;
            }
        }
        /// <summary>
        /// 抽签-自动模式
        /// </summary>
        private void ChouQian_Auto()
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (Nud_stid.Value > Nud_endid.Value)
                    {
                        Growl.Warning("起始学号不可以大于终止学号！");
                        return;
                    }
                    Tbk_RST_Text.Text = "执行中";

                    Tbcl.SelectedIndex = 3;
                    Btn_RST_BacktoPZ.IsEnabled = false;
                    Btn_RST_Cancle.IsEnabled = false;
                    Btn_RST_Restart.IsEnabled = false;
                    Stp_RST_s1.Children.Clear();
                    //抽取的人数
                    int a = (int)Nud_cqrs.Value;

                    int[] arr = new int[] { };

                    if (Cbx_cqgt.SelectedIndex == 0)
                    {
                        if (Cbx_sf.SelectedIndex == 0)
                        {
                            arr = randomF2.GenerateUniqueRandom((int)Nud_stid.Value, (int)Nud_endid.Value, a);
                        }
                        else if (Cbx_sf.SelectedIndex == 1)
                        {
                            arr = randomF.GenerateUniqueRandom((int)Nud_stid.Value, (int)Nud_endid.Value, a);
                        }
                    }
                    else if (Cbx_cqgt.SelectedIndex == 1)//如果只抽取男孩子
                    {
                        if (Cbx_sf.SelectedIndex == 0)
                        {
                            arr = randomF2.GenerateUniqueRandom(1, boys.Count, a);
                        }
                        else if (Cbx_sf.SelectedIndex == 1)
                        {
                            arr = randomF.GenerateUniqueRandom(1, girls.Count, a);
                        }
                    }
                    else if (Cbx_cqgt.SelectedIndex == 2)//如果只抽取女孩子
                    {
                        if (Cbx_sf.SelectedIndex == 0)
                        {
                            arr = randomF2.GenerateUniqueRandom(1, girls.Count, a);
                        }
                        else if (Cbx_sf.SelectedIndex == 1)
                        {
                            arr = randomF.GenerateUniqueRandom(1, girls.Count, a);
                        }
                    }

                    Console.WriteLine("---[自动模式] 生成的随机数---");
                    Console.WriteLine(string.Join("  ", arr));
                    Console.WriteLine("----");
                    try
                    {
                        
                        
                        AddCh(arr,Cbx_cqgt.SelectedIndex);
                        Tbk_RST_Text.Text = "抽取完成";
                        Tbk_RST_Info.Text = "[自动模式] 任务结束";
                        Btn_RST_BacktoPZ.IsEnabled = true;
                        Growl.Success("[自动模式] 抽取完成");
                        Btn_RST_Restart.IsEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        Tbk_RST_Text.Text = "ERROR!";
                        Tbk_RST_Info.Text = "[自动模式] 错误！";
                        Growl.Error("[自动模式] 程序执行时遇到错误！");
                        HandyControl.Controls.MessageBox.Error(ex.Message, "错误");
                        Btn_RST_BacktoPZ.IsEnabled = true;
                        Btn_RST_Restart.IsEnabled = true;
                        return;
                    }

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
                Dispatcher.Invoke(() =>
                {
                    
                    isCanxh = true;//Unlock
                    Nud_cqrs.IsEnabled = false;
                    BtnG.IsEnabled = false;
                    Nud_interval.IsEnabled = false;
                    Stp_RST_s1.Children.Clear();
                    Btn_RST_Restart.IsEnabled = false;
                    Tbk_RST_Count.Text = string.Format("共 {0} 个", Stp_RST_s1.Children.Count);
                    Tbk_RST_Text.Text = "";

                    al.Clear();

                    Btn_RST_cq.Visibility = Visibility.Visible;
                    Btn_RST_cq.IsEnabled = true;
                    Btn_RST_Cancle.IsEnabled = true;
    
                    Tbk_RST_Info.Text = string.Format("[手动模式] 本次将抽取{0}位同学，请点击抽取按钮自主抽取！", Nud_cqrs.Value);
                    if (Rbtn_GDMZ.IsChecked==true)
                    {
                        xhn2 = 1;
                        gt2 = 0;
                        if(Cbx_cqgt.SelectedIndex == 0)
                        {
                            if (Cbx_isNosquence.IsChecked == true)
                            {
                                ArrayList tmp_arl = new ArrayList();
                                tmp_arl = Getname(randomF2.GenerateUniqueRandom(1, root.students.Count, root.students.Count), 0);
                                seletlist = tmp_arl;         
                                Showflowtext("乱序滚动列已启用");
                            }
                            else
                            {
                                seletlist = Everyone;
                            }
                            startid = (int)Nud_stid.Value;
                            endid = (int)Nud_endid.Value;
                        }
                        else if (Cbx_cqgt.SelectedIndex == 1)
                        {
                            seletlist = boys;
                            startid = 1;
                            endid = boys.Count;
                        }
                        else if (Cbx_cqgt.SelectedIndex == 2)
                        {
                            seletlist = girls;
                            startid = 1;
                            endid = girls.Count;
                        }
                        timer_xh.Enabled = true;
                        timer_xh.Interval = Nud_interval.Value;
                        timer_xh.Start();                       
                    }
                    else
                    {
                        
                        xhn = 1;
                        gt = 0;                
                        timer1.Enabled = true;
                        timer1.Interval = Nud_interval.Value;//循环频率
                        timer1.Start();               
                    }
                    if (Cbx_DynamicFrequency.IsChecked == true)
                    {
                        timer_dynafreq.Enabled = true;
                        dfreqs = randomF2.GenerateUniqueRandom(24, 45, 20);
                        timer_dynafreq.Enabled = true;
                        timer_dynafreq.Start();
                    }
                    else
                    {
                        timer_dynafreq.Enabled = false;
                    }
                });
            });
        }
        private void XHName(object sender, ElapsedEventArgs e)
        {
            if(isCanxh == false)
            {
                return;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                isCanxh = false;
                if (xhn2 <= endid)
                {
                    Tbk_RST_Text.Text = seletlist[xhn2-1].ToString();
                    xhn2++;
                }
                else
                {
                    xhn2 = startid;
                    Tbk_RST_Text.Text = seletlist[xhn2-1].ToString();
                }
                isCanxh = true;
            }));
        }


        
            private void XHNum(object sender, ElapsedEventArgs e)
        {
            if (isCanxh == false)
            {
                return;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                isCanxh = false;
                if (xhn <= (int)Nud_endid.Value)
                {
                    Tbk_RST_Text.Text = xhn.ToString();
                    xhn++;
                }
                else
                {
                    xhn = (int)Nud_stid.Value;
                    Tbk_RST_Text.Text = xhn.ToString();
                }
                isCanxh = true;
            }));

        }
        /// <summary>
        /// 呈现被抽中的同学
        /// </summary>
        /// <param name="arr">学号列表</param>
        /// <param name="mode">模式</param>
        private void AddCh(int[] arr,int mode)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (mode == 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (root.students[arr[i] - 1].s == "b")
                        {
                            Stp_RST_s1.Children.Add(new UserControl2(arr[i].ToString(), root.students[arr[i] - 1].name, (Stp_RST_s1.Children.Count + 1).ToString()));
                        }
                        else
                        {
                            Stp_RST_s1.Children.Add(new UserControl1(arr[i].ToString(), root.students[arr[i] - 1].name, (Stp_RST_s1.Children.Count + 1).ToString()));
                        }
                        Tbk_RST_Count.Text = string.Format("共 {0} 个", Stp_RST_s1.Children.Count);
                        double d = Scve_RST_s1.ActualWidth;
                        Scve_RST_s1.ScrollToHorizontalOffset(d);
                    }               
                }
                else if (mode == 1)
                {
                    for(int i = 0; i < arr.Length; i++)
                    {
                        Stp_RST_s1.Children.Add(new UserControl2("", boys[arr[i] - 1].ToString(), (Stp_RST_s1.Children.Count + 1).ToString()));
                    }
                }
                else if(mode == 2)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Stp_RST_s1.Children.Add(new UserControl1("", girls[arr[i] - 1].ToString(), (Stp_RST_s1.Children.Count + 1).ToString()));
                    }
                }
            }));
        }

        private void Btn_RST_cq_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rbtn_GDXH.IsChecked == true)
            {
                if (gt < Nud_cqrs.Value)
                {
                    int s1 = int.Parse(Tbk_RST_Text.Text);
                    Console.Write("抽取到TARGET {0} 号，检查正确性...", s1);
                    if (s1 < (int)Nud_stid.Value || s1 > (int)Nud_endid.Value)
                    {
                        Growl.Warning("抽取TARGET命中限定范围之外，操作无效！\n请继续抽取！");
                        Console.WriteLine("[TARGET {0} 号 命中限定范围之外][执行操作：作废]", s1);
                        Tbk_RST_Info.Text = string.Format("[TARGET {0} 号 命中限定范围之外] 程序已将此操作结果撤销，请继续！", s1);
                        return;
                    }
                    foreach (int i in al)
                    {
                        if (i == s1)
                        {
                            Growl.Warning("抽取TARGET有重复，操作无效！\n请继续抽取！");
                            Console.WriteLine("[TARGET {0} 号 重复][执行操作：撤销]", s1);
                            Tbk_RST_Info.Text = string.Format("[{0} 号 已重复] 程序已将此操作结果撤销，请继续！", s1);
                            return;
                        }
                    }
                    Console.WriteLine("[PASS]");
                    Tbk_RST_Info.Text = string.Format("[未重复] 请继续，还剩 {0} 个", Nud_cqrs.Value - gt - 1);
                    al.Add(s1);
                    int[] ar = new int[] { s1 };
                    AddCh(ar, 0);
                    gt++;

                }
                if (gt >= Nud_cqrs.Value)
                {
                    isCanxh = false;
                    timer_dynafreq.Stop();
                    StopMualTask("[手动模式] 抽取完成!(如果上方有学号请忽略)", "抽取完成");
                    
                }
            }
            else if(Rbtn_GDMZ.IsChecked == true)
            {
                if (gt2 < Nud_cqrs.Value)
                {
                    string czgt = Tbk_RST_Text.Text;
                    int czgtid = Everyone.IndexOf(czgt) + 1;
                    Console.Write("抽取到TARGET {0} ，检查正确性...", czgt);
                    if (czgtid < (int)Nud_stid.Value||czgtid>(int)Nud_endid.Value)
                    {
                        Growl.Warning("抽取TARGET命中限定范围之外，操作无效！\n请继续抽取！");
                        Console.WriteLine("[TARGET {0}  命中限定范围之外][执行操作：作废]", czgt);
                        Tbk_RST_Info.Text = string.Format("[TARGET{0}  命中限定范围之外] 程序已将此操作结果撤销，请继续！", czgt);
                        return;
                    }
                    foreach(string sr in al)
                    {
                        
                        if(czgt == sr)
                        {
                            Growl.Warning("抽取TARGET有重复，操作无效！\n请继续抽取！");
                            Console.WriteLine("[TARGET {0}  重复][执行操作：撤销操作]", czgt);
                            Tbk_RST_Info.Text = string.Format("[TARGET{0}  已重复] 程序已将此操作结果撤销，请继续！", czgt);
                            return;
                        }
                    }
                    Console.WriteLine("[PASS]");
                    Tbk_RST_Info.Text = string.Format("[TARGET未重复] 请继续，还剩 {0} 个", Nud_cqrs.Value - gt2 - 1);
                    al.Add(czgt);
                    int[] ar = new int[] { czgtid };
                    AddCh(ar, 0);
                    gt2++;
                }
                if(gt2>= Nud_cqrs.Value)
                {
                    isCanxh = false;
                    timer_dynafreq.Stop();
                    StopMualTask("[手动模式] 抽取完成!(如果上方有名字请忽略)", "抽取完成");
                    
                }
            }
        }

        private void StopMualTask(string Infotext, string TText)
        {
            isCanxh = false;
            if(Rbtn_GDXH.IsChecked == true)
            {
                timer1.Stop();
                timer1.Enabled = false;
            }
            else
            {
                timer_xh.Stop();
                timer_xh.Enabled = false;
            }
            if(Cbx_DynamicFrequency.IsChecked== true)
            {
                timer_dynafreq.Stop();
                timer_dynafreq.Enabled = false;
            }
            Nud_cqrs.IsEnabled = true;
            Nud_interval.IsEnabled = true;
            BtnG.IsEnabled = true;
            Btn_RST_Cancle.IsEnabled = false;
            Btn_RST_cq.IsEnabled = false;
            Btn_RST_BacktoPZ.IsEnabled = true;
            Btn_RST_Restart.IsEnabled = true;
            Tbk_RST_Info.Text = Infotext;
            Tbk_RST_Text.Text = TText;
        }

        private void Btn_PZ_DrawS_Click(object sender, RoutedEventArgs e)
        {
            if (Rbtn_RAuto.IsChecked == true)
            {
                ChouQian_Auto();
            }
            else if (Rbtn_RManual.IsChecked == true)
            {
                if (Rbtn_GDXH.IsChecked == true) 
                { 
                    if (Cbx_cqgt_all.IsSelected != true) 
                    { 
                        Growl.Warning("滚动学号模式下只能抽取全部个体！"); 
                        return; 
                    }
                }
                isCanxh = true;
                if (Cbx_isNosquence.IsChecked == false && Cbx_DynamicFrequency.IsChecked == false)
                {
                    Tbcl.SelectedIndex = 4;
                    timer_showwarn.Enabled = true;
                    timer_showwarn.Start();
                    ChouQian_Manual();
                }
                else
                {
                    Tbcl.SelectedIndex = 3;
                    ChouQian_Manual();
                }

            }
        }

        private void MWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                timer1.Stop();
                timer1.Enabled = false;
            }
            catch
            {
            }
        }

        private void Btn_RST_Cancle_Click(object sender, RoutedEventArgs e)
        {
            StopMualTask("[手动模式] 已终止Task", "已终止！");
        }

        private void Btn_RST_ScvLeft_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Scve_RST_s1.ScrollToHorizontalOffsetWithAnimation(Scve_RST_s1.HorizontalOffset - 155);
            }));
        }

        private void Btn_RST_ScvRight_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Scve_RST_s1.ScrollToHorizontalOffsetWithAnimation(Scve_RST_s1.HorizontalOffset + 155);
            }));
        }
       
        private void Btn_Enter_Click(object sender, RoutedEventArgs e)
        {
            Btn_Enter.IsEnabled = false;
            Storyboard storyboard = (FindResource("Entermp") as Storyboard);
            Storyboard storyboard2 = (FindResource("Entermp2") as Storyboard);
            storyboard.Completed += (o, a) => { Tbcl.SelectedIndex = 2; storyboard2.Begin(Gd_PZ); };//去配置页//
            storyboard.Begin();
            
        }

        private void Btn_PZ_ToRST_Click(object sender, RoutedEventArgs e)
        {
            Tbcl.SelectedIndex = 3;
            //去结果页
        }

        private void Btn_RST_BacktoPZ_Click(object sender, RoutedEventArgs e)
        {
            Tbcl.SelectedIndex = 2;
            //去配置页
        }

        /// <summary>
        /// 按钮组选择 自动模式 的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rbtn_RAuto_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult vr = HandyControl.Controls.MessageBox.Show("自动模式下的概率并不平均哦，是否切换为手动模式？", "不推荐的操作", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (vr == MessageBoxResult.Yes) // 如果是确定，就执行下面代码
            {
                Rbtn_RManual.IsChecked = true;
            }
            
        }/**
        /// <summary>
        /// 按钮组选择 手动模式 的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rbtn_RManual_Click_1(object sender, RoutedEventArgs e)
        {
            if (Cbx_cqgt.SelectedIndex == 0)
            {
                BtnG_2.IsEnabled = true;
            }
            Nud_interval.IsEnabled = true;
            Cbx_sf.IsEnabled = false;
        }
        **/

        /**
        private void Cbx_cqgt_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (IsLoaded == false)
            {
                //如果界面没加载完就不触发，防止在 Init 结束之前操作控件
                return;
            }
            if (Cbx_cqgt.SelectedIndex == 0)
            {
                Nud_stid.IsEnabled = true;
                Nud_endid.IsEnabled = true;
                Nud_cqrs.Maximum = root.students.Count;
                BtnG_2.IsEnabled = true;
            }
            else if (Cbx_cqgt.SelectedIndex == 1)
            {
                Nud_stid.IsEnabled = false;
                Nud_endid.IsEnabled = false;
                Nud_cqrs.Maximum = boys.Count;
                BtnG_2.IsEnabled = false;
                Rbtn_GDMZ.IsChecked = true;
            }
            else if (Cbx_cqgt.SelectedIndex == 2)
            {
                Nud_stid.IsEnabled = false;
                Nud_endid.IsEnabled = false;
                Nud_cqrs.Maximum = girls.Count;
                BtnG_2.IsEnabled = false;
                Rbtn_GDMZ.IsChecked = true;
            }
        }
        **/
        
        private void Showwarn(object sender, ElapsedEventArgs e)
        {
            try
            {
                
                Dispatcher.Invoke(new Action(() =>
                {
                    if (wshow >= 0)
                    {
                        Otxt_Warn_down.Text = wshow.ToString();
                        wshow--;
                    }
                    else
                    {
                        if (Cbx_DynamicFrequency.IsChecked == true)
                        {
                            Showflowtext("动态频率已开启");
                        }
                        timer_showwarn.Stop();
                        Tbcl.SelectedIndex = 3;   
                    }
                    
                }));
            }
            catch
            {

            }
        }
        private void Bggd(object sender, ElapsedEventArgs e) 
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (bgidz == 0)
                    {
                        bgidz = 1;
                        if (bgid <= root_bg.list.Count - 1)
                        {                           
                            Img_bg2.Source = new BitmapImage(new Uri(root_bg.list[bgid].path));
                        }
                        Storyboard Stbd_Opacity1 = (FindResource("Opacity1") as Storyboard);
                        //Stbd_Opacity4.Completed += (o, a) => {Stbd_Opacity3.Begin(Tbk_bgif) };          
                        Stbd_Opacity1.Begin(Img_bg2);
                    }
                    else
                    {
                        bgidz = 0;
                        if (bgid <= root_bg.list.Count - 1)
                        {                         
                            Img_bg1.Source = new BitmapImage(new Uri(root_bg.list[bgid].path));
                        }
                        Storyboard Stbd_Opacity2 = (FindResource("Opacity2") as Storyboard);
                        //Stbd_Opacity4.Completed += (o, a) => {Stbd_Opacity3.Begin(Tbk_bgif) };          
                        Stbd_Opacity2.Begin(Img_bg2);
                    }
                    Tbk_wel_bgif.Text = root_bg.list[bgid].text;
                    Tbk_PZ_bgif.Text = root_bg.list[bgid].text;
                    var col = new SolidColorBrush((Color)ColorConverter.ConvertFromString(root_bg.list[bgid].color));

                    Tbk_RST_Count.Foreground = col;
                    Tbk_RST_Info.Foreground = col;
                    Tbk_RST_Text.Foreground = col;

                    if (bgid == root_bg.list.Count - 1)
                    {
                        bgid = 0;
                    }
                    else
                    {
                        bgid++;
                    }
                }));
                
            }
            catch(Exception ex)
            {
                Growl.Warning("BGGD:" + ex.Message);
                if (bgid == root_bg.list.Count - 1)
                {
                    bgid = 0;
                }
                else
                {
                    bgid++;
                }
            }

            
        }

        private void Dynafreq(object sender, ElapsedEventArgs e)
        {
            if(dfreq> dfreqs.Length - 1)
            {
                dfreq = 0;
            }
            else
            {
                if (Rbtn_GDMZ.IsChecked == true)
                {
                    timer_xh.Interval = dfreqs[dfreq];
                }
                else
                {
                    timer1.Interval = dfreqs[dfreq];
                }
                dfreq++;
            }    
        }

        private ArrayList Getname(int[] nm, int tid)
        {
            ArrayList names = new ArrayList();
            if (tid == 0)
            {
                foreach (int id in nm)
                {
                    names.Add(root.students[id - 1].name);
                }
                return names;
            }
            else if (tid == 1)
            {
                foreach (int id in nm)
                {
                    names.Add(boys[id - 1].ToString());
                }
                return names;
            }
            else if (tid == 2)
            {
                foreach (int id in nm)
                {
                    names.Add(girls[id - 1].ToString());
                }
                return names;
            }
            return names;


        }
        public void Showflowtext(string txt)
        {
            Task.Run(() =>{
                Dispatcher.Invoke(new Action(() =>
                {
                    Tbk_flowtext.Text = txt;
                    Storyboard storyboard = (FindResource("Opacity1") as Storyboard);
                    Storyboard storyboard2 = (FindResource("Opacity2") as Storyboard);
                    storyboard.Completed += (o, a) => { storyboard2.Begin(Tbk_flowtext); };
                    storyboard.Begin(Tbk_flowtext);
                }));
            });
        }

        private void Btn_RST_Restart_Click(object sender, RoutedEventArgs e)
        {
            Btn_PZ_DrawS_Click(sender, e);
        }
    }
}