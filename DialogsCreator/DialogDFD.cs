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
        public Point positionCanvas;
        public ElementDFDs[] elements;
        public LinkDataDialogPackageSerialize[] linkeds;

        public DialogDFDs(string language, ElementDFDs[] elements, LinkDataDialogPackageSerialize[] linkeds)
        {
            this.language = language;
            this.elements = elements;
            this.linkeds = linkeds;
        }
        public DialogDFDs()
        {
            this.language = null;
            this.positionCanvas = new Point(0, 0);
            this.elements = new ElementDFDs[0];
            this.linkeds = new LinkDataDialogPackageSerialize[0];
        }

        public void Clone(object obj)
        {
            if (!(obj is DialogDFD))
                throw new Exception($"{obj} is not DialogDFD");

            DialogDFD dialogDFD = obj as DialogDFD;

            this.language = dialogDFD.language;
            this.positionCanvas = dialogDFD.positionCanvas;
            this.elements = new ElementDFDs[dialogDFD.elements.Length];

            this.linkeds = new LinkDataDialogPackageSerialize[dialogDFD.linkeds.Length];
            for (int i = 0; i < this.linkeds.Length; i++)
                this.linkeds[i] = dialogDFD.linkeds[i];

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
        public TypeSayingElementDFD type;
        public LinkSayingElementDFDs nextElement;
        public LinkSayingElementDFDs[] requests;

        public SayingElementDFDs(string text, TypeSayingElementDFD type,LinkSayingElementDFDs nextElement, LinkSayingElementDFDs[] requests)
        {
            this.text = text;
            this.type = type;
            this.nextElement = nextElement;
            this.requests = requests;
        }
        public SayingElementDFDs()
        {
            this.text = null;
            this.type = TypeSayingElementDFD.none;
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
            this.type = sayingElementDFD.type;

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
        public TypeSayingElementDFD type;
        public string textElement;

        public LinkSayingElementDFDs (int idElement, TypeSayingElementDFD type, string textElement)
        {
            this.idElement = idElement;
            this.type = type;
            this.textElement = textElement;
        }
        public LinkSayingElementDFDs()
        {
            this.idElement = -1;
            this.type = TypeSayingElementDFD.none;
            this.textElement = null;
        }

        public void Clone(object obj)
        {
            if (!(obj is SayingElementDFD))
                return;
                //throw new Exception($"{obj} is not SayingElementDFD");

            SayingElementDFD sayingElementDFD = obj as SayingElementDFD;

            this.idElement = sayingElementDFD.idElement;
            this.type = sayingElementDFD.type;
            this.textElement = sayingElementDFD.text;
        }
    }


    public class DialogDFD : IClonable
    {
        public string language;
        public Point positionCanvas;
        public ElementDFD[] elements;
        public LinkDataDialogPackageSerialize[] linkeds;
        public DialogDFD(string language, ElementDFD[] elements, LinkDataDialogPackageSerialize[] linkeds)
        {
            this.language = language;
            this.elements = elements;
            this.linkeds = linkeds;
        }
        public DialogDFD()
        {
            this.language = null;
            this.positionCanvas = new Point(0, 0);
            this.elements = new ElementDFD[0];
            this.linkeds = new LinkDataDialogPackageSerialize[0];
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
            throw new IndexOutOfRangeException("Element is not found.");
        }
        public void Clone(object obj)
        {
            if (!(obj is DialogDFDs))
                throw new Exception($"{obj} is not DialogDFDs");

            DialogDFDs dialogDFDs = obj as DialogDFDs;

            this.linkeds = new LinkDataDialogPackageSerialize[dialogDFDs.linkeds.Length];
            for (int i = 0; i < this.linkeds.Length; i++)
                this.linkeds[i] = dialogDFDs.linkeds[i];

            this.language = dialogDFDs.language;
            this.positionCanvas = dialogDFDs.positionCanvas;
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
        public SayingElementDFD Search(string text)
        {
            if (question.text == text)
                return question;
            for (int i = 0; i < answers.Length; i++)
                if (answers[i].text == text)
                    return answers[i];
            return null;
            
        }
        public SayingElementDFD Search(SayingElementDFD text)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i] == text)
                    return answers[i];
            }
            if (question == text)
                return question;
            else
                return null;
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
        public TypeSayingElementDFD type;
        public SayingElementDFD nextElement;
        public SayingElementDFD[] requests;

        public SayingElementDFD(int idElement, string text, TypeSayingElementDFD type, SayingElementDFD nextElement, SayingElementDFD[] requests)
        {
            this.idElement = idElement;
            this.text = text;
            this.type = type;
            this.nextElement = nextElement;
            this.requests = requests;
        }
        public SayingElementDFD()
        {
            this.idElement = -1;
            this.text = null;
            this.type = TypeSayingElementDFD.none;
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
            if (element == nextElement)
            {
                nextElement = new SayingElementDFD();
                return;
            }

            SayingElementDFD[] tmp = new SayingElementDFD[requests.Length];
            int iTmp = 0;
            foreach (var item in requests)
            {
                if (item == element)
                    continue;
                tmp[iTmp++] = item;
            }
            Array.Resize(ref tmp, iTmp);
            Array.Resize(ref requests, tmp.Length);
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
            ElementDFD element;
            if (this.nextElement.idElement != -1)
                element = elements.Search(this.nextElement.idElement);
            else
                return;

            if (element != null)
                this.nextElement = element.Search(this.nextElement.text);
            else
                this.nextElement = null;

            for (int i = 0; i < this.requests.Length; i++)
            {
                element = elements.Search(this.requests[i].idElement);

                if (element != null)
                    this.requests[i] = element.Search(this.requests[i].text);
                else
                    this.requests[i] = null;
            }
        }
        public void Clone(object obj)
        {
            if (!(obj is SayingElementDFDs))
                throw new Exception($"{obj} is not SayingElementDFDs");

            SayingElementDFDs sayingElementDFDs = obj as SayingElementDFDs;

            this.idElement = sayingElementDFDs.idElement;
            this.text = sayingElementDFDs.text;
            this.type = sayingElementDFDs.type;

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
        public override void Bounds(LinkedObject output, LinkedObject input)
        {
            if (output == null || input == null)
                throw new Exception("Uninitialized object in view");

            if (output == input)
                throw new Exception("This is same object");

            if (output == this)
            {
                if ((input as SayingElementViewDFD).elementNew.type == TypeSayingElementDFD.question && (this.elementNew.type != TypeSayingElementDFD.none))
                    this.elementNew.nextElement = (input as SayingElementViewDFD).elementNew;
                else if ((input as SayingElementViewDFD).elementNew.type == TypeSayingElementDFD.none)
                    throw new Exception("This is NOT object");
            }
            else if (input == this)
            {
                if (this.elementNew.type == TypeSayingElementDFD.answer && (output as SayingElementViewDFD).elementNew.type == TypeSayingElementDFD.question)
                    throw new Exception("Other object is question!");
                else if((output as SayingElementViewDFD).elementNew.type == TypeSayingElementDFD.none)
                    throw new Exception("This is NOT object");
                else
                    elementNew.Add((output as SayingElementViewDFD).elementNew);
            }
            else
                throw new Exception("This is FUCKING DATA");
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

    public enum TypeSayingElementDFD
    {
        none = 0,
        question = 1,
        answer = 2
    }
}
