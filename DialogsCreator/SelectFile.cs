using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    internal class SelectFile
    {
        public string file { get; set; } = Environment.CurrentDirectory;
        public string filter { get; set; } = "Dialog files (*.dlag)|*.dlag";

        private void select_file(string path)
        {
            file = path;
        }

        public void create_file()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = file;

            if (saveFileDialog.ShowDialog() == true)
            {
                select_file(saveFileDialog.FileName);
                Stream myStream = saveFileDialog.OpenFile();
            }
            else
                return;
        }

        public void save_file()
        {
            Stream myStream;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = file;
            myStream = saveFileDialog.OpenFile();

            // TODO добавить помещение структуры в файл и сохранение
        }

        public void open_file()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter; // фильтры файлов
            openFileDialog.InitialDirectory = file; // путь по умолчанию для окна
            if (openFileDialog.ShowDialog() == true)
            {
                select_file(openFileDialog.FileName);
            }
            else
                return;
        }

        public void save_as_file()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = file;

            if (saveFileDialog.ShowDialog() == true)
            {
                select_file(saveFileDialog.FileName);
                Stream myStream = saveFileDialog.OpenFile();

                // TODO добавить помещение структуры в файл и сохранение
            }
            else
                return;
        }
    }
}
