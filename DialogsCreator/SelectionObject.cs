using DialogsCreator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        private DialogDFD dialog;
        public TypeObject selected { get; private set; } = TypeObject.none;
        public DialogComponentView element { get; private set; } = null;
        InfoPanel infoPanel;

        public SelectionObject(DialogDFD dialog, ref ListBox panel) 
        { 
            this.dialog = dialog; 
            infoPanel = new InfoPanel(ref panel); 
        }

        public void Select(object obj) // <- передается тип объекта
        {

            if (obj is DialogComponentView)
            {
                selected = TypeObject.element;
                element = (obj as DialogComponentView);
                //ElementDFD el = dialog.Search(((element.Source) as SayingElementViewDFD).idElement);

                //infoPanel.Show(el);
            }
            else
            {
                selected = TypeObject.none;
                //infoPanel.Close();
            }

        }
    }
}
