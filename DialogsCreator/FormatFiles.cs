using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsCreator
{
    

    // Игровой файл
    public struct FileDlag
    {
        public const string Filter = $"Game files dialogues (*.{DefaultExt})|*.{DefaultExt}";
        public static string InitialDirectory = "";
        public const string DefaultExt = "dlag";
        public const string Title = "Создать игровой файл";
    }

    // Файл перевода
    public struct FileDlt
    {
        public const string Filter = $"Translate files dialogues (*.{DefaultExt})|*.{DefaultExt}";
        public static string InitialDirectory = "";
        public const string DefaultExt = "dlt";
        public const string Title = "Создать файл перевода";
    }

    // Мусорный файл
    public struct FileDlv
    {
        public const string Filter = $"View files dialogues (*.{DefaultExt})|*.{DefaultExt}";
        public static string InitialDirectory = "";
        public const string DefaultExt = "dlv";
        public const string Title = "Создать мусорный файл";
    }

    // Файл изображений
    public struct FileImage
    {
        public const string Filter = "";
        public static string InitialDirectory = "";
        public const string DefaultExt = "";
        public const string Title = "Открыть файл изображения";
    }

    // Звуковой файл
    public struct FileSound
    {
        public const string Filter = "";
        public static string InitialDirectory = "";
        public const string DefaultExt = "";
        public const string Title = "Открыть звуковой файл";
    }
}
