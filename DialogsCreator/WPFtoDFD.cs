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

        private DialogDFDs dialogSecure { get; set; }
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
            dialogSecure = JsonConvert.DeserializeObject<DialogDFDs>(text);
            if (dialogSecure == null)
            {
                dialogSecure = new DialogDFDs();
                dialogSecure.language = manager.language.ToString();
            }
            else
                manager.language = manager.ToLanguage(dialogSecure.language);

            dialog.Clone(dialogSecure);// TODO проверить
            for (int i = 0; i < dialog.elements.Length; i++)
            {
                ref ElementDFD element = ref dialog.elements[i];
                element.question.SetLinkeds(dialog);

                for (int j = 0; j < element.answers.Length; j++)
                {
                    ref SayingElementDFD sayingElement = ref element.answers[j];
                    sayingElement.SetLinkeds(dialog);
                }
            }
            id = GetIdLastElement();
        }
        public void SerializationDFD(string path = null)
        {
            if (!manager.isOpen)
                throw new Exception("File is close");

            dialogSecure.Clone(dialog);

            string json = JsonConvert.SerializeObject(dialogSecure, Formatting.Indented);

            if (path == null)
                manager.SaveFile(json);
            else
                if (manager.SaveAsFile(path, json) == false)
                    return;
        }
        // Добавление объекта без связей
        public void AddEmptyElement(ElementDFD element)
        {
            if (id == -1)
                throw new Exception("Не загружен dfd файл");

            element.idElement = ++id;
            element.question.idElement = element.idElement;
            for (int i = 0; i < element.answers.Length; i++)
                element.answers[i].idElement = element.idElement;

            dialog.Add(element);
        }
        // Установка координат объекту View
        public void ReplaceCoords(ref ElementDFD element, Point pointNew)
        {
            element.point = pointNew; // TODO тестить
        }
        public bool DeleteId(int id)
        {
            if (id > this.id)
                return false;
            
            ref ElementDFD delElement = ref this.dialog.elements[id-1];

            for (int i = 1; i <= this.id; i++)
            {
                if (i-1 == id-1)
                    continue;

                ref ElementDFD element = ref this.dialog.elements[i-1];

                if (element.idElement > id)
                    element.idElement = element.idElement - 1; // уменьшение id

                ref SayingElementDFD question = ref element.question;

                for (int j = 0; j < question.requests.Length; j++) // если в вопросе есть связь с удаляемым элементом
                {
                    ref SayingElementDFD request = ref question.requests[j];
                    if (request == delElement.question) // если есть связь с вопросом удаляемого элемента
                        request = null;
                    for (int g = 0; g < delElement.answers.Length; g++) // если есть связь с ответом удаляемого элемента
                    {
                        if (request == delElement.answers[g])
                            request = null;
                    }
                }
                ref SayingElementDFD nextElement = ref question.nextElement;
                if (nextElement == delElement.question) // если есть связь с вопросом удаляемого элемента
                    nextElement = null;
                for (int g = 0; g < delElement.answers.Length; g++) // если есть связь с ответом удаляемого элемента
                {
                    if (nextElement == delElement.answers[g])
                        nextElement = null;
                }




                for (int j = 0; j < element.answers.Length; j++) // если в ответах есть связь с удаляемым элементом
                {
                    ref SayingElementDFD answer = ref element.answers[j];

                    for (int g = 0; g < answer.requests.Length; g++)
                    {
                        ref SayingElementDFD request = ref answer.requests[g];

                        if (request == delElement.question) // если есть связь с вопросом удаляемого элемента
                            request = null;

                        for (int h = 0; h < delElement.answers.Length; h++) // смотрим конкретный вопрос
                        {
                            if (request == delElement.answers[h]) // если есть связь с вопросом удаляемого элемента
                                request = null;
                        }
                    }

                    nextElement = ref answer.nextElement;
                    if (nextElement == delElement.question) // если есть связь с вопросом удаляемого элемента
                        nextElement = null;
                    for (int g = 0; g < delElement.answers.Length; g++) // если есть связь с ответом удаляемого элемента
                    {
                        if (nextElement == delElement.answers[g])
                            nextElement = null;
                    }
                }
                
            }

            this.dialog.Delete(delElement);
            this.id--;
            return true;
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
