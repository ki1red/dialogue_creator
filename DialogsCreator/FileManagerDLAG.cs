using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DialogsCreator
{
    public enum Language
    {
        none = 0,
        ru = 1,
        en = 2,
        de = 3
    }
    public class FileManagerDLAG
    {
        public const uint countLanguages = 4;

        public string file { get; private set; } = null;
        public string path { get; private set; } = Environment.CurrentDirectory;
        public Language language;

        public const string filter = $"Develop files dialogues (*.{type})|*.{type}";
        public const string type = "dfd";

        private void SelectFile(string pathAndFile)
        {
            string[] pathToFile = pathAndFile.Split('\\');

            string[] tmpfile = pathToFile[pathToFile.Length - 1].Split('.');

            this.file = tmpfile[0]; // название файла без расширения

            this.path = "";
            for (int i = 0; i < pathToFile.Length - 1; i++)
                this.path += pathToFile[i] + "\\"; // путь к файлу без самого файла
        }

        public FileManagerDLAG(Language language = Language.none) { this.language = language; }

        public void CreateFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.FileName = file;
            saveFileDialog.DefaultExt = type;

            if (saveFileDialog.ShowDialog() == true)
            {
                SelectFile(saveFileDialog.FileName);
                Stream myStream = saveFileDialog.OpenFile();
                myStream.Close();
            }
            else
                return;
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.InitialDirectory = path;
            openFileDialog.FileName = file;
            openFileDialog.DefaultExt = type;

            if (openFileDialog.ShowDialog() == true)
            {
                SelectFile(openFileDialog.FileName);
            }
            else
                return;
        }

        public void SaveFile(string data)
        {
            if (file == null)
                throw new Exception("При сохранении файла обнаружено отсутствие файла");

            File.WriteAllText($"{path}{file}.{type}", data);
        }

        public void SaveAsFile(string path, string data)
        {
            if (file == null)
                throw new Exception("При сохранении файла обнаружено отсутствие файла");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.Filter = filter;
            saveFileDialog.DefaultExt = type;

            if (saveFileDialog.ShowDialog() == true)
            {
                SelectFile(saveFileDialog.FileName);
                File.WriteAllText($"{path}{file}.{type}", data);
            }
            else
                return;
        }
        /*
        public bool CheckIsNotEmptyFile()
        {
            string[] path = file.Split('\\');

            string dfd = "";
            for (int i = path[path.Length - 1].Length - 1; i > path[path.Length - 1].Length - 5; i--)
                dfd += path[path.Length - 1][i];
            dfd = new string(dfd.Reverse().ToArray());


            if (dfd == $".{type}")
                return true;
            else
                return false;
        }*/
    }
}
