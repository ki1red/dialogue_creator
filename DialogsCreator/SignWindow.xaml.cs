using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для SignWindow.xaml
    /// </summary>
    public partial class SignWindow : Window
    {
        Roots root { get; set; } = new Roots();
        public SignWindow()
        {
            InitializeComponent();

            painter.IsEnabled = false;
            translator.IsEnabled = false;
        }

        private void Choice_Click(object sender, RoutedEventArgs e)
        {
            // Блокирует / разблокирует кнопку в зависимости от выбранного типа пользователя
            ButtonSign.IsEnabled = root.SetRoot((sender as RadioButton).Name);
        }

        private void ButtonSign_Click(object sender, RoutedEventArgs e)
        {
            RunWindow();
        }

        private void RunWindow()
        {

            switch (root.typeUser)
            {
                case TypeUser.scenarist:
                    this.Hide();
                    VisualBindings window = new VisualBindings();
                    window.ShowDialog();
                    window.Closed += ThisClose(window, null);
                    break;
                case TypeUser.painter:
                    WebWindow windowW = new WebWindow();
                    windowW.ShowDialog();
                    windowW.Closed += ThisClose(windowW, null);
                    break;
                case TypeUser.translator:
                    this.Hide();
                    TranslateWindow windowT = new TranslateWindow();
                    windowT.ShowDialog();
                    windowT.Closed += ThisClose(windowT, null);
                    break;
                default:
                    MessageBox.Show("Ошибка. Отсутствует окно для данного типа пользователя");
                    break;
            }
            
        }

        private EventHandler ThisClose(object sender, EventArgs e)
        {
            this.Close();
            return null;
        }
    }
}
