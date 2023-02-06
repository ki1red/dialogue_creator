using Microsoft.Win32;
using Newtonsoft.Json;
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
using System.Windows.Shapes;

namespace DialogsCreator
{
    public partial class ExportWindow : Window
    {
        private byte[] cryptLanguage = new byte[32] { 4, 19, 38, 29, 250, 173, 195, 136, 133, 251, 192, 37, 55, 12, 222, 210, 102, 32, 99, 10, 28, 22, 13, 49, 31, 55, 27, 29, 138, 9, 11, 93 };
        private byte[] cryptGame = new byte[32] { 18, 39, 20, 19, 222, 19, 211, 10, 1, 255, 39, 12, 14, 155, 19, 33, 123, 176, 182, 150, 214, 13, 165, 200, 53, 164, 177, 234, 55, 155, 255, 91 };

        private DialogDTO dialogDTO = new DialogDTO();
        private DialogDFD dialog;
        private Dictionary<int, string> elements;
        private Dictionary<int, string> allTexts = new Dictionary<int, string>();
        private string pathToDlag = null;
        private string pathToDlt = null;
        private string trashFile = null;
        private string startDialog = null;
        private string endDialog = null;
        private string pathToNextDlag = null;
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

            this.Button_createFile.Click += Button_createFile_Click;
        }

