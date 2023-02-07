using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DialogsCreator
{
    public partial class TranslateWindow : Window
    {
        private DialogDTO dialog = null;
        private int[] randomIndex = null;
        private int numCreateFileInGrid = -1;
        private Language language_createFile = DialogsCreator.Language.none;
        public TranslateWindow()
        {
            InitializeComponent();

            InitializeLoadFile();
            InitializeCreateFile();
            InitializeOpenFile();
            InitializeSaveFile();
            InitializeCompleteFile();
            InitializeCloseFile();
        }

        // ===========================================================================================================================
        // ============================================ ИНИЦИАЛИЗАЦИЯ ================================================================
        // ===========================================================================================================================

        private void InitializeLoadFile()
        {
            this.MenuItem_loadFile.IsEnabled = true;
            this.MenuItem_loadFile.Click += MenuItem_loadFile_Click;
            this.MenuItem_loadFile.Click += UpdateWindowElements;
        }
        private void InitializeCreateFile()
        {
            this.MenuItem_createFile.IsEnabled = false;
            this.MenuItem_createFile.Click += MenuItem_createFile_Click;
            this.MenuItem_createFile.Click += UpdateWindowElements;
        }
        private void InitializeOpenFile()
        {
            this.MenuItem_openFile.IsEnabled = false;
            this.MenuItem_openFile.Click += MenuItem_openFile_Click;
            this.MenuItem_openFile.Click += UpdateWindowElements;
        }
        private void InitializeSaveFile()
        {
            this.MenuItem_saveFile.IsEnabled = false;
            this.MenuItem_saveFile.Click += MenuItem_saveFile_Click;
            this.MenuItem_saveFile.Click += UpdateWindowElements;
        }
        private void InitializeCompleteFile()
        {
            this.MenuItem_completeTranslate.IsEnabled = false;
            this.MenuItem_completeTranslate.Click += MenuItem_completeTranslate_Click;
        }
        private void InitializeCloseFile()
        {
            this.MenuItem_closeFile.IsEnabled = false;
            this.MenuItem_closeFile.Click += MenuItem_closeFile_Click;
            this.MenuItem_closeFile.Click += UpdateWindowElements;
        }

        // ===========================================================================================================================
        // ==================================================== КЛИКИ ================================================================
        // ===========================================================================================================================

        private void MenuItem_loadFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void MenuItem_createFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void MenuItem_openFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void MenuItem_saveFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void MenuItem_closeFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void MenuItem_completeTranslate_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // ===========================================================================================================================
        // =================================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ================================================================
        // ===========================================================================================================================

        private void UpdateWindowElements(object sender, EventArgs e)
        {

        }
    }
}
