﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace SimpleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var path = tbPath.Text;
            var lstFiles = Directory.GetFiles(path);
            var lstDirs = Directory.GetDirectories(path);
            List<FileInformation> fileAtributes = new List<FileInformation>();
            foreach (var name in lstDirs)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(name);
                fileAtributes.Add(new FileInformation(name, dirInfo.LastWriteTime.ToString("dd-MM-yyyy HH:mm:ss"), "Dir", "-"));
            }
            foreach (var name in lstFiles)
            {
                fileAtributes.Add(new FileInformation(name, File.GetLastWriteTime(name).ToString("dd-MM-yyyy HH:mm:ss"),name.Substring(name.LastIndexOf(".")+1), new FileInfo(name).Length.ToString()));
            }
            grid.ItemsSource = fileAtributes;
            grid.Items.Refresh();
            //MessageBox.Show("Кнопка нажата");
        }
        public class FileInformation
        {
            public string name { get; set; }

            public string dateUpdate { get; set; }

            public string type { get; set; }

            public string size{ get; set; }

            public FileInformation(string name,string dateUpdate,string type,string size)
            {
                this.name = name.Substring(name.LastIndexOf(@"\")+1);
                this.dateUpdate = dateUpdate;
                this.type= type;
                this.size = size;
            }
        }

    }

}