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
    /// Логика взаимодействия для OptionDialogComponent.xaml
    /// </summary>
    public partial class OptionDialogComponent : UserControl
    {
        public BindingDialogComponentView LeftBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView RightBindingDialogComponentView { get; private set; }

        private const int marginBindingDialogCopmonentView = 10;
        private const int bindingDialogComponentWidth = 10;
        private const int bindingDialogComponentHeight = 10;

        private DialogComponentView parent;
        private Canvas canvas;

        public OptionDialogComponent(Canvas drawingCanvas,DialogComponentView componentView)
        {
            InitializeComponent();
            parent = componentView;
            canvas = drawingCanvas;
        }

        public void ShowBindigsDialogComponentsView()
        {
            if (CheckBindingsInit() == true)
            {
                RightBindingDialogComponentView.Visibility = Visibility.Visible;
                LeftBindingDialogComponentView.Visibility = Visibility.Visible;
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
                RightBindingDialogComponentView.Visibility = Visibility.Hidden;
                LeftBindingDialogComponentView.Visibility = Visibility.Hidden;
            }
            else
            {
                InitEmptyBindings();
                HideBindigsDialogComponentsView();
            }
        }

        private void InitEmptyBindings()
        {

            if (LeftBindingDialogComponentView == null)
            {
                LeftBindingDialogComponentView = new BindingDialogComponentView(parent, canvas, GetPointLeftBindingComponent());
                LeftBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Green);

            }

            if (RightBindingDialogComponentView == null)
            {
                RightBindingDialogComponentView = new BindingDialogComponentView(parent, canvas, GetPointRightBindingComponent());
                RightBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Blue);
            }
        }

        private int GetIndex() 
        { 
            for(int i = 0;i < parent.Options.Count(); i++) 
            { 
                if(parent.Options[i] == this)
                    return i;
            }

            throw new ArgumentException("Option don't find in parent");
        }

        private Point GetPointLeftBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(parent) - ((bindingDialogComponentWidth / 2f) + marginBindingDialogCopmonentView),
                y: Canvas.GetTop(parent) + (this.OptionCanvas.Height / 2f) + (parent.DialogComponentCanvas.Height / 2f) + ((GetIndex() + 1) * this.OptionCanvas.Height)
            );
        }

        private Point GetPointRightBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(parent) + this.OptionCanvas.Width  + (bindingDialogComponentWidth / 2f) + marginBindingDialogCopmonentView,
                y: Canvas.GetTop(parent) + (this.OptionCanvas.Height / 2f) + (parent.DialogComponentCanvas.Height / 2f) + ((GetIndex() + 1) * this.OptionCanvas.Height)
            );
        }

        private bool CheckBindingsInit()
        {
            return
                 LeftBindingDialogComponentView != null &&
                 RightBindingDialogComponentView != null;
        }
    }
}
