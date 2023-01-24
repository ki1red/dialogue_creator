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
    /// Логика взаимодействия для SignWindow.xaml
    /// </summary>
    public partial class SignWindow : Window
    {
        Roots roots { get; set; }
        public SignWindow()
        {
            InitializeComponent();
            roots = new Roots();
        }

        private void choice_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string root = rb.Content.ToString();
            roots.SetRoot(root);
            button_sign.IsEnabled = true;
        }

        private void buttin_sign_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            MainWindow mainWindow = new MainWindow(roots);
            mainWindow.ShowDialog();
            mainWindow.Closed += ThisClose(mainWindow, null);
            this.Hide();
        }

        private EventHandler ThisClose(object sender, EventArgs e)
        {
            this.Close();
            return null;
        }
    }
}
