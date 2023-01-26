using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DialogsCreator
{
    [Serializable]
    public class DialogDFD
    {
        public string language;
        public ElementDFD[] elements;

        public DialogDFD(string language, ElementDFD[] elements)
        {
            this.language = language;
            this.elements = elements;
        }
        public DialogDFD()
        {
            this.language = "NULL";
            this.elements = new ElementDFD[0];
        }
        public void Add(ElementDFD element)
        {
            Array.Resize(ref elements, elements.Length+1);
            elements[elements.Length-1] = element;
        }
        public void Replace(ref ElementDFD elementOld, ElementDFD elementNew)
        {
            elementOld = elementNew;
        }
        public void Delete(ElementDFD element)
        {
            ElementDFD[] tmp = new ElementDFD[elements.Length - 1];
            int iTmp = 0;
            foreach (var item in elements)
            {
                if (item == element)
                    continue;
                tmp[iTmp++] = item;
            }
            Array.Resize(ref elements, elements.Length - 1);
            tmp.CopyTo(elements, 0);
        }
    }

    [Serializable]
    public class ElementDFD
    {
        public int idElement;
        public string pathToSound;
        public string pathToImage;

        public string author;
        public SayingElementDFD question;
        public SayingElementDFD[] answers;

        public ElementDFD(int idElement, string pathToSound, string pathToImage, string author, SayingElementDFD question, SayingElementDFD[] answers)
        {
            this.idElement = idElement;
            this.pathToSound = pathToSound;
            this.pathToImage = pathToImage;
            this.author = author;
            this.question = question;
            this.answers = answers;
        }
        public ElementDFD()
        {
            this.idElement = -1;
            this.pathToSound = "NULL";
            this.pathToImage = "NULL";
            this.author = "NULL";
            this.question = new SayingElementDFD();
            this.answers = new SayingElementDFD[0];
        }
        public void Add(SayingElementDFD element)
        {
            Array.Resize(ref answers, answers.Length+1);
            answers[answers.Length-1] = element;
        }
        public void Replace(ref SayingElementDFD elementOld, SayingElementDFD elementNew)
        {
            elementOld = elementNew;
        }
        public void Delete(SayingElementDFD element)
        {
            SayingElementDFD[] tmp = new SayingElementDFD[answers.Length - 1];
            int iTmp = 0;
            foreach (var item in answers)
            {
                if (item == element)
                    continue;
                tmp[iTmp++] = item;
            }
            Array.Resize(ref answers, answers.Length - 1);
            tmp.CopyTo(answers, 0);
        }
    }

    [Serializable]
    public class SayingElementDFD
    {
        public string text;
        public int nextIdElement;
        public int[] requests;
        public SayingElementDFD(string text, int nextIdElement, int[] requests)
        {
            this.text = text;
            this.nextIdElement = nextIdElement;
            this.requests = requests;
        }
        public SayingElementDFD()
        {
            this.text = "NULL";
            this.nextIdElement = -1;
            this.requests = new int[0];
        }
        public void Add(int element)
        {
            Array.Resize(ref requests, requests.Length + 1);
            requests[requests.Length - 1] = element;
        }
        public void Replace(ref int elementOld, int elementNew)
        {
            elementOld = elementNew;
        }
        public void Delete(int element)
        {
            int[] tmp = new int[requests.Length - 1];
            int iTmp = 0;
            foreach (var item in requests)
            {
                if (item == element)
                    continue;
                tmp[iTmp++] = item;
            }
            Array.Resize(ref requests, requests.Length - 1);
            tmp.CopyTo(requests, 0);
        }
    }
}
