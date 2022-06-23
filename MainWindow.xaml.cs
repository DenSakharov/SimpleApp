using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<FileInformation> fileAtributes = new List<FileInformation>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var path = tbPath.Text;
            var lstFiles = Directory.GetFiles(path);
            var lstDirs = Directory.GetDirectories(path);
            fileAtributes = new List<FileInformation>();

            if (tbPath.Text.LastIndexOf(@"\") != tbPath.Text.Length - 1) fileAtributes.Add(new FileInformation("..", "", "Dir", "-"));

                foreach (var name in lstDirs)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(name);
                fileAtributes.Add(new FileInformation(name, dirInfo.LastWriteTime.ToString("dd-MM-yyyy HH:mm:ss"), "Dir",GetSizeInDir(name).ToString()));
            }
            foreach (var name in lstFiles)
            {
                fileAtributes.Add(new FileInformation(name, File.GetLastWriteTime(name).ToString("dd-MM-yyyy HH:mm:ss"),name.Substring(name.LastIndexOf(".")+1), new FileInfo(name).Length.ToString()));
            }
            grid.ItemsSource = fileAtributes;
            grid.Items.Refresh();
            //MessageBox.Show("Кнопка нажата");
        }
        private void Row_Click(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
          
                // Starts the Edit on the row;
                FileInformation item = (FileInformation)((DataGridRow)sender).Item;
               if (item.Tip.ToLower().Equals("dir"))
                {
                if (item.name.Equals(".."))
                {
                    tbPath.Text = tbPath.Text.Substring(0, tbPath.Text.LastIndexOf(@"\"));
                    if (tbPath.Text.IndexOf(@"\") < 0) tbPath.Text += @"\";
                }

                else
                {
                    if (tbPath.Text.LastIndexOf(@"\")!=tbPath.Text.Length-1) tbPath.Text += @"\";
                    tbPath.Text += item.name;
                } 
                               Button_Click(null, null);
                }
            
        }

        public long GetSizeInDir(string path)
        {
       
            long size = 0;
            var lst= Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (string name in lst)
            {
                try
                {
                    size += new FileInfo(name).Length;
                }
                catch (Exception e) {
                    int a = 0;
                }
            }
            //                var lstFiles = Directory.GetFiles(path);
            //                var lstDirs = Directory.GetDirectories(path);
            //                List<FileInformation> fileAtributes = new List<FileInformation>();

            //                foreach (var name in lstDirs)
            //                {
            //                    DirectoryInfo dirInfo = new DirectoryInfo(name);
            //                size += GetSizeInDir(name);
            ////                fileAtributes.Add(new FileInformation(name, dirInfo.LastWriteTime.ToString("dd-MM-yyyy HH:mm:ss"), "Dir", "-"));
            //                }
            //                foreach (var name in lstFiles)
            //                {
            //                size += new FileInfo(name).Length;
            // //                   fileAtributes.Add(new FileInformation(name, File.GetLastWriteTime(name).ToString("dd-MM-yyyy HH:mm:ss"), name.Substring(name.LastIndexOf(".") + 1), new FileInfo(name).Length.ToString()));
            //                }
            return size;
            }

        public class FileInformation
        {
            public string name { get; set; }

            public string dateUpdate { get; set; }

            public string Tip { get; set; }

            public string size{ get; set; }

            public FileInformation(string name,string dateUpdate,string tip,string size)
            {
                this.name = name.Substring(name.LastIndexOf(@"\")+1);
                this.dateUpdate = dateUpdate;
                this.Tip= tip;
                this.size = size;
            }
        }

        private void Button_Click_to_JSON(object sender, RoutedEventArgs e)
        {
            string json = @"[";
            foreach(var item in fileAtributes)
            {
                if (json.Length > 1) json += ",";
                json += @"{""name"":""" + item.name + @""",""dateUpdate"":""" + item.dateUpdate + @""",""tip"":""" + item.Tip + @""",""size"":""" + item.size + @"""}";
            }
            json += "]";
            StreamWriter SW = new StreamWriter(new FileStream("FileManager.json", FileMode.Create, FileAccess.Write));
            SW.Write(json);
            SW.Close();
        }

        private void Button_Click_from_JSON(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader r = new StreamReader("FileManager.json"))
                {
                    string json = r.ReadToEnd();
                    List<FileInformation> items = JsonConvert.DeserializeObject<List<FileInformation>>(json);
                    grid.ItemsSource = items;
                    grid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

}
