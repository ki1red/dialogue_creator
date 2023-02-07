using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace DialogsCreator
{
    public partial class TranslateWindow : Window
    {
        private byte[] cryptLanguage = new byte[32] { 4, 19, 38, 29, 250, 173, 195, 136, 133, 251, 192, 37, 55, 12, 222, 210, 102, 32, 99, 10, 28, 22, 13, 49, 31, 55, 27, 29, 138, 9, 11, 93 };
        private byte[] cryptGame = new byte[32] { 18, 39, 20, 19, 222, 19, 211, 10, 1, 255, 39, 12, 14, 155, 19, 33, 123, 176, 182, 150, 214, 13, 165, 200, 53, 164, 177, 234, 55, 155, 255, 91 };

        private bool dlagOpen = false; // открыт игровой файл
        private bool dltOpen = false; // открыт файл перевода (стандартный)
        private bool dltOpenn = false; // открыт/создан файл перевода (новый)
        private string typeOldLanguage = null; // изначальный язык
        private string typeNewLanguage = null; // добавленный язык

        private DialogDTO dialog = null; // открытый файл игры
        private Dictionary<int, string> defaultTextLanguage = null; // набор текстов на стандартном языке
        private Dictionary<int, string> newTextLanguage = null; // набор текстов на новом языке
        private int[] randomIndex = null; // массив индексов для grid

        private string pathToNewDlt = null;
        private string nameNewDlt = null;

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
            bool open;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileDlag.Filter;
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.DefaultExt = FileDlag.DefaultExt;
            openFileDialog.Title = "Открыть игровой файл";

            if (openFileDialog.ShowDialog() == true)
            {
                open = OpenDlag(openFileDialog.FileName);

                if (!open)
                {
                    MessageBox.Show("Не получилось открыть игровой файл", "Ошибка");

                    dlagOpen = false;
                    dltOpen = false;

                    return;
                }

                dlagOpen = true;
            }
            else return;

            string[] pathToDlt = openFileDialog.FileName.Split("\\");
            pathToDlt[pathToDlt.Length - 1] = dialog.textPaths[0].path;
            typeOldLanguage = dialog.textPaths[0].language;
            string dlt = String.Join('\\', pathToDlt);

            open = OpenDlt(dlt);

            if (!open)
            {
                MessageBox.Show("Не получилось открыть файл перевода", "Ошибка");

                dlagOpen = false;
                dltOpen = false;
                typeOldLanguage = null;

                return;
            }

            dlagOpen = true;
            dltOpen = true;

            randomIndex = GetRandomArray(defaultTextLanguage.Count); // TODO надо тестить
        }
        private void MenuItem_createFile_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateFile())
                return;
            dltOpenn = true;
        }
        private void MenuItem_openFile_Click(object sender, RoutedEventArgs e)
        {
            if (!OpenFile())
            {
                MessageBox.Show("Не удалось открыть файл", "Ошибка");
                return;
            }

            if (!DeserializationDTO())
            {
                MessageBox.Show("Не удалось считать данные с файла", "Ошибка");
                return;
            }

            dltOpenn = true;
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
            if (dlagOpen && dltOpen)
            {
                this.MenuItem_createFile.IsEnabled = true;
                this.MenuItem_openFile.IsEnabled = true;
                this.MenuItem_loadFile.IsEnabled = false;
            }
            else
            {
                this.MenuItem_createFile.IsEnabled = false;
                this.MenuItem_openFile.IsEnabled = false;
                this.MenuItem_loadFile.IsEnabled = true;
            }

            if (dltOpenn)
            {
                this.MenuItem_closeFile.IsEnabled = true;
                this.MenuItem_saveFile.IsEnabled = true;
                this.MenuItem_completeTranslate.IsEnabled = true;
                DrawText();
            }
            else
            {
                this.MenuItem_closeFile.IsEnabled = false;
                this.MenuItem_saveFile.IsEnabled = false;
                this.MenuItem_completeTranslate.IsEnabled = false;
            }
        }
        private void DrawText()
        {
            this.listBoxNew.MouseDoubleClick += ListBoxNew_MouseDoubleClick;
            this.listBoxOld.MouseDoubleClick += ListBoxNew_MouseDoubleClick;

            List<string> listOld = new List<string>();
            List<string> listNew = new List<string>();
            const int size = 18;

            foreach (var item in defaultTextLanguage)
            {
                listOld.Add(item.Value);
            }

            if (newTextLanguage != null && newTextLanguage.Count > 0)
            {
                foreach (var item in newTextLanguage)
                {
                    listNew.Add(item.Value);
                }
            }
            else
            {
                listNew = new List<string>(listOld);
            }

            for (int i = 0; i < listOld.Count; i++)
            {
                int index = i;
                if (randomIndex != null && i < randomIndex.Length)
                {
                    index = randomIndex[i];
                }

                TextBlock textBlock = new TextBlock();
                textBlock.Text = listOld[index];
                textBlock.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x19, 0x18, 0x18));
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x13, 0xA9, 0xF7));
                textBlock.FontSize = size;
                textBlock.Width = listBoxOld.Width;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.UpdateLayout();
                listBoxOld.Items.Add(textBlock);

                TextBox textBox = new TextBox();
                textBox.KeyDown += TextBox_KeyDown;
                textBox.Text = (i < listNew.Count) ? listNew[index] : "";
                textBox.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x19, 0x18, 0x18));
                textBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x19, 0x18, 0x18));
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x13, 0xA9, 0xF7));
                textBox.FontSize = size;
                textBox.Width = listBoxNew.Width;
                textBox.TextWrapping = TextWrapping.Wrap;
                textBox.UpdateLayout();
                listBoxNew.Items.Add(textBox);
            }
            listBoxNew.UpdateLayout();
            listBoxOld.UpdateLayout();
        }

        private void ListBoxNew_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            int select = listBox.SelectedIndex;

            if (listBox.Name == "listBoxNew")
                listBoxOld.SelectedIndex = select;
            else if (listBox.Name == "listBoxOld")
                listBoxNew.SelectedIndex = select;
        }

        private bool DeserializationDTO()
        {
            byte[] data = File.ReadAllBytes($"{pathToNewDlt}\\{nameNewDlt}");

            if (data == null)
                return false;

            if (data.Length == 0)
            {
                newTextLanguage = new Dictionary<int, string>();
                return true;
            }

            string text = EncryptJSON.Decrypt(data, cryptLanguage);

            if (text == null)
                return false;

            newTextLanguage = JsonConvert.DeserializeObject<Dictionary<int,string>>(text);

            if (newTextLanguage == null)
                return false;
            else
                return true;
        }
        private bool SerializationDTO()
        {
            return false;
        }
        private bool OpenDlag(string pathToDlag)
        {
            byte[] cryptText = null;
            cryptText = File.ReadAllBytes($"{pathToDlag}");
            if (cryptText == null)
                return false;

            string text = EncryptJSON.Decrypt(cryptText, cryptGame);

            dialog = JsonConvert.DeserializeObject<DialogDTO>(text);

            if (dialog == null)
                return false;
            else
                return true;
        }
        private bool OpenDlt(string pathToDlt)
        {
            byte[] cryptText = null;
            cryptText = File.ReadAllBytes($"{pathToDlt}");
            if (cryptText == null)
                return false;

            if (cryptText == null)
                return false;

            string text = EncryptJSON.Decrypt(cryptText, cryptLanguage);

            defaultTextLanguage = JsonConvert.DeserializeObject<Dictionary<int, string>>(text);

            if (defaultTextLanguage == null)
                return false;
            else
                return true;
        }
        private bool CreateFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FileDlt.Filter;
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.DefaultExt = FileDlt.DefaultExt;
            saveFileDialog.Title = FileDlt.Title;

            if (saveFileDialog.ShowDialog() == true)
            {

                Stream myStream;
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    myStream.Close();
                    string[] pathAndFile = saveFileDialog.FileName.Split("\\");

                    nameNewDlt = pathAndFile[pathAndFile.Length - 1];

                    Array.Resize(ref pathAndFile, pathAndFile.Length - 1);

                    pathToNewDlt = String.Join('\\', pathAndFile);

                    if (nameNewDlt == null || pathToNewDlt == null)
                    {
                        nameNewDlt = null;
                        pathToNewDlt = null;
                        return false;
                    }
                    else
                    {
                        Language language = DialogsCreator.Language.none;
                        SelectLanguageWindow window = new SelectLanguageWindow(); // требование указать язык файла
                        do
                        {
                            window.ShowDialog();
                            language = window.language;
                        } while (language == DialogsCreator.Language.none);

                        typeNewLanguage = language.ToString();

                        return true;
                    }
                }
            }
            return false;
        }
        private bool OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileDlt.Filter;
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.DefaultExt = FileDlt.DefaultExt;
            openFileDialog.Title = FileDlt.Title;

            if (openFileDialog.ShowDialog() == true)
            {
                string[] pathAndFile = openFileDialog.FileName.Split("\\");

                nameNewDlt = pathAndFile[pathAndFile.Length - 1];

                Array.Resize(ref pathAndFile, pathAndFile.Length - 1);

                pathToNewDlt = String.Join('\\', pathAndFile);

                if (nameNewDlt == null || pathToNewDlt == null)
                {
                    nameNewDlt = null;
                    pathToNewDlt = null;
                    return false;
                }
                else
                {
                    Language language = DialogsCreator.Language.none;
                    SelectLanguageWindow window = new SelectLanguageWindow(); // требование указать язык файла
                    do
                    {
                        window.ShowDialog();
                        language = window.language;
                    } while (language == DialogsCreator.Language.none);

                    typeNewLanguage = language.ToString();

                    return true;
                }
               
            }
            return false;
        }
        public static int[] GetRandomArray(int length)
        {
            Random rand = new Random();
            int[] num1 = new int[length];
            for (int i = 0; i < num1.Length; i++)
                num1[i] = i;
            
            for (int i = 0; i < num1.Length-1; i++)
            {
                int j = rand.Next(i+1, num1.Length - 1);

                int tmp = num1[i];
                num1[i] = num1[j];
                num1[j] = tmp;
            }
            return num1;
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int caretIndex = textBox.CaretIndex;
            string currentText = textBox.Text;
            switch (e.Key)
            {
                case Key.Enter:
                    caretIndex = textBox.CaretIndex;
                    currentText = textBox.Text;
                    textBox.Text = currentText.Insert(caretIndex, Environment.NewLine);
                    textBox.CaretIndex = caretIndex + Environment.NewLine.Length;
                    e.Handled = true;
                    break;
                case Key.Tab:
                    caretIndex = textBox.CaretIndex;
                    currentText = textBox.Text;
                    textBox.Text = currentText.Insert(caretIndex, "\t");
                    textBox.CaretIndex = caretIndex + "\t".Length;
                    e.Handled = true;
                    break;
            }
        }

    }

}
