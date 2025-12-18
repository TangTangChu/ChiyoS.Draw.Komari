using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Runtime.Remoting.Messaging;
using HandyControl.Controls;

namespace ChiyoS.Draw.Komari
{
    
    public enum SexOpt { Boy, Girl };
    /// <summary>
    /// StudentsDataEditor.xaml 的交互逻辑
    /// </summary>
    public partial class StudentsDataEditor : HandyControl.Controls.Window
    {
        Root root1;
        string jsfilepath="";
        private ObservableCollection<member> memberData = new ObservableCollection<member>();
        public ObservableCollection<member> MemberData
        {
            get { return memberData; }
            set { memberData = value; }
        }
        public StudentsDataEditor(Root root)
        {
            InitializeComponent();
            root1 = root;
            //Dtg_1.DataContext = MemberData;
        }
        public StudentsDataEditor(string jspath)
        {
            InitializeComponent();
            jsfilepath = jspath;
            //Dtg_1.Items.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Ascending));

        }
        public void LoadJson(string newjspath)
        {
            Mit_save.IsEnabled = false;
            Mit_saveas.IsEnabled = false;
            jsfilepath = newjspath;
            string str;
            try
            {
                //加载学号表
                str = File.ReadAllText(jsfilepath);
                root1 = JsonConvert.DeserializeObject<Root>(str);
                Tbk_filepath.Text = "Json文件路径: "+jsfilepath;
                Tbx_title.Text = root1.title;
                memberData.Clear();
                int sIndex = 1;
                foreach (Students sts in root1.students)
                {
                    if (sts.s == "b")
                    {
                        memberData.Add(new member { SIndex = sIndex, Name = sts.name, Gender = SexOpt.Boy });
                    }
                    else
                    {
                        memberData.Add(new member { SIndex = sIndex, Name = sts.name, Gender = SexOpt.Girl });
                    }
                    sIndex++;
                }
            }
            catch
            {
                Tbk_filepath.Text = "加载失败";
                HandyControl.Controls.Growl.Error("你加载的啥东西啊？");
                return;
            }
            Mit_save.IsEnabled = true;
            Mit_saveas.IsEnabled = true;
        }
        public void RefreshData(string newpath)
        {
            jsfilepath = newpath;
        }
            
        
        private void Mit_open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "学生数据Json文件 (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.Title = "选择你的英雄！";
            if (openFileDialog.ShowDialog() == true) 
            {
                LoadJson(openFileDialog.FileName);
            }            
        }

        private string SerializeData()
        {
            Root root = new Root();
            root.title = Tbx_title.Text;
            root.students = new List<Students>();
            string sg="";
            foreach (member item in MemberData)
            {
                if (item.Gender == 0)
                {
                    sg = "b";
                }
                else
                {
                    sg = "g";
                }
                root.students.Add(new Students { name = item.Name, s = sg });
            }
            return (JsonConvert.SerializeObject(root));
        }

