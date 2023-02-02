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
    public class FileManager
    {
        public const uint countLanguages = 4;
        private Dictionary<string, string> titles = new Dictionary<string, string>() { { "open" , "Открыть файл"}, { "create", "Создать файл" }, { "save as", "Сохранить файл как" } };

        public string file { get; private set; } = null;
        public string path { get; private set; } = Environment.CurrentDirectory;
        public Language language;

        public const string filter = $"Develop files dialogues (*.{type})|*.{type}";
        public const string type = "dfd";

        public bool isOpen;

        private void SelectFile(string pathAndFile)
        {
            string[] pathToFile = pathAndFile.Split('\\');

            string[] tmpfile = pathToFile[pathToFile.Length - 1].Split('.'); // обращение к последнему элементу, где находится название файла
            this.file = tmpfile[0]; // название файла без расширения
            
            Array.Resize(ref pathToFile, pathToFile.Length - 1);
            this.path = "";
            this.path = String.Join("\\", pathToFile);
        }

        public FileManager(Language language = Language.none) { this.language = language; isOpen = false; }

        public bool CreateFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.FileName = file;
            saveFileDialog.DefaultExt = type;
            saveFileDialog.Title = titles["create"];

            if (saveFileDialog.ShowDialog() == true)
            {
                Stream myStream;
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    myStream.Close();
                    SelectFile(saveFileDialog.FileName);
                    isOpen = true;
                    return true;
                }
            }
            return false;
        }

        public bool OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.InitialDirectory = path;
            openFileDialog.FileName = file;
            openFileDialog.DefaultExt = type;
            openFileDialog.Title = titles["open"];

            if (openFileDialog.ShowDialog() == true)
            {
                SelectFile(openFileDialog.FileName);
                isOpen = true;
                return true;
            }
            return false;
        }

        public void SaveFile(string data)
        {
            if (file == null)
                throw new Exception("При сохранении файла обнаружено отсутствие файла");

            File.WriteAllText($"{path}{file}.{type}", data);
        }

        public bool SaveAsFile(string path, string data)
        {
            if (file == null)
                throw new Exception("При сохранении файла обнаружено отсутствие файла");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.Filter = filter;
            saveFileDialog.DefaultExt = type;
            saveFileDialog.Title = titles["save as"];

            if (saveFileDialog.ShowDialog() == true)
            {
                SelectFile(saveFileDialog.FileName);
                File.WriteAllText($"{path}{file}.{type}", data);
                return true;
            }
            return false;
        }

        public void CloseFile()
        {
            file = null;
            language = Language.none;
            isOpen = false;
        }

        public Language ToLanguage(string language)
        {
            switch (language)
            {
                case "ru":
                    return Language.ru;
                case "en":
                    return Language.en;
                case "de":
                    return Language.de;
                default:
                    return Language.none;
            }
        }
    }
}
