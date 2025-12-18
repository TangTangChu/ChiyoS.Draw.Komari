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
        ObservableCollection<member> memberData = new ObservableCollection<member>();
        public StudentsDataEditor(Root root)
        {
            InitializeComponent();
            root1 = root;
            Dtg_1.DataContext = memberData;
        }
        public StudentsDataEditor(string jspath)
        {
            InitializeComponent();
            LoadJson(jspath);
            Dtg_1.DataContext = memberData;
            //Dtg_1.Items.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Ascending));

        }
        public void LoadJson(string newjspath)
        {
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
                return;
            }
        }
        public void RefreshData(string newpath)
        {
            jsfilepath = newpath;
            
           
        }
            
        
        private void Mit_open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "学生数据Json文件 (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true) 
            {
                LoadJson(openFileDialog.FileName);
                
            }
                
        }
    

        private void Mit_save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_AddST_Click(object sender, RoutedEventArgs e)
        {
            memberData.Add(new member { SIndex = Dtg_1.Items.Count+1, Name = "栗山未来", Gender = SexOpt.Girl });
            Dtg_1.ScrollIntoView(Dtg_1.Items.GetItemAt(Dtg_1.Items.Count-1));
        }

        private void Btn_DelST_Click(object sender, RoutedEventArgs e)
        {
            var sl = (member)Dtg_1.SelectedItem;
            memberData.Remove(sl);
        }

        private void Btn_UppenST_Click(object sender, RoutedEventArgs e)
        {
            member sl1 = (member)Dtg_1.SelectedItem;
            member sl2 = (member)Dtg_1.Items.GetItemAt(Dtg_1.SelectedIndex - 1);
            sl1.SIndex--;
            sl2.SIndex++;
            
        }

        private void Btn_DnST_Click(object sender, RoutedEventArgs e)
        {
            member sl1 = (member)Dtg_1.SelectedItem;
            member sl2 = (member)Dtg_1.Items.GetItemAt(Dtg_1.SelectedIndex + 1);
            sl1.SIndex++;
            sl2.SIndex--;
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
