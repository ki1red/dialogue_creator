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

        public DialogComponentView parent { get; private set; }
        public List<Line> bindingsLines = new List<Line>();
<<<<<<< Updated upstream
        public BindingDialogComponentView(DialogComponentView parent, Canvas canvas,Point pointCreate)
=======
        
        public BindingDialogComponentView(DialogComponentView parent, Canvas canvas, Point pointCreate)
>>>>>>> Stashed changes
        {
            InitializeComponent();
            this.parent = parent;
            
            canvas.Children.Add(this);

            Canvas.SetLeft(this, pointCreate.X);
            Canvas.SetTop(this,pointCreate.Y);
        }

        public void LinkWith(BindingDialogComponentView other, List<Line> lines) 
        {
            if (parent == null)
                throw new NullReferenceException("Parent is null but you try link BindingDialogComponentView object without parent");

            if(other == null)
                throw new NullReferenceException("You try link this BindingDialogComponentView with other BindingDialogComponentView but other is null");

            if (other.parent == this.parent)
                throw new ArgumentException("You try link two BindingDialogComponentView with same parent");

            foreach(var linkDataPackage in parent.linkDataPackages) 
            {
                if ((linkDataPackage.firstBindingDialogComponentView == this && linkDataPackage.secondeBindingDialogComponentView == other) ||
                    (linkDataPackage.secondeBindingDialogComponentView == this && linkDataPackage.firstBindingDialogComponentView == other)
                ) throw new ArgumentException("This link already exsist");

                if((linkDataPackage.firstDialogComponent == parent && linkDataPackage.secondeDialogComponent == other.parent) ||
                    (linkDataPackage.secondeDialogComponent == parent && linkDataPackage.firstDialogComponent == other.parent)
                ) throw new ArgumentException("This dialog components view is already bindings!");
            }

            this.bindingsLines.AddRange(lines);
            other.bindingsLines.AddRange(lines);

            this.parent.Link(new LinkDataPackage(this.parent, other.parent, this, other));
            other.parent.Link(new LinkDataPackage(this.parent, other.parent, this, other));
        }
    }
}
