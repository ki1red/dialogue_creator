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
        // ===========================================================================================================================
        // ================================= НЕИЗМЕННЫЕ ЗНАЧЕНИЯ ДЛЯ КЛАССА ==========================================================
        // ===========================================================================================================================

        public const uint countLanguages = 4;
        private Dictionary<string, string> titles = new Dictionary<string, string>() { { "open" , "Открыть файл"}, { "create", "Создать файл" }, { "save as", "Сохранить файл как" } };

        public const string filter = $"Develop files dialogues (*.{type})|*.{type}";
        public const string type = "dfd";

        // ===========================================================================================================================
        // ================================= ПЕРЕМЕННЫЕ ДЛЯ ВЗАИМОДЕЙСТВИЯ С ФАЙЛОМ ==================================================
        // ===========================================================================================================================

        public string file { get; private set; } = null;
        public string path { get; private set; } = Environment.CurrentDirectory;
        public Language language;


        public bool isOpen;
        public bool isSave;
        //public bool isEmpty;

        public FileManager(Language language = Language.none) 
        { 
            this.language = language; 
            this.isOpen = false; 
            this.isSave = false;
            //this.isEmpty = true;
        }

        // ===========================================================================================================================
        // ================================= ОСНОВНАЯ МЕТОДЫ ДЛЯ РАБОТЫ С ФАЙЛОМ =====================================================
        // ===========================================================================================================================

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
                //if (isEmpty)
                //    DeleteFile();

                Stream myStream;
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    myStream.Close();
                    SelectFile(saveFileDialog.FileName);
                    isOpen = true;
                    isSave = false;
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
                //if (isEmpty)
                  //  DeleteFile();

                SelectFile(openFileDialog.FileName);
                isOpen = true;
                isSave = true;
                return true;
            }
            return false;
        }
        public void SaveFile(string data)
        {
            if (!isOpen)
                throw new Exception("File not loaded");

            //bool oldIsSave = isSave;
            //if (data == null || data == "")
            //    isEmpty = true;
            //else
            //    isEmpty = false;

            File.WriteAllText($"{path}{file}.{type}", data);
            isSave = true;
        }
        public bool SaveAsFile(string path, string data)
        {
            if (!isOpen)
                throw new Exception("File not loaded");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.Filter = filter;
            saveFileDialog.DefaultExt = type;
            saveFileDialog.Title = titles["save as"];

            if (saveFileDialog.ShowDialog() == true)
            {
                SelectFile(saveFileDialog.FileName);
                File.WriteAllText($"{path}{file}.{type}", data);
                isOpen = true;
                isSave = true;
                return true;
            }
            return false;
        }
        public bool CloseFile()
        {
            if (isOpen)
            {
                file = null;
                language = Language.none;
                isOpen = false;
                isSave = false;
                //isEmpty = true;
                return true;
            }
            else
                return false;
        }
        public bool DeleteFile()
        {
            if (!isOpen)
                return false;
            else
            {
                File.Delete($"{path}{file}.{type}");
                return true;
            }
        }

        // ===========================================================================================================================
        // ====================================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =============================================================
        // ===========================================================================================================================

        private void SelectFile(string pathAndFile)
        {
            string[] pathToFile = pathAndFile.Split('\\');

            string[] tmpfile = pathToFile[pathToFile.Length - 1].Split('.'); // обращение к последнему элементу, где находится название файла
            this.file = tmpfile[0]; // название файла без расширения

            Array.Resize(ref pathToFile, pathToFile.Length - 1);
            this.path = "";
            this.path = String.Join("\\", pathToFile) + "\\";
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
                    throw new Exception("It's not language");
            }
        }
    }
}
