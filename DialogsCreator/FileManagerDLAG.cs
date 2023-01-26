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
        ru = 0,
        en = 1,
        de = 2
    }
    public class FileManagerDLAG
    {
        
        public string file { get; private set; } = null;
        public string path { get; private set; } = Environment.CurrentDirectory;
        // public string fileL { get; private set; } = Environment.CurrentDirectory;
        public Language lang;

        public const string filter = $"Develop files dialogues (*.{type})|*.{type}";
        //private const string typeL = ".dl";
        public const string type = "dfd";

        private void SelectFile(string path)
        {
            string[] pathToFile = path.Split('\\');

            string[] tmpfile = pathToFile[pathToFile.Length - 1].Split('.');

            this.file = tmpfile[0]; // название файла без расширения

            this.path = "";
            for (int i = 0; i < pathToFile.Length - 1; i++)
                this.path += pathToFile[i] + "\\"; // путь к файлу без самого файла
        }

        public FileManagerDLAG(Language language) { lang = language; }

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
                //myStream = new FileStream(fileL, FileMode.Create);
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

        public void SaveFile(string dialog)
        {
            if (file == null)
                throw new Exception("При сохранении файла обнаружено отсутствие файла");

            //Stream myStream;

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = path;
            //saveFileDialog.FileName = file;
            //saveFileDialog.DefaultExt = type;
            //saveFileDialog.ShowDialog();
            //myStream = saveFileDialog.OpenFile();
            //myStream.Close();
            File.WriteAllText($"{path}{file}.{type}", dialog);
            // TODO добавить помещение структуры в файл и сохранение
        }

        public void SaveAsFile(string path, string dialog)
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
                File.WriteAllText($"{path}{file}.{type}", dialog);

                // TODO добавить помещение структуры в файл и сохранение
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
