using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    internal class FileManagerDLAG
    {
        public string file { get; private set; } = Environment.CurrentDirectory;
        public string filter { get; private set; } = "Dialog files (*.dlag)|*.dlag";

        private void SelectFile(string path)
        {
            file = path;
        }

        public FileManagerDLAG() { }

        public void CreateFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = file;

            if (saveFileDialog.ShowDialog() == true)
            {
                SelectFile(saveFileDialog.FileName);
                Stream myStream = saveFileDialog.OpenFile();
            }
            else
                return;
        }

        public void SaveFile()
        {
            if (!CheckIsNotEmptyFile())
                return;

            Stream myStream;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = file;
            myStream = saveFileDialog.OpenFile();

            // TODO добавить помещение структуры в файл и сохранение
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.InitialDirectory = file;
            if (openFileDialog.ShowDialog() == true)
            {
                SelectFile(openFileDialog.FileName);
            }
            else
                return;
        }

        public void SaveAsFile()
        {
            if (!CheckIsNotEmptyFile())
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = file;

            if (saveFileDialog.ShowDialog() == true)
            {
                SelectFile(saveFileDialog.FileName);
                Stream myStream = saveFileDialog.OpenFile();

                // TODO добавить помещение структуры в файл и сохранение
            }
            else
                return;
        }

        public bool CheckIsNotEmptyFile()
        {
            string[] path = file.Split('\\');

            string dlag = "";
            for (int i = path[path.Length - 1].Length - 1; i > path[path.Length - 1].Length - 6; i--)
                dlag += path[path.Length - 1][i];
            dlag = new string(dlag.Reverse().ToArray());


            if (dlag == ".dlag")
                return true;
            else
                return false;
        }
    }
}
