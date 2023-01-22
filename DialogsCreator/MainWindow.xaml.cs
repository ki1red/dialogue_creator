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
    }
}
