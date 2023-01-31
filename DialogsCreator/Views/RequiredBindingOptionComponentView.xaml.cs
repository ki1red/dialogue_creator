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
    /// <summary>
    /// Логика взаимодействия для RequiredBindingOptionComponentView.xaml
    /// </summary>
    public partial class RequiredBindingOptionComponentView : UserControl
    {
        public OptionDialogComponent parent { get; private set; }

        public RequiredBindingOptionComponentView(OptionDialogComponent parent, Canvas canvas, Point pointCreate)
        {
            InitializeComponent();
            this.parent = parent;

            canvas.Children.Add(this);

            Canvas.SetLeft(this, pointCreate.X);
            Canvas.SetTop(this, pointCreate.Y);
        }

        public void LinkWith(RequiredBindingOptionComponentView other, List<Line> lines)
        {
            if (parent == null)
                throw new NullReferenceException("Parent is null but you try link BindingDialogComponentView object without parent");

            if (other == null)
                throw new NullReferenceException("You try link this BindingDialogComponentView with other BindingDialogComponentView but other is null");

            if (other.parent == this.parent)
                throw new ArgumentException("You try link two BindingDialogComponentView with same parent");

            foreach (var linkDataPackage in parent.linkDataOptionPackages)
            {
                if ((linkDataPackage.firstReqiredBindingDialogComponentView == this && linkDataPackage.secondeReqiredBindingDialogComponentView == other) ||
                    (linkDataPackage.secondeReqiredBindingDialogComponentView == this && linkDataPackage.firstReqiredBindingDialogComponentView == other)
                ) throw new ArgumentException("This link already exsist");

                if ((linkDataPackage.firstOptionComponent == parent && linkDataPackage.secondeOptionComponent == other.parent) ||
                    (linkDataPackage.secondeOptionComponent == parent && linkDataPackage.firstOptionComponent == other.parent)
                ) throw new ArgumentException("This dialog components view is already bindings!");
            }

            var nlines = new List<Line>();
            nlines.AddRange(lines);
            var package = new LinkDataOptionPackage(this.parent, other.parent, this, other, nlines);
            this.parent.LinkWith(package);
            other.parent.LinkWith(package);
        }


    }
}

