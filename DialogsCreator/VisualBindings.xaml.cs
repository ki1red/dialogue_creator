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
    /// Логика взаимодействия для VisualBindings.xaml
    /// </summary>
    public partial class VisualBindings : Window
    {
        // pathToFile - Dialog.dlag
        // название файла с озвучкой будет Dialog_ru.dl, который будет лежать в той же папке, что и .dlag
        // при закрытии формы файлы автоматически будут сохраняться, либо сделай кнопку
        public VisualBindings(string pathToFile)
        {
            InitializeComponent();
        }
    }
}
