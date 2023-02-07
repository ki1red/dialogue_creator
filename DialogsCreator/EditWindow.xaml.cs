using Microsoft.Win32;
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
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public ElementDFD element;
        public bool isEdit = false;
        public EditWindow(ElementDFD element)
        {
            InitializeComponent();
            this.element = element;

            InitializeQuestion();
            InintializeImage();
            InintializeSound();
            InitializeAnswers();

            this.Button_addDialog.IsEnabled = true;
            this.Button_addDialog.Click += Button_addDialog_Click;
        }

        public void InintializeImage()
        {
            if (element.pathToImage == null)
            {
                Label_imagePath.Content = null;
                Border_addImage.IsEnabled = false;
                CheckBox_image.IsChecked = false;
            }
            else
            {
                Label_imagePath.Content = element.pathToImage;
                Border_addImage.IsEnabled = true;
                CheckBox_image.IsChecked = true;
            }

            this.CheckBox_image.Click += CheckBox_image_Click;
            this.Button_importImage.Click += Button_importImage_Click;
        }
        public void InintializeSound()
        {
            if (element.pathToSound == null)
            {
                Label_soundPath.Content = null;
                Border_addSound.IsEnabled = false;
                CheckBox_sound.IsChecked = false;
            }
            else
            {
                Label_soundPath.Content = element.pathToSound;
                Border_addSound.IsEnabled = true;
                CheckBox_sound.IsChecked = true;
            }

            this.CheckBox_sound.Click += CheckBox_sound_Click;
            this.Button_importSound.Click += Button_importSound_Click;
        }
        public void InitializeQuestion()
        {
            TextBox_author.Text = element.author;
            TextBox_question.Text = element.question.text;

            TextBox_question.KeyDown += TextBox_question_KeyDown;
        }
        public void InitializeAnswers()
        {

            foreach (SayingElementDFD answer in element.answers)
            {
                TextBox tb = new TextBox();
                tb.Text = answer.text;
                tb.KeyDown += TextBox_question_KeyDown;
                ListBox_answers.Children.Add(tb);
            }

            //ListBox_answers.PreviewMouseLeftButtonDown += ListBox_answers_PreviewMouseLeftButtonDown;
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
        private void Button_addDialog_Click(object sender, RoutedEventArgs e)
        {
            string check = FieldValidation();
            if (check != null)
            {
                MessageBox.Show(check, "Ошибка");
                return;
            }

            element.author = TextBox_author.Text;
            Filling(ref element.question, TextBox_question.Text);
            if (CheckBox_image.IsChecked == true)
                element.pathToImage = Label_imagePath.Content as string;
            else
                element.pathToImage = null;
            if (CheckBox_sound.IsChecked == true)
                element.pathToSound = Label_soundPath.Content as string;
            else
                element.pathToSound = null;

            int i = 0;
            foreach (TextBox answer in ListBox_answers.Children)
            {
                Filling(ref element.answers[i], answer.Text);
                i++;
            }

            isEdit = true;
            this.Close();
        }
        private void CheckBox_sound_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_sound.IsChecked == true)
                Border_addSound.IsEnabled = true;
            else
            {
                Border_addSound.IsEnabled = false;
            }
        }
        private void CheckBox_image_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_image.IsChecked == true)
                Border_addImage.IsEnabled = true;
            else
                Border_addImage.IsEnabled = false;
        }

        private void TextBox_question_KeyDown(object sender, KeyEventArgs e)
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

        private void Filling(ref SayingElementDFD sayingElement, string text)
        {
            sayingElement.text = text;
        }
        private string FieldValidation()
        {
            if (TextBox_author.Text.Length == 0)
                return "Не указано имя автора";

            if (TextBox_question.Text.Length == 0)
                return "Не указан текст вопроса";

            if (CheckBox_sound.IsChecked == true)
            {
                if (Label_soundPath.Content as string == null)
                    return "Не указан звуковой файл";
            }
            if (CheckBox_image.IsChecked == true)
            {
                if (Label_imagePath.Content as string == null)
                    return "Не указан файл изображения";
            }

            foreach (TextBox answer in ListBox_answers.Children)
                if (answer.Text.Length == 0)
                    return "Не указаны ответы к диалогу";

            return null;
        }
    }
}
