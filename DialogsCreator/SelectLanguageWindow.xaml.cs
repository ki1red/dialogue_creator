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
    /// Логика взаимодействия для SelectLanguageWindow.xaml
    /// </summary>
    public partial class SelectLanguageWindow : Window
    {
        public Language language = DialogsCreator.Language.none;
        public SelectLanguageWindow()
        {
            InitializeComponent();
            Button_selectLanguage.IsEnabled = false;
        }

        private void Button_selectLanguage_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Choice_Click(object sender, RoutedEventArgs e)
        {
            switch (((sender as RadioButton).Name))
            {
                case "ru":
                    language = DialogsCreator.Language.ru;
                    break;
                case "en":
                    language = DialogsCreator.Language.en;
                    break;
                case "de":
                    language = DialogsCreator.Language.de;
                    break;
                default:
                    MessageBox.Show("Ошибка. Отсутствует такой тип языка");
                    break;
            }

            if (language != DialogsCreator.Language.none)
                Button_selectLanguage.IsEnabled = true;
        }
    }
}
