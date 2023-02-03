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
    public class DialogDFDs : IClonable
    {
        public string language;
        public ElementDFDs[] elements;

        public DialogDFDs(string language, ElementDFDs[] elements)
        {
            this.language = language;
            this.elements = elements;
        }
        public DialogDFDs()
        {
            this.language = null;
            this.elements = new ElementDFDs[0];
        }

        public void Clone(object obj)
        {
            if (!(obj is DialogDFD))
                throw new Exception($"{obj} is not DialogDFD");

            DialogDFD dialogDFD = obj as DialogDFD;

            this.language = dialogDFD.language;

            this.elements = new ElementDFDs[dialogDFD.elements.Length];

            for (int i = 0; i < this.elements.Length; i++)
            {
                this.elements[i] = new ElementDFDs();
                this.elements[i].Clone(dialogDFD.elements[i]);
            }
        }
    }


    [Serializable]
    public class ElementDFDs : IClonable
    {
        public int idElement;
        public string pathToSound;
        public string pathToImage;
        public string author;
        public SayingElementDFDs question;
        public SayingElementDFDs[] answers;
        public Point point;

        public ElementDFDs(int idElement, string pathToSound, string pathToImage, string author, SayingElementDFDs question, SayingElementDFDs[] answers, Point point)
        {
            this.idElement = idElement;
            this.pathToSound = pathToSound;
            this.pathToImage = pathToImage;
            this.author = author;
            this.question = question;
            this.answers = answers;
            this.point = point;
        }
        public ElementDFDs()
        {
            this.idElement = -1;
            this.pathToSound = null;
            this.pathToImage = null;
            this.author = null;
            this.question = null;
            this.answers = new SayingElementDFDs[0];
            this.point = new Point(-1, -1);
        }

        public void Clone(object obj)
        {
            if (!(obj is ElementDFD))
                throw new Exception($"{obj} is not ElementDFD");

            ElementDFD elementDFD = obj as ElementDFD;

            this.idElement = elementDFD.idElement;
            this.pathToImage = elementDFD.pathToImage;
            this.pathToSound = elementDFD.pathToSound;

            this.author = elementDFD.author;

            this.question = new SayingElementDFDs();
            this.question.Clone(elementDFD.question);

            this.answers = new SayingElementDFDs[elementDFD.answers.Length];
            for (int i = 0; i < elementDFD.answers.Length; i++)
            {
                this.answers[i] = new SayingElementDFDs();
                this.answers[i].Clone(elementDFD.answers[i]);
            }

            this.point = elementDFD.point;
        }
    }


    [Serializable]
    public class SayingElementDFDs : IClonable
    {
        public int idElement;
        public string text;
        public LinkSayingElementDFDs nextElement;
        public LinkSayingElementDFDs[] requests;

        public SayingElementDFDs(string text, LinkSayingElementDFDs nextElement, LinkSayingElementDFDs[] requests)
        {
            this.text = text;
            this.nextElement = nextElement;
            this.requests = requests;
        }
        public SayingElementDFDs()
        {
            this.text = null;
            this.nextElement = null;
            this.requests = new LinkSayingElementDFDs[0];
        }

        public void Clone(object obj)
        {
            if (!(obj is SayingElementDFD))
                throw new Exception($"{obj} is not SayingElementDFD");

            SayingElementDFD sayingElementDFD = obj as SayingElementDFD;

            this.idElement = sayingElementDFD.idElement;
            this.text = sayingElementDFD.text;

            this.nextElement = new LinkSayingElementDFDs();
            this.nextElement.Clone(sayingElementDFD.nextElement);

            this.requests = new LinkSayingElementDFDs[sayingElementDFD.requests.Length];
            for (int i = 0; i < sayingElementDFD.requests.Length; i++)
            {
                this.requests[i] = new LinkSayingElementDFDs();
                this.requests[i].Clone(sayingElementDFD.requests[i]);
            }
        }

    }


    [Serializable]
    public class LinkSayingElementDFDs : IClonable
    {
        public int idElement;
        public string textElement;

        public LinkSayingElementDFDs (int idElement, string textElement)
        {
            this.idElement = idElement;
            this.textElement = textElement;
        }
        public LinkSayingElementDFDs()
        {
            this.idElement = -1;
            this.textElement = null;
        }

        public void Clone(object obj)
        {
            if (!(obj is SayingElementDFD))
                throw new Exception($"{obj} is not SayingElementDFD");

            SayingElementDFD sayingElementDFD = obj as SayingElementDFD;

            this.idElement = sayingElementDFD.idElement;
            this.textElement = sayingElementDFD.text;
        }
    }


    public class DialogDFD : IClonable
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
            this.language = null;
            this.elements = new ElementDFD[0];
        }

        public void Add(ElementDFD element)
        {
            Array.Resize(ref elements, elements.Length + 1);
            elements[elements.Length - 1] = element;
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
            return ref elements[elements.Length - 1];
        }
        public void Clone(object obj)
        {
            if (!(obj is DialogDFDs))
                throw new Exception($"{obj} is not DialogDFDs");

            DialogDFDs dialogDFDs = obj as DialogDFDs;

            this.language = dialogDFDs.language;
            this.elements = new ElementDFD[dialogDFDs.elements.Length];

            for (int i = 0; i < this.elements.Length; i++)
            {
                this.elements[i] = new ElementDFD();
                this.elements[i].Clone(dialogDFDs.elements[i]);
            }
        }
    }


    public class ElementDFD : IClonable
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
            this.pathToSound = null;
            this.pathToImage = null;
            this.author = null;
            this.question = null;
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
                if (answers[i].text == text)
                    return ref answers[i];
            return ref question;
        }
        public ref SayingElementDFD Search(SayingElementDFD text)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i] == text)
                    return ref answers[i];
            }
            return ref question;
        }
        public void Clone(object obj)
        {
            if (!(obj is ElementDFDs))
                throw new Exception($"{obj} is not ElementDFDs");

            ElementDFDs elementDFDs = obj as ElementDFDs;

            this.idElement = elementDFDs.idElement;
            this.author = elementDFDs.author;

            this.pathToImage = elementDFDs.pathToImage;
            this.pathToSound = elementDFDs.pathToSound;

            this.question = new SayingElementDFD();
            this.question.Clone(elementDFDs.question);
            this.point = elementDFDs.point;

            this.answers = new SayingElementDFD[elementDFDs.answers.Length];
            for (int i = 0; i < this.answers.Length; i++)
            {
                this.answers[i] = new SayingElementDFD();
                this.answers[i].Clone(elementDFDs.answers[i]);
            }
        }
    }

    
    public class SayingElementDFD : IClonable
    {
        public int idElement;
        public string text;
        public SayingElementDFD nextElement;
        public SayingElementDFD[] requests;

        public SayingElementDFD(int idElement, string text, SayingElementDFD nextElement, SayingElementDFD[] requests)
        {
            this.idElement = idElement;
            this.text = text;
            this.nextElement = nextElement;
            this.requests = requests;
        }
        public SayingElementDFD()
        {
            this.idElement = -1;
            this.text = null;
            this.nextElement = null;
            this.requests = new SayingElementDFD[0];
        }
        private SayingElementDFD(LinkSayingElementDFDs linkSayingElementDFDs)
        {
            this.idElement = linkSayingElementDFDs.idElement;
            this.text = linkSayingElementDFDs.textElement;
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
        public void SetLinkeds(DialogDFD elements)
        {
            ref ElementDFD element = ref elements.Search(this.nextElement.idElement);
            this.nextElement = element.Search(this.nextElement.text);

            for (int i = 0; i < this.requests.Length; i++)
            {
                element = ref elements.Search(this.requests[i].idElement);
                this.requests[i] = element.Search(this.requests[i].text);
            }
        }
        public void Clone(object obj)
        {
            if (!(obj is SayingElementDFDs))
                throw new Exception($"{obj} is not SayingElementDFDs");

            SayingElementDFDs sayingElementDFDs = obj as SayingElementDFDs;

            this.idElement = sayingElementDFDs.idElement;
            this.text = sayingElementDFDs.text;

            this.nextElement = new SayingElementDFD(sayingElementDFDs.nextElement);

            this.requests = new SayingElementDFD[sayingElementDFDs.requests.Length];
            for (int i = 0; i < this.requests.Length; i++)
                this.requests[i] = new SayingElementDFD(sayingElementDFDs.requests[i]);
        }
    }


    public class SayingElementViewDFD : LinkedObject
    {
        public int idElement;
        public SayingElementDFD elementOld;
        public SayingElementDFD elementNew;

        public SayingElementViewDFD(int idElement, SayingElementDFD sayingElement)
        {
            this.idElement = idElement;
            this.elementOld = sayingElement;
            this.elementNew = sayingElement;
        }

        public void UpdateElement()
        {
            this.elementOld = this.elementNew;
        }
        public override void Bounds(LinkedObject linkObject)
        {
            if (linkObject == null)
                throw new Exception("Uninitialized object in view");

            
            elementNew.Add((linkObject as SayingElementViewDFD).elementNew);
        }
        public override void UnBounds(LinkedObject linkObject)
        {
            if (linkObject == null)
                throw new Exception("Uninitialized object in view");
            elementNew.Delete((linkObject as SayingElementViewDFD).elementNew);
        }
    }


    public interface IClonable
    {
        void Clone(object obj);
    }
}
