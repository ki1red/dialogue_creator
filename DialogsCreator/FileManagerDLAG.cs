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
        public enum language
        {
            ru = 0,
            en = 1,
            de = 2
        }
        public string file { get; private set; } = Environment.CurrentDirectory;
        public string fileL { get; private set; } = Environment.CurrentDirectory;
        public language lang;
        public string filter { get; private set; } = "Dialog files (*.dlag)|*.dlag";

        private void SelectFile(string path)
        {
            file = path;

            string[] pathToFile = path.Split('\\');

            string[] fileLanguage = pathToFile[pathToFile.Length - 1].Split('.');

            fileLanguage[0] = fileLanguage[0] + "_" + lang;
            fileLanguage[1] = ".dl";

            fileL = "";
            for (int i = 0; i < pathToFile.Length - 1; i++)
                fileL += pathToFile[i] + "\\";
            fileL += fileLanguage[0] + fileLanguage[1];
        }

        public FileManagerDLAG(language language) { lang = language; }

        public void CreateFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = file;

            if (saveFileDialog.ShowDialog() == true)
            {
                SelectFile(saveFileDialog.FileName);
                Stream myStream = saveFileDialog.OpenFile();
                myStream = new FileStream(fileL, FileMode.Create);
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
