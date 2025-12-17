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
        Root_wtl root_Wtl = new Root_wtl();
        System.Timers.Timer timer1 = new System.Timers.Timer();
        System.Timers.Timer timer_start = new System.Timers.Timer();
        System.Timers.Timer timer_xh = new System.Timers.Timer();

        int stid = 0;
        int xhn = 0;
        int gt = 0;
        int xhn2 = 0;
        int gt2 = 0;
        bool isCanxh = false;
        bool isewtl = false;

        ArrayList al = new ArrayList();
        ArrayList Everyone = new ArrayList();
        ArrayList boys = new ArrayList();
        ArrayList Everyone_f = new ArrayList();
        ArrayList boys_f = new ArrayList();
        ArrayList girls = new ArrayList();
        ArrayList seletlist = new ArrayList();
        ArrayList wtlist = new ArrayList();
        int[] tzya = new int[1];
        int[] tzyb = new int[1];
        public MainWindow()
        {
            InitializeComponent();          
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
                    Tbk_M_Text.Text = "执行中";

                    Tbcl.SelectedIndex = 1;
                    Btn_M_BacktoPG.IsEnabled = false;
                    Btn_M_Cancle.IsEnabled = false;
                    Btn_M_Restart.IsEnabled = false;
                    Stp_M_s1.Children.Clear();
                    //抽取的人数
                    int a = (int)Nud_n1.Value;

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
                        int[] nst = arr;
                        if (isewtl)
                        {
                            nst = STFilter(arr, Cbx_cqgt.SelectedIndex);
                        }
                        Console.WriteLine(string.Join("  ", nst));
                        
                        AddCh(nst,Cbx_cqgt.SelectedIndex);
                        Tbk_M_Text.Text = "抽取完成";
                        Tbk_M_Info.Text = "[自动模式] 任务结束";
                        Btn_M_BacktoPG.IsEnabled = true;
                        Growl.Success("[自动模式] 抽取完成");
                        Btn_M_Restart.IsEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        Tbk_M_Text.Text = "ERROR!";
                        Tbk_M_Info.Text = "[自动模式] 错误！";
                        Growl.Error("[自动模式] 程序执行时遇到错误！");
                        HandyControl.Controls.MessageBox.Error(ex.Message, "错误");
                        Btn_M_BacktoPG.IsEnabled = true;
                        Btn_M_Restart.IsEnabled = true;
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
                    isCanxh = true;
                    Nud_n1.IsEnabled = false;
                    BtnG.IsEnabled = false;
                    Nud_n2.IsEnabled = false;
                    Stp_M_s1.Children.Clear();
                    Btn_M_Restart.IsEnabled = false;
                    Tbk_M_Count.Text = string.Format("共 {0} 个", Stp_M_s1.Children.Count);
                    Tbk_M_Text.Text = "";

                    al.Clear();

                    Btn_M_cq.Visibility = Visibility.Visible;
                    Btn_M_cq.IsEnabled = true;
                    Btn_M_Cancle.IsEnabled = true;
                    Tbcl.SelectedIndex = 1;
                    
                    Tbk_M_Info.Text = string.Format("[手动模式] 本次将抽取{0}位同学，请点击抽取按钮自主抽取！", Nud_n1.Value);
                    if (Rbtn_R3.IsChecked==true)
                    {
                        xhn2 = 1;
                        gt2 = 0;
                        if(Cbx_cqgt.SelectedIndex == 0)
                        {
                            seletlist = Everyone_f;
                        }
                        else if (Cbx_cqgt.SelectedIndex == 1)
                        {
                            seletlist = boys_f;
                        }
                        else if (Cbx_cqgt.SelectedIndex == 2)
                        {
                            seletlist = girls;
                        }
                        timer_xh.Enabled = true;
                        timer_xh.Interval = Nud_n2.Value;
                        timer_xh.Start();
                        
                    }
                    else
                    {
                        xhn = 1;
                        gt = 0;
                        timer1.Enabled = true;
                        timer1.Interval = Nud_n2.Value;//循环频率
                        timer1.Start();
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
                if (xhn2 <= seletlist.Count)
                {
                    Tbk_M_Text.Text = seletlist[xhn2-1].ToString();
                    xhn2++;
                }
                else
                {
                    xhn2 = 1;
                    Tbk_M_Text.Text = seletlist[xhn2-1].ToString();
                }
            }));
        }


        private void ST(object sender, ElapsedEventArgs e)
        {
            if (stid == 2)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    timer_start.Stop();
                    timer_start.Enabled = false;
                    Tbk_wel.Text = "欢迎回来！";
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
            private void XHNum(object sender, ElapsedEventArgs e)
        {
            if (isCanxh == false)
            {
                return;
            }
            Dispatcher.Invoke(new Action(() =>
            {
                if (xhn <= (int)Nud_endid.Value)
                {
                    Tbk_M_Text.Text = xhn.ToString();
                    xhn++;
                }
                else
                {
                    xhn = (int)Nud_stid.Value;
                    Tbk_M_Text.Text = xhn.ToString();
                }
            }));

        }
        /// <summary>
        /// 展示被抽中的同学
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
                        //string num = (i + 1).ToString();

                        if (root.students[arr[i] - 1].s == "b")
                        {

                            Stp_M_s1.Children.Add(new UserControl2(arr[i].ToString(), root.students[arr[i] - 1].name, (Stp_M_s1.Children.Count + 1).ToString()));
                        }
                        else
                        {
                            Stp_M_s1.Children.Add(new UserControl1(arr[i].ToString(), root.students[arr[i] - 1].name, (Stp_M_s1.Children.Count + 1).ToString()));
                        }
                        Tbk_M_Count.Text = string.Format("共 {0} 个", Stp_M_s1.Children.Count);
                        double d = Sve_M_s1.ActualWidth;
                        Sve_M_s1.ScrollToHorizontalOffset(d);
                    }               
                }
                else if (mode == 1)
                {
                    for(int i = 0; i < arr.Length; i++)
                    {
                        Stp_M_s1.Children.Add(new UserControl2("", boys[arr[i] - 1].ToString(), (Stp_M_s1.Children.Count + 1).ToString()));
                    }
                }
                else if(mode == 2)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        Stp_M_s1.Children.Add(new UserControl1("", girls[arr[i] - 1].ToString(), (Stp_M_s1.Children.Count + 1).ToString()));
                    }
                }
            }));
        }

        private void Btn_M_cq_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rbtn_R4.IsChecked == true)
            {
                if (gt < Nud_n1.Value)
                {
                    int s1 = int.Parse(Tbk_M_Text.Text);
                    Console.Write("抽取到TARGET {0} 号，查重中...", s1);
                    foreach (int i in al)
                    {
                        if (i == s1)
                        {
                            Growl.Warning("抽取TARGET有重复，操作无效！\n请继续抽取！");
                            Console.WriteLine("[TARGET {0} 号 重复][执行操作：撤销]", s1);
                            Tbk_M_Info.Text = string.Format("[{0} 号 已重复] 程序已将此操作结果撤销，请继续！", s1);
                            return;
                        }
                    }
                    Console.WriteLine("[未重复]");
                    Tbk_M_Info.Text = string.Format("[未重复] 请继续，还剩 {0} 个", Nud_n1.Value - gt - 1);
                    al.Add(s1);
                    int[] ar = new int[] { s1 };
                    AddCh(ar, 0);
                    gt++;

                }
                if (gt >= Nud_n1.Value)
                {
                    StopMualTask("[手动模式] 抽取完成!(如果上方有名字请忽略)", "抽取完成");
                }
            }
            else if(Rbtn_R3.IsChecked == true)
            {
                if (gt2 < Nud_n1.Value)
                {
                    string czgt = Tbk_M_Text.Text;
                    Console.Write("抽取到TARGET {0} ，查重中...", czgt);
                    foreach(string sr in al)
                    {
                        if(czgt == sr)
                        {
                            Growl.Warning("抽取TARGET有重复，操作无效！\n请继续抽取！");
                            Console.WriteLine("[TARGET {0}  重复][执行操作：撤销操作]", czgt);
                            Tbk_M_Info.Text = string.Format("[TARGET{0}号  已重复] 程序已将此操作结果撤销，请继续！", czgt);
                            return;
                        }
                    }
                    Console.WriteLine("[未重复]");
                    Tbk_M_Info.Text = string.Format("[TARGET未重复] 请继续，还剩 {0} 个", Nud_n1.Value - gt - 1);
                    al.Add(czgt);
                    int[] ar = new int[] { Everyone.IndexOf(czgt)+1 };
                    AddCh(ar, 0);
                    gt2++;
                }
                if(gt2>= Nud_n1.Value)
                {
                    StopMualTask("[手动模式] 抽取完成!(如果上方有名字请忽略)", "抽取完成");
                }
            }
        }

        private void StopMualTask(string Infotext, string TText)
        {
            isCanxh = false;
            if(Rbtn_R4.IsChecked == true)
            {
                timer1.Stop();
                timer1.Enabled = false;
            }
            else
            {
                timer_xh.Stop();
                timer_xh.Enabled = false;
            }
            

            Nud_n1.IsEnabled = true;
            Nud_n2.IsEnabled = true;
            BtnG.IsEnabled = true;
            Btn_M_Cancle.IsEnabled = false;
            Btn_M_cq.IsEnabled = false;
            Btn_M_BacktoPG.IsEnabled = true;
            Btn_M_Restart.IsEnabled = true;
            Tbk_M_Info.Text = Infotext;
            Tbk_M_Text.Text = TText;
        }

        private void Btn_DrawS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Rbtn_R1.IsChecked == true)
            {
                ChouQian_Auto();
            }
            else if (Rbtn_R2.IsChecked == true)
            {
                ChouQian_Manual();
            }
            else
            {

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

        private void Btn_Cancle_Click(object sender, RoutedEventArgs e)
        {
            StopMualTask("[手动模式] 已终止Task", "已终止！");
        }

        private void Btn_ScvLeft_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Sve_M_s1.ScrollToHorizontalOffsetWithAnimation(Sve_M_s1.HorizontalOffset - 155);
            }));
        }

        private void Btn_ScvRight_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Sve_M_s1.ScrollToHorizontalOffsetWithAnimation(Sve_M_s1.HorizontalOffset + 155);
            }));
        }

        private void Btn_Enter_Click(object sender, RoutedEventArgs e)
        {
            Tbcl.SelectedIndex = 2;
        }

        private void Btn_BacktoM_Click(object sender, RoutedEventArgs e)
        {
            Tbcl.SelectedIndex = 1;
        }

        private void Btn_M_BacktoPG_Click(object sender, RoutedEventArgs e)
        {
            Tbcl.SelectedIndex = 2;
        }

        /// <summary>
        /// 按钮组选择 自动模式 的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rbtn_R1_Click_1(object sender, RoutedEventArgs e)
        {
            Nud_n2.IsEnabled = false;
            BtnG_2.IsEnabled = false;
            Cbx_sf.IsEnabled = true;
        }
        /// <summary>
        /// 按钮组选择 手动模式 的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rbtn_R2_Click_1(object sender, RoutedEventArgs e)
        {
            if (Cbx_cqgt.SelectedIndex == 0)
            {
                BtnG_2.IsEnabled = true;
            }
            Nud_n2.IsEnabled = true;
            Cbx_sf.IsEnabled = false;
        }

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
                Nud_n1.Maximum = root.students.Count;
                BtnG_2.IsEnabled = true;
            }
            else if (Cbx_cqgt.SelectedIndex == 1)
            {
                Nud_stid.IsEnabled = false;
                Nud_endid.IsEnabled = false;
                Nud_n1.Maximum = boys.Count;
                BtnG_2.IsEnabled = false;
                Rbtn_R3.IsChecked = true;
            }
            else if (Cbx_cqgt.SelectedIndex == 2)
            {
                Nud_stid.IsEnabled = false;
                Nud_endid.IsEnabled = false;
                Nud_n1.Maximum = girls.Count;
                BtnG_2.IsEnabled = false;
                Rbtn_R3.IsChecked = true;
            }
        }

        private void MWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer1.Interval = 13;
            timer1.Enabled = false;
            timer1.Elapsed += XHNum;
            timer_xh.Interval = 13;
            timer_xh.Enabled = false;
            timer_xh.Elapsed += XHName;
            timer_start.Interval = 1000;
            timer_start.Enabled = true;
            timer_start.Elapsed += ST;
            timer_start.Start();
            _ = Task.Run(new Action(() =>
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
                        Btn_DrawS.IsEnabled = false;
                        Btn_BacktoM.IsEnabled = false;
                        Btn_Enter.IsEnabled = true;
                        Btn_Enter.Visibility = Visibility.Visible;
                        Tbk_pz_Info.Visibility = Visibility.Visible;
                        Tbk_wel.Text = "少女祈祷失败";
                        Tbk_wel2.Text = "加载Json数据失败！";
                        Pgb_wel.Visibility = Visibility.Hidden;
                        HandyControl.Controls.MessageBox.Error(ex.Message, "错误");
                        return;
                    }
                    try
                    {
                        //wtl
                        str = File.ReadAllText(@"D:\ChiyoS\config\wtlist.json");
                        root_Wtl = JsonConvert.DeserializeObject<Root_wtl>(str);

                        int[] st_tmp= new int[root_Wtl.student.Count];
                        int i = 0;
                        foreach(string n in root_Wtl.student)
                        {
                            st_tmp[i] = int.Parse(n);
                            //Everyone.Remove(n);
                            i++;
                        }
                        wtlist = Getname(st_tmp, 0);
                        isewtl = true;
                    }
                    catch
                    {

                    }
                    Nud_n1.Maximum = root.students.Count;
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
                    try
                    {
                        Everyone_f = Everyone;
                        boys_f = boys;
                        if (wtlist.Count != 0)
                        {
                            foreach (string ns in wtlist)
                            {
                                Everyone_f.Remove(ns);
                                Everyone_f.Remove("曾婉晴");
                                boys_f.Remove(ns);
                            }
                            
                            
                        }
                        
                    }
                    catch
                    {

                    }
                    //Growl.Success("少女祈祷成功！");
                    Growl.Info("当前载入 " + root.title);
                    Title = "ChiyoS.Draw.Komari|Version:2.3.0.0_WBranch|当前载入配置文件 " + root.title;
                }));
            }));
        }
        private int[] STFilter(int[] st,int md)
        {
            if (md == 3)
            {
                return st;
            }
            ArrayList sts = Getname(st, md);
            for (int i = 0; i < st.Length; i++)
            {
                
                //int[] l_tmp = new int[st[i]];
                //string s1 = Getname(l_tmp, md)[0].ToString();
                if (wtlist.Contains(sts[i].ToString()))
                {
                    if (md == 0)
                    {
                        while (true)
                        {
                            foreach (string ns in Everyone_f)
                            {
                                if (sts.Contains(ns))
                                {
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("Name {0} 将被替换成 Name {1}", sts[i], ns));
                                    Console.WriteLine(string.Format("P-ns:{0} sts[i]:{1} st[i]:{2} eveid:{3}", ns, sts[i], st[i],Everyone.IndexOf(ns)));
                                    sts[i] = ns;
                                    st[i] = Everyone.IndexOf(ns)+3;
                                    Console.WriteLine(string.Format("-ns:{0} sts[i]:{1} st[i]:{2} ", ns, sts[i], st[i]));
                                    break;
                                }
                            }
                            break;
                        }
                        
                    }
                    else if (md == 1)
                    {
                        while (true)
                        {
                            foreach (string ns in boys_f)
                            {
                                if (sts.Contains(ns))
                                {
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("Name {0} 将被替换成 Name{1}", sts[i], ns));
                                    Console.WriteLine(string.Format("P-ns:{0} sts[i]:{1} st[i]:{2} ", ns, sts[i], st[i]));
                                    sts[i] = ns;
                                    st[i] = boys.IndexOf(ns)+1;
                                    Console.WriteLine(string.Format("-ns:{0} sts[i]:{1} st[i]:{2} ", ns, sts[i], st[i]));
                                    break;
                                }
                            }
                            break;
                        }
                        
                    }
                }
            }
            return st;
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


        private void Btn_M_Restart_Click(object sender, RoutedEventArgs e)
        {
            Btn_DrawS_Click(sender, e);
        }
    }
}