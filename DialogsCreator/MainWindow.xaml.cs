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
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;


namespace DialogsCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WPFtoDFD modelView;
        public MainWindow(ref WPFtoDFD modelView)
        {
            InitializeComponent();

            this.modelView = modelView;
            //this.TranslatingFile.IsEnabled = false;
            //this.SaveFile.IsEnabled = false;
            //this.SaveAsFile.IsEnabled = false;
            //this.visBindings.IsEnabled = false;
            this.gb_add_answers.IsEnabled = false;
            this.gb_add_image.IsEnabled = false;
            this.gb_add_sound.IsEnabled = false;
        }

        // ===========================================================================================================================
        // ============================= РАБОТА С ОСНОВНЫМИ ЭЛЕМЕНТАМИ УПРАВЛЕНИЯ ====================================================
        // ===========================================================================================================================

        private void cb_sound_Click(object sender, RoutedEventArgs e)
        {
            if (cb_sound.IsChecked == true)
                gb_add_sound.IsEnabled = true;
            else
            {
                gb_add_sound.IsEnabled = false;
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

        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            string check = FieldValidation();
            if (check != null)
            {
                MessageBox.Show(check);
                return;
            }

            string author; string question; string pathToSound; string pathToImage; string[] answers;

            author = textbox_name.Text;
            question = textbox_text.Text;

            if (label_image.Content != null && cb_image.IsChecked == true)
                pathToImage = label_image.Content as string;
            else
                pathToImage = "NULL";

            if (label_sound.Content != null && cb_sound.IsChecked == true)
                pathToSound = label_sound.Content as string;
            else
                pathToSound = "NULL";

            if (cb_answers.IsChecked == true)
            {
                answers = new string[combobox_answers.Items.Count];
                combobox_answers.Items.CopyTo(answers, 0);
            }
            else
                answers = new string[0];

            modelView.AddElementDFDWithoutConnection(author, question, pathToSound, pathToImage, answers);
        }

        private void button_import_image_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                label_image.Content = openFileDialog.FileName;
            }
            else
                return;
        }

        private void button_import_sound_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                label_sound.Content = openFileDialog.FileName;
            }
            else
                return;
        }

        private void button_add_answer_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_answer.Text.Length == 0)
            {
                MessageBox.Show("Не заполнено текстовое поле ответа");
                return;
            }
            else if (CheckedComboBoxAnswers(textbox_answer.Text))
            {
                MessageBox.Show("Такой ответ уже существует");
                return;
            }

            combobox_answers.Items.Add(textbox_answer.Text);
        }

        private void button_del_answer_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_answers.Items.Count == 0 || (combobox_answers.SelectedItem as string).Length <= 0)
            {
                MessageBox.Show("Не выбран текст ответа для удаления");
                return;
            }

            combobox_answers.Items.Remove(combobox_answers.SelectedItem);
        }

        // ===========================================================================================================================
        // ======================================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ===========================================================
        // ===========================================================================================================================
        private bool CheckedComboBoxAnswers(string text)
        {
            foreach (var item in combobox_answers.Items)
            {
                if (text == item as string)
                    return true;
            }
            return false;
        }

        private string FieldValidation()
        {
            if (textbox_name.Text.Length == 0)
                return "Не указано имя автора";

            if (textbox_text.Text.Length == 0)
                return "Не указан текст вопроса";

            if (cb_sound.IsChecked == true)
            {
                if (label_sound.Content as string == "None")
                    return "Не указан звуковой файл";
            }
            if (cb_image.IsChecked == true)
            {
                if (label_image.Content as string == "None")
                    return "Не указан файл изображения";
            }

            if (cb_answers.IsChecked == true)
            {
                if (combobox_answers.Items.Count == 0)
                    return "Не указаны ответы к диалогу";
            }

            return null;
        }
    }
}
