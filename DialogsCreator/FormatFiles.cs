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
        public static string InitialDirectory = null;
        public const string DefaultExt = "dlag";
        public const string Title = "Создать игровой файл";
    }

    // Файл перевода
    public struct FileDlt
    {
        public const string Filter = $"Translate files dialogues (*.{DefaultExt})|*.{DefaultExt}";
        public static string InitialDirectory = null;
        public const string DefaultExt = "dlt";
        public const string Title = "Создать файл перевода";
    }

    // Мусорный файл
    public struct FileDlv
    {
        public const string Filter = $"View files dialogues (*.{DefaultExt})|*.{DefaultExt}";
        public static string InitialDirectory = null;
        public const string DefaultExt = "dlv";
        public const string Title = "Создать мусорный файл";
    }

    // Файл изображений
    public struct FileImage
    {
        public const string Filter = "Image Files(*.jpeg;*.jpg;*.png;*.gif)|*.jpeg;*.jpg;*.png*;.gif";
        public static string InitialDirectory = null;
        public const string DefaultExt = "*.png";
        public const string Title = "Открыть файл изображения";
    }

    // Звуковой файл
    public struct FileSound
    {
        public const string Filter = "Audio Files(*.mp3;*.wav;*.m4a;*.aac)|*.mp3;*.wav;*.m4a;*.aac";
        public static string InitialDirectory = null;
        public const string DefaultExt = "*.mp3";
        public const string Title = "Открыть звуковой файл";
    }
}
