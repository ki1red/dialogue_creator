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
        public bool added = false;
        private WPFtoDFD modelView;
        public MainWindow(WPFtoDFD modelView)
        {
            InitializeComponent();

            this.modelView = modelView;
            this.gb_add_answers.IsEnabled = false;
            this.gb_add_image.IsEnabled = false;
            this.gb_add_sound.IsEnabled = false;

            CheckBox_sound.Click += CheckBox_sound_Click;
            CheckBox_image.Click += CheckBox_image_Click;
            CheckBox_answers.Click += CheckBox_answers_Click;
            Button_addDialog.Click += Button_addDialog_Click;
            Button_importSound.Click += Button_importSound_Click;
            Button_importImage.Click += Button_importImage_Click;
            Button_addAnswer.Click += Button_addAnswer_Click;
            Button_delAnswer.Click += Button_delAnswer_Click;
        }

        // ===========================================================================================================================
        // ============================= РАБОТА С ОСНОВНЫМИ ЭЛЕМЕНТАМИ УПРАВЛЕНИЯ ====================================================
        // ===========================================================================================================================

        private void CheckBox_sound_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_sound.IsChecked == true)
                gb_add_sound.IsEnabled = true;
            else
            {
                gb_add_sound.IsEnabled = false;
            }
        }
        private void CheckBox_image_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_image.IsChecked == true)
                gb_add_image.IsEnabled = true;
            else
                gb_add_image.IsEnabled = false;
        }
        private void CheckBox_answers_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_answers.IsChecked == true)
                gb_add_answers.IsEnabled = true;
            else
                gb_add_answers.IsEnabled = false;
        }
        private void Button_addDialog_Click(object sender, RoutedEventArgs e)
        {
            string check = FieldValidation();
            if (check != null)
            {
                MessageBox.Show(check);
                return;
            }

            string author; string question; string pathToSound; string pathToImage; string[] answers;

            author = TextBox_author.Text;
            question = TextBox_question.Text;

            if (Label_imagePath.Content != null && CheckBox_image.IsChecked == true)
                pathToImage = Label_imagePath.Content as string;
            else
                pathToImage = "NULL";

            if (Label_soundPath.Content != null && CheckBox_sound.IsChecked == true)
                pathToSound = Label_soundPath.Content as string;
            else
                pathToSound = "NULL";

            if (CheckBox_answers.IsChecked == true)
            {
                answers = new string[ComboBox_answers.Items.Count];
                ComboBox_answers.Items.CopyTo(answers, 0);
            }
            else
                answers = new string[0];

            modelView.AddElementDFDWithoutConnection(author, question, pathToSound, pathToImage, answers);
            modelView.SerializationDFD();
            added = true;

            this.Close();
        }
        private void Button_importImage_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                Label_imagePath.Content = openFileDialog.FileName;
            }
            else
                return;
        }
        private void Button_importSound_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                Label_soundPath.Content = openFileDialog.FileName;
            }
            else
                return;
        }
        private void Button_addAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_answer.Text.Length == 0)
            {
                MessageBox.Show("Не заполнено текстовое поле ответа");
                return;
            }
            else if (CheckedComboBoxAnswers(TextBox_answer.Text))
            {
                MessageBox.Show("Такой ответ уже существует");
                return;
            }

            ComboBox_answers.Items.Add(TextBox_answer.Text);
        }
        private void Button_delAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_answers.Items.Count == 0 || (ComboBox_answers.SelectedItem as string).Length <= 0)
            {
                MessageBox.Show("Не выбран текст ответа для удаления");
                return;
            }

            ComboBox_answers.Items.Remove(ComboBox_answers.SelectedItem);
        }

        // ===========================================================================================================================
        // ======================================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ===========================================================
        // ===========================================================================================================================

        private bool CheckedComboBoxAnswers(string text)
        {
            foreach (var item in ComboBox_answers.Items)
            {
                if (text == item as string)
                    return true;
            }
            return false;
        }
        private string FieldValidation()
        {
            if (TextBox_author.Text.Length == 0)
                return "Не указано имя автора";

            if (TextBox_question.Text.Length == 0)
                return "Не указан текст вопроса";

            if (CheckBox_sound.IsChecked == true)
            {
                if (Label_soundPath.Content as string == "None")
                    return "Не указан звуковой файл";
            }
            if (CheckBox_image.IsChecked == true)
            {
                if (Label_imagePath.Content as string == "None")
                    return "Не указан файл изображения";
            }

            if (CheckBox_answers.IsChecked == true)
            {
                if (ComboBox_answers.Items.Count == 0)
                    return "Не указаны ответы к диалогу";
            }

            return null;
        }
    }
}
