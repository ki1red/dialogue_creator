﻿using DialogsCreator.Views;
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
        //public ElementDFD element { get; private set; } = null;
        public SayingElementViewDFD element { get; private set; } = null;

        public SelectionObject() { }

        public void Select(object obj) // <- передается тип объекта
        {
            
            if (obj is DialogComponentView)
            {
                selected = TypeObject.element;
                element = (obj as DialogComponentView).Source as SayingElementViewDFD;
            }
            //element = (ElementDFD)obj;

        }
    }
}
