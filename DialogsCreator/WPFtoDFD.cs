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

namespace DialogsCreator
{
    public class WPFtoDFD
    {
        public FileManagerDLAG fileManager { get; private set; }
        public DialogDFD dialog { get; private set; }
        public int id { get; private set; }
        public WPFtoDFD(FileManagerDLAG fileManager)
        {
            this.fileManager = fileManager;
            this.dialog = new DialogDFD();
            this.id = -1;
        }
        public void DesirializationDFD()    // TODO сделать обработку если никакой файл не выбран
        {
            string text;
            using (StreamReader reader = new StreamReader($"{fileManager.path}{fileManager.file}.{FileManagerDLAG.type}")) // полный путь до файла
            {
                text = reader.ReadToEnd();
            }
            dialog = JsonConvert.DeserializeObject<DialogDFD>(text);
            if (dialog == null)
            {
                dialog = new DialogDFD();
                dialog.language = fileManager.language.ToString();
            }

            id = GetIdLastElement();
        }
        public void SerializationDFD()
        {
            string json = JsonConvert.SerializeObject(dialog);

            fileManager.SaveFile(json);
        }
        public void SerializationDFD(string path)
        {
            string json = JsonConvert.SerializeObject(dialog);

            fileManager.SaveAsFile(path, json);
        }
        public void AddElementDFDWithoutConnection(string author, string question, string pathToSound, string pathToImage, string[] answers)
        {
            if (id == -1)
                throw new Exception("Не загружен dfd файл");

            ElementDFD element = new ElementDFD();
            element.idElement = ++id;
            element.author = author;

            SayingElementDFD sayingElement = new SayingElementDFD();
            sayingElement.text = question;
            sayingElement.nextIdElement = -1;
            sayingElement.requests = new int[0];

            element.question = sayingElement;

            element.pathToSound = pathToSound;
            element.pathToImage = pathToImage;

            SayingElementDFD[] sayingElements = new SayingElementDFD[answers.Length];
            
            for (int iAnswers = 0; iAnswers <  sayingElements.Length; iAnswers++)
            {
                SayingElementDFD item = new SayingElementDFD(answers[iAnswers], -1, new int[0]);
                sayingElements[iAnswers] = item;
            }

            element.answers = sayingElements;

            dialog.Add(element);
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
