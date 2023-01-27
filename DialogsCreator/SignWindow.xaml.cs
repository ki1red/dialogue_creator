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
                    MainWindow mainWindow = new MainWindow(root);
                    mainWindow.ShowDialog();
                    mainWindow.Closed += ThisClose(mainWindow, null);
                    break;
                case TypeUser.admin:
                    MessageBox.Show("Окно пока не готово");
                    break;
                case TypeUser.translator:
                    MessageBox.Show("Окно пока не готово");
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
