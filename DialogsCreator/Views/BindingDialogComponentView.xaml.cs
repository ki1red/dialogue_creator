using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DialogsCreator.Views
{

    public partial class BindingDialogComponentView : UserControl
    {

        public object parent { get; private set; }  //DialogView or OptionView
        public TypePointBindingView TypePointBinding { get; private set; }

        public BindingDialogComponentView(object parent, Canvas canvas, Point pointCreate, TypePointBindingView type)
        {
            InitializeComponent();
            this.parent = parent;

            canvas.Children.Add(this);

            Canvas.SetLeft(this, pointCreate.X);
            Canvas.SetTop(this, pointCreate.Y);

            TypePointBinding = type;
        }

        public void LinkWith(BindingDialogComponentView other, List<Line> lines)
        {
            if (parent == null)
                throw new NullReferenceException("Parent is null but you try link BindingDialogComponentView object without parent");

            if (other == null)
                throw new NullReferenceException("You try link this BindingDialogComponentView with other BindingDialogComponentView but other is null");

            if (other.parent == this.parent)
                throw new ArgumentException("You try link two BindingDialogComponentView with same parent");


            var nlines = new List<Line>();
            nlines.AddRange(lines);

            var package = new LinkDataDialogPackage(this.parent, other.parent, this, other, nlines);

            if (parent is DialogComponentView) 
            {
                var _parent = (DialogComponentView)parent;
                foreach (var linkDataPackage in _parent.linkDataPackages)
                {
                    if ((linkDataPackage.firstBindingDialogComponentView == this && linkDataPackage.secondeBindingDialogComponentView == other) ||
                        (linkDataPackage.secondeBindingDialogComponentView == this && linkDataPackage.firstBindingDialogComponentView == other)
                    ) throw new ArgumentException("This link already exsist");

                    if ((linkDataPackage.firstView == parent && linkDataPackage.secondeView == other.parent) ||
                        (linkDataPackage.secondeView == parent && linkDataPackage.firstView == other.parent)
                    ) throw new ArgumentException("This dialog components view is already bindings!");
                }

                _parent.Link(package);
              
                if(other.parent is DialogComponentView) 
                {
                    var _otherParent = (DialogComponentView)other.parent;
                    _otherParent.Link(package);
                }
                else if (other.parent is OptionDialogComponent) {
                    var _otherParent = (OptionDialogComponent)other.parent;
                    _otherParent.LinkWith(package);
                }
                
            }
            else if (parent is OptionDialogComponent) 
            {
                var _parent = (OptionDialogComponent)parent;
               
                foreach (var linkDataPackage in _parent.linkDataOptionPackages)
                {
                    if ((linkDataPackage.firstBindingDialogComponentView == this && linkDataPackage.secondeBindingDialogComponentView == other) ||
                        (linkDataPackage.secondeBindingDialogComponentView == this && linkDataPackage.firstBindingDialogComponentView == other)
                    ) throw new ArgumentException("This link already exsist");

                    if ((linkDataPackage.firstView == parent && linkDataPackage.secondeView == other.parent) ||
                        (linkDataPackage.secondeView == parent && linkDataPackage.firstView == other.parent)
                    ) throw new ArgumentException("This dialog components view is already bindings!");
                }
  
                _parent.LinkWith(package);

                if (other.parent is DialogComponentView)
                {
                    var _otherParent = (DialogComponentView)other.parent;
                    _otherParent.Link(package);
                }
                else if (other.parent is OptionDialogComponent)
                {
                    var _otherParent = (OptionDialogComponent)other.parent;
                    _otherParent.LinkWith(package);
                }

            }
        }
    }

    public enum TypePointBindingView
    {
        InputTypePoint,
        OutTypePoint
    }
}
