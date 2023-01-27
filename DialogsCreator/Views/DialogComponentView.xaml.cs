using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DialogsCreator.Views
{
    /// <summary>
    /// Логика взаимодействия для DialogComponentView.xaml
    /// </summary>
    /// 
  
    public class LinkDataPackage
    {
        public DialogComponentView currentDialogComponent { get; private set;}
        public DialogComponentView linkedDialogComponent { get; private set; }
        public BindingDialogComponentView currentBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView linkedBindingDialogComponentView { get; private set; }
        public KeyValuePair<PathFigure, BezierSegment> linkLineData { get; private set; }
        public LinkDataPackage(
            DialogComponentView currentDialogComponent, 
            DialogComponentView linkedDialogComponent, 
            BindingDialogComponentView currentBindingDialogComponentView, 
            BindingDialogComponentView linkedBindingDialogComponentView, 
            KeyValuePair<PathFigure, BezierSegment> lineData)
        {
            this.currentDialogComponent = currentDialogComponent;
            this.linkedDialogComponent = linkedDialogComponent;
            this.currentBindingDialogComponentView = currentBindingDialogComponentView;
            this.linkedBindingDialogComponentView = linkedBindingDialogComponentView;
            this.linkLineData = lineData;
        }
    }

    public partial class DialogComponentView : UserControl
    {
        private const int marginBindingDialogCopmonentView = 10;
        private const int bindingDialogComponentWidth = 10;
        private const int bindingDialogComponentHeight = 10;

        private Canvas canvas;
        private Point _mousePosition;
        private bool _isMouseDown = false;
        private List<LinkDataPackage> linkDataPackages = new List<LinkDataPackage>();
        private List<BindingDialogComponentView> bindingDialogComponentViews = new List<BindingDialogComponentView>();

        public BindingDialogComponentView TopBindingDialogComponentView { get; private set; }

        public BindingDialogComponentView BottomBindingDialogComponentView { get; private set; }

        public BindingDialogComponentView LeftBindingDialogComponentView { get; private set; }

        public BindingDialogComponentView RightBindingDialogComponentView { get; private set; }

        public DialogComponentView(Canvas drawingCanvas)
        {
            InitializeComponent();
            canvas = drawingCanvas;
        }
        public void ShowBindigsDialogComponentsView() 
        {
            if(CheckBindingsInit() == true) 
            {
                foreach(var view in bindingDialogComponentViews) 
                {
                    view.Visibility = Visibility.Visible;
                }
            }
            else
            {
                InitEmptyBindings();
                ShowBindigsDialogComponentsView();
            }
        }

        public void HideBindigsDialogComponentsView() 
        {
            if (CheckBindingsInit() == true)
            {
                foreach (var view in bindingDialogComponentViews)
                {
                    view.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                InitEmptyBindings();
                HideBindigsDialogComponentsView();
            }
        }

        private bool CheckBindingsInit() 
        {
           return 
                TopBindingDialogComponentView != null && 
                BottomBindingDialogComponentView != null && 
                LeftBindingDialogComponentView != null && 
                RightBindingDialogComponentView != null;
        }

        private void InitEmptyBindings() 
        {
            if(TopBindingDialogComponentView == null)
            {
                TopBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointTopBindingComponent());
                TopBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Yellow);
                if (bindingDialogComponentViews.Contains(TopBindingDialogComponentView) == false)
                    bindingDialogComponentViews.Add(TopBindingDialogComponentView);
            }
                
            if(BottomBindingDialogComponentView == null) 
            {
                BottomBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointBottomBindingComponent());
                BottomBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Red);
                if (bindingDialogComponentViews.Contains(BottomBindingDialogComponentView) == false)
                    bindingDialogComponentViews.Add(BottomBindingDialogComponentView);
            }
           
            if(LeftBindingDialogComponentView == null)
            {
                LeftBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointLeftBindingComponent());
                LeftBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Green);
                if (bindingDialogComponentViews.Contains(LeftBindingDialogComponentView) == false)
                    bindingDialogComponentViews.Add(LeftBindingDialogComponentView);
            }
            
            if(RightBindingDialogComponentView == null) 
            {
                RightBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointRightBindingComponent());
                RightBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Blue);
                if (bindingDialogComponentViews.Contains(RightBindingDialogComponentView) == false)
                    bindingDialogComponentViews.Add(RightBindingDialogComponentView);
            }
        }

        private void DialogComponentView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            _mousePosition = e.GetPosition(this);
        }

        private void DialogComponentView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void DialogComponentView_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                var currentPosition = e.GetPosition(this);
                var offset = currentPosition - _mousePosition;
                
                Canvas.SetLeft(this, Canvas.GetLeft(this) + offset.X);
                Canvas.SetTop(this, Canvas.GetTop(this) + offset.Y);
              
                foreach(var binding in bindingDialogComponentViews) 
                {
                    Canvas.SetLeft(binding, offset.X + Canvas.GetLeft(binding));
                    Canvas.SetTop(binding,  offset.Y + Canvas.GetTop(binding));
                }

                foreach (var linkData in linkDataPackages) 
                {
                    var p1 = BindingDialogComponentView.GetPostionBindingPoint(
                        linkData.currentBindingDialogComponentView);
                   
                    var p3 = BindingDialogComponentView.GetPostionBindingPoint(
                        linkData.linkedBindingDialogComponentView);

                    var p2 = new Point(
                        x: (p1.X + p3.X) / 2,
                        y: (p1.Y + p3.Y) / 2);

                    linkData.linkLineData.Key.StartPoint = p1;
                    linkData.linkLineData.Value.Point1 = p1;
                    linkData.linkLineData.Value.Point2 = p2;
                    linkData.linkLineData.Value.Point3 = p3;
                }
            }
        }

        private Point GetPointTopBindingComponent() 
        {
            return new Point(
                x:Canvas.GetLeft(this) + (this.DialogComponentCanvas.Width / 2f),
                y:Canvas.GetTop(this) - ((bindingDialogComponentHeight) + marginBindingDialogCopmonentView)
            );
        }

        private Point GetPointBottomBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(this) + (this.DialogComponentCanvas.Width / 2f),
                y: Canvas.GetTop(this) + this.DialogComponentCanvas.Height + (bindingDialogComponentHeight / 2f) + marginBindingDialogCopmonentView
            );
        }

        private Point GetPointLeftBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(this) - ((bindingDialogComponentWidth) + marginBindingDialogCopmonentView),
                y: Canvas.GetTop(this) + this.DialogComponentCanvas.Height/2f
            );
        }

        private Point GetPointRightBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(this) + this.DialogComponentCanvas.Width + (bindingDialogComponentWidth / 2f) + marginBindingDialogCopmonentView,
                y: Canvas.GetTop(this) + this.DialogComponentCanvas.Height / 2f
            );
        }

        public void Link(LinkDataPackage linkDataPackage) 
        {
            linkDataPackages.Add(linkDataPackage);
        }
    }
}
