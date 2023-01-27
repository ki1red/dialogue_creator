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
        
        public BindingDialogComponentView(DialogComponentView parent, Canvas canvas,Point pointCreate)
        {
            InitializeComponent();
            this.parent = parent;
            
            canvas.Children.Add(this);

            Canvas.SetLeft(this, pointCreate.X);
            Canvas.SetTop(this,pointCreate.Y);
        }

        public void LinkWith(BindingDialogComponentView other,Canvas canvas) 
        {   
            if (parent == null)
                throw new NullReferenceException("Parent is null but you try link BindingDialogComponentView object without parent");

            if(other == null)
                throw new NullReferenceException("You try link this BindingDialogComponentView with other BindingDialogComponentView but other is null");

            if (canvas == null)
                throw new NullReferenceException("Canvas is null, imposable draw line without canvas");

            if (other.parent == this.parent)
                throw new ArgumentException("You try link two BindingDialogComponentView with same parent");


            Path path = new Path();
            PathFigure myPathFigure = new PathFigure();
            PathGeometry myPathGeometry = new PathGeometry();

            Point pointCurrentBindingComponentView = GetPostionBindingPoint(this);

            Point pointLinkedBindingComponentView = GetPostionBindingPoint(other);

            Point pointControll = new Point (
                (pointCurrentBindingComponentView.X + pointLinkedBindingComponentView.X) / 2f, 
                (pointCurrentBindingComponentView.Y + pointLinkedBindingComponentView.Y) / 2f);

            myPathFigure.StartPoint = pointCurrentBindingComponentView;

            var line = new BezierSegment(
                    pointCurrentBindingComponentView,
                    pointControll,
                    pointLinkedBindingComponentView,
                    true);

            canvas.Children.Add(path);
            myPathFigure.Segments.Add(line);
            myPathGeometry.Figures.Add(myPathFigure);

            path.Stroke = Brushes.Black;
            path.StrokeThickness = 5;
            path.StrokeStartLineCap = PenLineCap.Round;
            path.StrokeEndLineCap = PenLineCap.Round;
            path.Data = myPathGeometry;

            this.parent.Link(new LinkDataPackage(this.parent, other.parent, this, other, new KeyValuePair<PathFigure, BezierSegment>(myPathFigure, line)));
            other.parent.Link(new LinkDataPackage(other.parent, this.parent, other, this, new KeyValuePair<PathFigure, BezierSegment>(myPathFigure, line)));
        }



        public static Point GetPostionBindingPoint(BindingDialogComponentView view)
        {
            return new Point(
            x: (
                Canvas.GetLeft(view) +
                (view.Width / 2f)),
            y: (+
                Canvas.GetTop(view) +
                (view.Height / 2f)));
        }
    }
}
