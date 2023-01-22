using Microsoft.Win32;
using System;
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
using System.Text.Json;

namespace DialogsCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathFile;
        public MainWindow()
        {
            InitializeComponent();

            pathFile = Environment.CurrentDirectory;
            this.gb_add_answers.IsEnabled = false;
            this.gb_add_image.IsEnabled = false;
            this.gb_add_sound.IsEnabled = false;
        }

        private void CreateTable(string pathToFile)
        {
            
        }

        private void EnableAddingDialogues()
        {

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            string path = pathFile;


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.dlag)|*.dlag|All files (*.*)|*.*"; // фильтры файлов
            openFileDialog.InitialDirectory = @path; // путь по умолчанию для окна
            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
                pathFile = path;
            }
            else
                return;

            CreateTable(path);
        }

        private void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            string path = pathFile;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.dlag)|*.dlag|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = @path;

            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
                Stream myStream = saveFileDialog.OpenFile();

                pathFile = path;
            }
            else
                return;

            CreateTable(path);
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = pathFile;
            myStream = saveFileDialog.OpenFile();
        }

        private void DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            datagrid.Width = this.Width / 2;
            datagrid.Height = this.Height;
        }

        private void cb_sound_Click(object sender, RoutedEventArgs e)
        {
            if (cb_sound.IsChecked == true)
                gb_add_sound.IsEnabled = true;
            else
            {
                gb_add_sound.IsEnabled = false;
                gb_add_sound.
            }
        }

        private void cb_image_Click(object sender, RoutedEventArgs e)
        {
            if (cb_image.IsChecked == true)
                gb_add_image.IsEnabled = true;
            else
                gb_add_image.IsEnabled = false;
        }

        private void cb_answers_Click(object sender, RoutedEventArgs e)
        {
            if (cb_answers.IsChecked == true)
                gb_add_answers.IsEnabled = true;
            else
                gb_add_answers.IsEnabled = false;
        }
    }
}
