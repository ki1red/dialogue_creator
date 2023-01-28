using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DialogsCreator
{
    public enum TypeObject
    {
        none = 0,
        element = 1,
        point = 2,
        line = 3
    }
    public class SelectionObject
    {
        public TypeObject selected { get; private set; } = TypeObject.none;
        public ElementDFD element { get; private set; } = null;

        public SelectionObject() { }

        public void Select(object obj) // <- передается тип объекта
        {
            MessageBox.Show("You select obj");
            if ( (obj as ElementDFD) == null)
            {
                selected = TypeObject.none;
                return;
            }
            element = (ElementDFD)obj;

        }
    }
}