        private void Mit_save_Click(object sender, RoutedEventArgs e)
        {
            string jss = SerializeData();
            try
            {
                StreamWriter streamWriter = new StreamWriter(jsfilepath, false);
                streamWriter.WriteLine(jss);
                streamWriter.Close();
                Growl.Success("保存成功");
            }
            catch(Exception ex)
            {
                Growl.Error("保存失败了呢");
                HandyControl.Controls.MessageBox.Error(ex.Message);
            }
            
            //StreamWriter streamWriter = new StreamWriter(jss);
        }
        private void Mit_saveas_Click(object sender, RoutedEventArgs e)
        {

            string jss = SerializeData();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "选择一个合适的位置保存你的数据";
            saveFileDialog.Filter = "学生数据Json文件 (*.json)|*.json|All files (*.*)|*.*";
            
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false);
                    streamWriter.WriteLine(jss);
                    streamWriter.Close();
                    Growl.Success("保存成功");
                }
                catch (Exception ex)
                {
                    Growl.Error("保存失败了呢");
                    HandyControl.Controls.MessageBox.Error(ex.Message);
                }
            } 
        }

        private void Btn_AddST_Click(object sender, RoutedEventArgs e)
        {
            memberData.Add(new member { SIndex = Dtg_1.Items.Count+1, Name = "栗山未来", Gender = SexOpt.Girl });
            Dtg_1.ScrollIntoView(Dtg_1.Items.GetItemAt(Dtg_1.Items.Count-1));
            Dtg_1.SelectedIndex=Dtg_1.Items.Count-1;
        }

        private void Btn_DelST_Click(object sender, RoutedEventArgs e)
        {
            if (Dtg_1.SelectedIndex == -1)
            {
                HandyControl.Controls.Growl.Warning("请选择项目！");
                return;
            }
            Stp_btg.IsEnabled = false;
            var sl = (member)Dtg_1.SelectedItem;
            int id = Dtg_1.SelectedIndex;
            memberData.Remove(sl);
            Task.Run(() =>
            {
                Dispatcher.Invoke(new Action(()=>{
                    int i = 1;
                    foreach (var item in MemberData)
                    {
                        item.SIndex = i;
                        i++;
                    }
            }));
            });
            Stp_btg.IsEnabled = true;
            Dtg_1.SelectedIndex = id;
        }

        private void Btn_UppenST_Click(object sender, RoutedEventArgs e)
        {
            if (Dtg_1.SelectedIndex == 0)
            {
                HandyControl.Controls.Growl.Warning("选择项已经位于顶部");
                return;
            }
            else if (Dtg_1.SelectedIndex == -1)
            {
                HandyControl.Controls.Growl.Warning("请选择项目！");
                return;
            }
            int oid = Dtg_1.SelectedIndex;
            var sl = (member)Dtg_1.SelectedItem;
            var sl2 = (member)Dtg_1.Items[oid - 1];
            sl.SIndex--;
            sl2.SIndex++;
            MemberData.Remove(sl);
            MemberData.Insert(oid-1, sl);
            Dtg_1.SelectedIndex = oid - 1;
        }

        private void Btn_DnST_Click(object sender, RoutedEventArgs e)
        {
            if (Dtg_1.SelectedIndex == Dtg_1.Items.Count-1)
            {
                HandyControl.Controls.Growl.Warning("选择项已经位于底部");
                return;
            }
            else if (Dtg_1.SelectedIndex == -1)
            {
                HandyControl.Controls.Growl.Warning("请选择项目！");
                return;
            }
            int oid = Dtg_1.SelectedIndex;
            var sl = (member)Dtg_1.SelectedItem;
            Console.WriteLine(oid);
            var sl2 = (member)Dtg_1.Items[oid + 1];
            sl.SIndex++;
            sl2.SIndex--;
            MemberData.Remove(sl);
            MemberData.Insert(oid + 1, sl);
            Dtg_1.SelectedIndex = oid + 1;
        }

        private void Btn_LoadNow_Click(object sender, RoutedEventArgs e)
        {
            LoadJson(jsfilepath);
            Dtg_1.DataContext = MemberData;
            Btn_LoadNow.Visibility = Visibility.Hidden;
            Dtg_1.Visibility = Visibility.Visible;
            Mit_tbk_1.Visibility = Visibility.Hidden;
            Stp_btg.IsEnabled = true;
        }

        private void Btn_CLEAR_Click(object sender, RoutedEventArgs e)
        {
            MemberData.Clear();
        }

        
    }

    public class member : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int _Sindex;
        private string _name;
        private SexOpt _gender;
        public int SIndex 
        {
            get => _Sindex;
            set { _Sindex = value;OnPropertyChanged("Sindex"); } 
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged("Name"); }
        }
        public SexOpt Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged("Gender"); }
        }

        protected internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }

}
