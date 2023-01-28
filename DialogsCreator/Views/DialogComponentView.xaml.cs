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
    public class LinkDataPackage
    {
        public DialogComponentView firstDialogComponent { get; private set;}
        public DialogComponentView secondeDialogComponent { get; private set; }
        public BindingDialogComponentView firstBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView secondeBindingDialogComponentView { get; private set; }

        public LinkDataPackage(
            DialogComponentView firstDialogComponent, 
            DialogComponentView secondeDialogComponent, 
            BindingDialogComponentView firstBindingDialogComponentView, 
            BindingDialogComponentView secondeBindingDialogComponentView)
        {
            this.firstDialogComponent = firstDialogComponent;
            this.secondeDialogComponent = secondeDialogComponent;
            this.firstBindingDialogComponentView = firstBindingDialogComponentView;
            this.secondeBindingDialogComponentView = secondeBindingDialogComponentView;
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
        private List<BindingDialogComponentView> bindingDialogComponentViews = new List<BindingDialogComponentView>();

        public List<LinkDataPackage> linkDataPackages { get; private set; } = new List<LinkDataPackage>();

        public BindingDialogComponentView TopBindingDialogComponentView { get; private set; }

        public BindingDialogComponentView BottomBindingDialogComponentView { get; private set; }

        public BindingDialogComponentView LeftBindingDialogComponentView { get; private set; }

        public BindingDialogComponentView RightBindingDialogComponentView { get; private set; }

        public List<OptionDialogComponent> Options { get; private set; } = new List<OptionDialogComponent>();

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
        
        public void AddOption()
        {
            var option = new OptionDialogComponent(canvas,this);
            OptionStackPanel.Children.Add(option);
            Options.Add(option);
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
                    if(linkData.firstDialogComponent == this) 
                    {
                        linkData.firstBindingDialogComponentView.bindingsLines.First().X1 = 
                            offset.X + linkData.firstBindingDialogComponentView.bindingsLines.First().X1;
                        linkData.firstBindingDialogComponentView.bindingsLines.First().Y1 =
                            linkData.firstBindingDialogComponentView.bindingsLines.First().Y1 + offset.Y;
                    }

                    else if (linkData.secondeDialogComponent == this) 
                    {
                        linkData.firstBindingDialogComponentView.bindingsLines.Last().X2 =
                            offset.X + linkData.firstBindingDialogComponentView.bindingsLines.Last().X2;

                        linkData.firstBindingDialogComponentView.bindingsLines.Last().Y2 =
                            linkData.firstBindingDialogComponentView.bindingsLines.Last().Y2 + offset.Y;
                    }

                    else
                    {
                        throw new ArgumentException("This Dialog Component not linked but have linkData with another objects this is error in logic");
                    }

                }

                foreach(var option in Options)
                {
                    Canvas.SetLeft(option.RightBindingDialogComponentView, offset.X + Canvas.GetLeft(option.RightBindingDialogComponentView));
                    Canvas.SetTop(option.RightBindingDialogComponentView, offset.Y + Canvas.GetTop(option.RightBindingDialogComponentView));

                    Canvas.SetLeft(option.LeftBindingDialogComponentView, offset.X + Canvas.GetLeft(option.LeftBindingDialogComponentView));
                    Canvas.SetTop(option.LeftBindingDialogComponentView, offset.Y + Canvas.GetTop(option.LeftBindingDialogComponentView));
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
