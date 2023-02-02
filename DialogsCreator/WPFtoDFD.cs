using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Windows.Shapes;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace DialogsCreator
{
    public class WPFtoDFD
    {
        private FileManager manager { get; set; }
        public DialogDFD dialog { get; private set; }
        public int id { get; private set; }
        public WPFtoDFD(FileManager fileManager)
        {
            this.manager = fileManager;
            this.dialog = new DialogDFD();
            this.id = -1;
        }
        public void DesirializationDFD()
        {
            if (!manager.isOpen)
                throw new Exception("File is close");

            string text;
            using (StreamReader reader = new StreamReader($"{manager.path}{manager.file}.{FileManager.type}")) // полный путь до файла
            {
                text = reader.ReadToEnd();
            }
            dialog = JsonConvert.DeserializeObject<DialogDFD>(text);
            if (dialog == null)
            {
                dialog = new DialogDFD();
                dialog.language = manager.language.ToString();
            }
            manager.language = manager.ToLanguage(dialog.language); // TODO нахуй надо?
            id = GetIdLastElement();
        }
        public void SerializationDFD(string path = null)
        {
            if (!manager.isOpen)
                throw new Exception("File is close");

            string json = JsonConvert.SerializeObject(dialog);

            if (path == null)
                manager.SaveFile(json);
            else
                if (manager.SaveAsFile(path, json) == false)
                    return;
        }
        public void AddEmptyElement(ElementDFD element)
        {
            if (id == -1)
                throw new Exception("Не загружен dfd файл");

            element.idElement = ++id;

            dialog.Add(element);
        }
        public void ReplaceCoords(ref ElementDFD element, Point point)
        {
            element.point = point; // TODO тестить
        }
        public void UpdateLinkeds(ref SayingElementViewDFD sayingViewElement)
        {
            ref ElementDFD element = ref dialog.Search(sayingViewElement.idElement); // TODO тестить
            ref SayingElementDFD sayingElement = ref element.Search(sayingViewElement.elementOld);
            sayingElement = sayingViewElement.elementNew;

            sayingViewElement.UpdateElement(); // TODO возможно, лучше вынести
        }
        private int GetIdLastElement()
        {
            int maxId = 0;
            foreach(var element in dialog.elements)
            {
                if (element.idElement > maxId)
                    maxId = element.idElement;
            }
            return maxId;
        }
        
    }
}