        private void Button_createFile_Click(object sender, RoutedEventArgs e)
        {
            string check;
            if ((check = CheckValidation()) != null)
            {
                MessageBox.Show(check, "Ошибка");
                return;
            }

            if (!Write())
                return;

            string jsonDlag = JsonConvert.SerializeObject(dialogDTO, Formatting.Indented);
            string jsonDlt = JsonConvert.SerializeObject(allTexts, Formatting.Indented);

            if (CheckBox_cryptFile.IsChecked == true)
            {
                var encryptDlag = EncryptJSON.Encrypt(jsonDlag, cryptGame);
                var encryptDlt = EncryptJSON.Encrypt(jsonDlt, cryptLanguage);


                File.WriteAllBytes(pathToDlag, encryptDlag);
                File.WriteAllBytes(pathToDlt, encryptDlt);
            }
            else
            {
                File.WriteAllText(pathToDlag, jsonDlag);
                File.WriteAllText(pathToDlt, jsonDlt);
            }

            MessageBox.Show("Экспорт завершён!");
            this.Close();
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
            this.TextBox_fileTranslate.Text = null;
            this.TextBox_fileTranslate.IsEnabled = false;

            this.Button_createFileTranslate.IsEnabled = true;
            this.Button_createFileTranslate.Click += Button_createFileTranslate_Click;
        }
        private void InitializeGroupFileDlv()
        {
            this.TextBox_trashFile.Text = null;
            this.TextBox_trashFile.IsEnabled = true;

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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FileDlag.Filter;
            saveFileDialog.InitialDirectory = FileDlag.InitialDirectory;
            saveFileDialog.DefaultExt = FileDlag.DefaultExt;
            saveFileDialog.Title = FileDlag.Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                Stream myStream;
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    myStream.Close();
                    TextBox_gameFile.Text = GetFile(saveFileDialog.FileName);
                    pathToDlag = saveFileDialog.FileName;
                }
            }
        }
        private void Button_createFileTranslate_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = FileDlt.Filter;
            saveFileDialog.InitialDirectory = FileDlt.InitialDirectory;
            saveFileDialog.DefaultExt = FileDlt.DefaultExt;
            saveFileDialog.Title = FileDlt.Title;

            if (saveFileDialog.ShowDialog() == true)
            {
                Stream myStream;
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    myStream.Close();
                    TextBox_fileTranslate.Text = GetFile(saveFileDialog.FileName);
                    pathToDlt = saveFileDialog.FileName;
                }
            }
        }
        private void CheckBox_addTrashFile_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_addTrashFile.IsChecked == true && TextBox_trashFile.Text != null && TextBox_trashFile.Text.Length > 0)
            {
                string[] trashArray = TextBox_trashFile.Text.Split('.');
                trashFile = $"{trashArray[0]}.dlv";
                TextBox_trashFile.IsEnabled = false;
            }
            else
            {
                TextBox_trashFile.Text = null;
                TextBox_trashFile.IsEnabled = true;
                CheckBox_addTrashFile.IsChecked = false;
            }
        }
        private void CheckBox_addStartDialog_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_addStartDialog.IsChecked == true)
            {
                startDialog = ComboBox_startDialog.Items[ComboBox_startDialog.SelectedIndex].ToString();
                ComboBox_startDialog.IsEnabled = false;
            }
            else
            {
                startDialog = null;
                ComboBox_startDialog.IsEnabled = true;
            }
        }
        private void CheckBox_addEndDialog_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_addEndDialog.IsChecked == true)
            {
                endDialog = ComboBox_endDialog.Items[ComboBox_endDialog.SelectedIndex].ToString();
                ComboBox_endDialog.IsEnabled = false;
            }
            else
            {
                endDialog = null;
                ComboBox_endDialog.IsEnabled = true;
            }
        }
        private void Button_selectNextFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileDlag.Filter;
            openFileDialog.DefaultExt = FileDlag.DefaultExt;
            openFileDialog.Title = "Открыть игровой файл";

            if (openFileDialog.ShowDialog() == true)
            {
                TextBox_fileTranslate.Text = GetFile(openFileDialog.FileName);
                pathToNextDlag = openFileDialog.FileName;
            }
        }
        private string CheckValidation()
        {
            if (pathToDlag == null)
                return "Отсутствует игровой файл";
            if (pathToDlt == null)
                return "Отсутствует файл перевода";

            string[] pathDlag = pathToDlag.Split("\\");
            string[] pathDlt = pathToDlt.Split("\\");

            for (int i = 0; i < pathDlag.Length-1; i++)
                if (pathDlag[i] != pathDlt[i])
                    return "Диреткории игрового и файла перевода различны";

            if (startDialog == null || CheckBox_addStartDialog.IsChecked == false)
                return "Не указан начальный файл";

            if (endDialog == null || CheckBox_addEndDialog.IsChecked == false)
                return "Не указан конечной файл";

            return null;
        }

        private void AddText(int idText, string text)
        {
            this.allTexts.Add(idText, text);
        }
        private bool Write()
        {
            dialogDTO = new DialogDTO();

            dialogDTO.textPaths = new TextPathDTO[1];
            dialogDTO.textPaths[0] = new TextPathDTO();
            dialogDTO.textPaths[0].path = pathToDlt.Split("\\")[pathToDlt.Split("\\").Length - 1]; // файл в этой же директории
            dialogDTO.textPaths[0].language = dialog.language;

            bool start = false, end = false;
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i] == startDialog)
                {
                    dialogDTO.firstLineId = i;
                    start = true;
                }
                if (elements[i] == endDialog)
                {
                    dialogDTO.lastLineId = i;
                    end = true;
                }
                if (start && end)
                    break;
            }

            dialogDTO.nextDialogPath = pathToNextDlag;

            int id = 0;
            dialogDTO.dialogLines = new DialogLineDTO[dialog.elements.Length];
            for (int i = 0; i < dialogDTO.dialogLines.Length; i++)
            {
                ref DialogLineDTO dialogLine = ref dialogDTO.dialogLines[i];
                dialogLine = new DialogLineDTO();

                dialogLine.id = dialog.elements[i].idElement;
                dialogLine.nextLineId = dialog.elements[i].question.nextElement.idElement;
                if (dialog.elements[i].pathToImage != null)
                    dialogLine.pathToImage = dialog.elements[i].pathToImage.Split("\\")[dialog.elements[i].pathToImage.Split("\\").Length - 1];
                else
                    dialogLine.pathToImage = null;

                if (dialog.elements[i].pathToSound != null)
                    dialogLine.pathToSound = dialog.elements[i].pathToSound.Split("\\")[dialog.elements[i].pathToSound.Split("\\").Length - 1];
                else
                    dialogLine.pathToSound = null;

                dialogLine.textId = id;
                AddText(id, dialog.elements[i].question.text);
                id++;

                dialogLine.options = new OptionDTO[dialog.elements[i].answers.Length];

                for (int j = 0; j < dialogLine.options.Length; j++)
                {
                    ref OptionDTO option = ref dialogLine.options[j];
                    option = new OptionDTO();

                    option.id = j; // локальный id
                    option.nextLineId = dialog.elements[i].answers[j].nextElement.idElement;

                    option.textId = id;
                    AddText(id, dialog.elements[i].answers[j].text);
                    id++;
                }
            }

            for (int i = 0; i < dialogDTO.dialogLines.Length; i++)
            {
                ref DialogLineDTO dialogLine = ref dialogDTO.dialogLines[i];

                for (int j = 0; j < dialogLine.options.Length; j++)
                {
                    ref OptionDTO option = ref dialogLine.options[j];

                    option.requiredAnswers = new RequiredAnswerDTO[dialog.elements[i].answers[j].requests.Length];
                    for (int g = 0; g < option.requiredAnswers.Length; g++)
                    {
                        ref RequiredAnswerDTO required = ref option.requiredAnswers[g];

                        required = new RequiredAnswerDTO();

                        required.dialogLineId = dialog.elements[i].answers[j].requests[g].idElement;

                        int optionId = -1;
                        ref ElementDFD el = ref dialog.Search(required.dialogLineId);
                        for (int k = 0; k < el.answers.Length; k++)
                            if (el.answers[k] != dialog.elements[i].answers[j].requests[g])
                                optionId = k;
                        if (optionId != -1)
                            required.optionId = optionId;
                        else
                        {
                            MessageBox.Show($"У {dialogLine.id} объекта в ответе {option.id} отсутствует запрос {g}", "Ошибка");
                            return false;
                        }

                        if (required.optionId == -1)
                        {
                            MessageBox.Show($"Запрос {g} элемента {i} в ответе {j} не был найден", "Ошибка");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private int GetIdRequiredOption(string text)
        {
            int id = 0;
            for (int i = 0; i < allTexts.Count; i++)
                if (allTexts[i] == text)
                {
                    id = i;
                    break;
                }

            foreach (var dialogLine in dialogDTO.dialogLines)
                for (int i = 0; i < dialogLine.options.Length; i++)
                    if (dialogLine.options[i].textId == id)
                        return i;
            return -1;
        }
        private bool sr(SayingElementDFD first)
        {
            if (first.text == null && first.type == TypeSayingElementDFD.none && first.idElement == -1 && (first.requests == null || first.requests.Length == 0))
                return true;
            else
                return false;
        }
        private string GetFile(string text)
        {
            string[] pathToFile = text.Split('\\');
            string file = pathToFile[pathToFile.Length - 1];

            Array.Resize(ref pathToFile, pathToFile.Length - 1);
            InitializeDirectoryes(String.Join('\\', pathToFile));

            return file;
        }
    }
}
