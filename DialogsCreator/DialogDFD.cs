using DialogsCreator.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Point = System.Windows.Point;

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
        public ref ElementDFD Search(int id)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].idElement == id)
                    return ref elements[i];
            }
            return ref elements[elements.Length-1];
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

        public Point point;

        public ElementDFD(int idElement, string pathToSound, string pathToImage, string author, SayingElementDFD question, SayingElementDFD[] answers, Point point)
        {
            this.idElement = idElement;
            this.pathToSound = pathToSound;
            this.pathToImage = pathToImage;
            this.author = author;
            this.question = question;
            this.answers = answers;
            this.point = point;
        }
        public ElementDFD()
        {
            this.idElement = -1;
            this.pathToSound = "NULL";
            this.pathToImage = "NULL";
            this.author = "NULL";
            this.question = new SayingElementDFD();
            this.answers = new SayingElementDFD[0];
            this.point = new Point(-1, -1);
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
        public ref SayingElementDFD Search(string text)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i].text == text)
                    return ref answers[i];
            }
            return ref question;
        }
    }

    [Serializable]
    public class SayingElementDFD
    {
        public string text;
        public SayingElementDFD nextElement;
        public SayingElementDFD[] requests;
        public SayingElementDFD(string text, SayingElementDFD nextElement, SayingElementDFD[] requests)
        {
            this.text = text;
            this.nextElement = nextElement;
            this.requests = requests;
        }
        public SayingElementDFD()
        {
            this.text = "NULL";
            this.nextElement = null;
            this.requests = new SayingElementDFD[0];
        }
        public void Add(SayingElementDFD element)
        {
            Array.Resize(ref requests, requests.Length + 1);
            requests[requests.Length - 1] = element;
        }
        public void Replace(ref string elementOld, string elementNew)
        {
            elementOld = elementNew;
        }
        public void Delete(SayingElementDFD element)
        {
            SayingElementDFD[] tmp = new SayingElementDFD[requests.Length - 1];
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
        public ref SayingElementDFD Search(SayingElementDFD request)
        {
            for (int i = 0; i < requests.Length; i++)
                if (requests[i] == request)
                    return ref requests[i];
            return ref requests[requests.Length - 1];
        }

        
    }

    public class SayingElementViewDFD : LinkedObject
    {
        public int idElement;
        public SayingElementDFD sayingElement;
        public SayingElementViewDFD(int idElement, SayingElementDFD sayingElement)
        {
            this.idElement = idElement;
            this.sayingElement = sayingElement;
        }

        public override void Bounds(LinkedObject linkObject)
        {
            if (linkObject == null)
                throw new Exception("Неинициализированный объект во View");
            sayingElement.Add((linkObject as SayingElementViewDFD).sayingElement);
        }

        public override void UnBounds(LinkedObject linkObject)
        {
            if (linkObject == null)
                throw new Exception("Неинициализированный объект во View");
            sayingElement.Delete((linkObject as SayingElementViewDFD).sayingElement);
        }
    }
}
