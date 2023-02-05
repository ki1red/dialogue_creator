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
    public partial class ExportWindow : Window
    {
        private DialogDFD dialog;
        private Dictionary<int, string> elements;
        public ExportWindow(string pathToDfdFile, string nameDfdFile, string formatDfdFile, DialogDFD dialog)
        {
            InitializeComponent();

            this.dialog = dialog;
            InitializeElements();

            InitializeDirectoryes(pathToDfdFile);
            InitializeGroupFileDfd(nameDfdFile, formatDfdFile);
            InitializeGroupFileDlag();
            InitializeGroupFileDlt();
            InitializeGroupFileDlv();
            InitializeGroupStartDialog();
            InitializeGroupEndDialog();
            InitializeNextFileDlag();
        }

        private void InitializeDirectoryes(string initialDirectory)
        {
            FileDlag.InitialDirectory = initialDirectory;
            FileDlt.InitialDirectory = initialDirectory;
            FileDlv.InitialDirectory = initialDirectory;
        }
        private void InitializeGroupFileDfd(string nameFile, string formatFile)
        {
            this.TextBox_devFile.Text = $"{nameFile}.{formatFile}";
            this.TextBox_devFile.IsEnabled = false;

            this.CheckBox_addDevFile.IsChecked = true;
            this.CheckBox_addDevFile.IsEnabled = false;
        }
        private void InitializeGroupFileDlag()
        {
            this.TextBox_gameFile.Text = null;
            this.TextBox_gameFile.IsEnabled = false;

            this.Button_createGameFile.IsEnabled = true;
            this.Button_createGameFile.Click += Button_createGameFile_Click;
        }
        private void InitializeGroupFileDlt()
        {
            this.TextBox_gameFile.Text = null;
            this.TextBox_gameFile.IsEnabled = false;

            this.Button_createFileTranslate.IsEnabled = true;
            this.Button_createFileTranslate.Click += Button_createFileTranslate_Click;
        }
        private void InitializeGroupFileDlv()
        {
            this.TextBox_gameFile.Text = null;
            this.TextBox_gameFile.IsEnabled = true;

            this.CheckBox_addTrashFile.IsEnabled = true;
            this.CheckBox_addTrashFile.Click += CheckBox_addTrashFile_Click;
        }
        private void InitializeGroupStartDialog()
        {
            this.ComboBox_startDialog.IsEnabled = true;
            for (int i = 0; i < dialog.elements.Length; i++)
            {
                bool check = true;

                ElementDFD element = dialog.elements[i];
                if (element.question.requests.Length != 0)
                {
                    check = false;
                    continue;
                }
                for (int j = 0; j < element.answers.Length; j++)
                {
                    SayingElementDFD saying = element.answers[j];

                    if (saying.requests.Length != 0)
                    {
                        check = false;
                        break;
                    }

                }

                if (check)
                    this.ComboBox_startDialog.Items.Add(element.question.text);
            }
            if (this.ComboBox_startDialog.Items.Count == 0)
            {
                MessageBox.Show("Нет ни одного начального элемента", "Ошибка");
                return; // TODO исключение
            }
            else if (this.ComboBox_startDialog.Items.Count > 1)
            {
                MessageBox.Show("Слишком много начальных элементов", "Ошибка");
            }

            this.ComboBox_startDialog.SelectedIndex = 0;

            this.CheckBox_addStartDialog.IsEnabled = true;
            this.CheckBox_addStartDialog.Click += CheckBox_addStartDialog_Click;
        }
        private void InitializeGroupEndDialog()
        {
            this.ComboBox_endDialog.IsEnabled = true;
            for (int i = 0; i < dialog.elements.Length; i++)
            {
                bool check = true;

                ElementDFD element = dialog.elements[i];
                SayingElementDFD test = new SayingElementDFD();
                test.requests = null;

                if (!sr(element.question.nextElement))
                {
                    check = false;
                    continue;
                }
                for (int j = 0; j < element.answers.Length; j++)
                {
                    SayingElementDFD saying = element.answers[j];

                    if (!sr(saying.nextElement))
                    {
                        check = false;
                        break;
                    }

                }

                if (check)
                    this.ComboBox_endDialog.Items.Add(element.question.text);
            }
            if (this.ComboBox_endDialog.Items.Count == 0)
            {
                MessageBox.Show("Нет ни одного конечного элемента", "Ошибка");
                return; // TODO исключение
            }
            else if (this.ComboBox_endDialog.Items.Count > 1)
            {
                MessageBox.Show("Слишком много конечных элементов", "Ошибка");
            }

            this.ComboBox_endDialog.SelectedIndex = 0;

            this.CheckBox_addEndDialog.IsEnabled = true;
            this.CheckBox_addEndDialog.Click += CheckBox_addEndDialog_Click;
        }
        private void InitializeNextFileDlag()
        {
            this.TextBox_selectNextFile.Text = null;
            this.TextBox_selectNextFile.IsEnabled = false;

            this.Button_selectNextFile.IsEnabled = true;
            this.Button_selectNextFile.Click += Button_selectNextFile_Click;
        }
        private void InitializeElements()
        {
            this.elements = new Dictionary<int, string>(this.dialog.elements.Length);

            for (int i = 0; i < this.elements.Count; i++)
            {
                this.elements[i] = this.dialog.elements[i].question.text;
            }
        }

        private void Button_createGameFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Button_createFileTranslate_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void CheckBox_addTrashFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void CheckBox_addStartDialog_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void CheckBox_addEndDialog_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Button_selectNextFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool sr(SayingElementDFD first)
        {
            if (first.text == null && first.type == TypeSayingElementDFD.none && first.idElement == -1 && (first.requests == null || first.requests.Length == 0))
                return true;
            else
                return false;
        }
    }
}
