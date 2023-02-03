using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public class LinkDataDialogPackage
    {
        public DialogComponentView firstDialogComponent { get; private set; }
        public DialogComponentView secondeDialogComponent { get; private set; }
        public BindingDialogComponentView firstBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView secondeBindingDialogComponentView { get; private set; }
        public ElementDFD element { get; private set; }
        public List<Line> Lines { get; private set; } 
        public LinkDataDialogPackage(
            DialogComponentView firstDialogComponent,
            DialogComponentView secondeDialogComponent,
            BindingDialogComponentView firstBindingDialogComponentView,
            BindingDialogComponentView secondeBindingDialogComponentView,
            List<Line>lines)
        {
            this.firstDialogComponent = firstDialogComponent;
            this.secondeDialogComponent = secondeDialogComponent;
            this.firstBindingDialogComponentView = firstBindingDialogComponentView;
            this.secondeBindingDialogComponentView = secondeBindingDialogComponentView;
            Lines = lines;
        }
    }

    public partial class DialogComponentView : UserControl
    {
        private const int marginBindingDialogCopmonentView = 10;
        private const int bindingDialogComponentWidth = 10;
        private const int bindingDialogComponentHeight = 10;

        public Canvas canvas { get; private set; }
        private Point _mousePosition;
        private bool _isMouseDown = false;

        private List<BindingDialogComponentView> bindingDialogComponentViews = new List<BindingDialogComponentView>();
        public List<LinkDataDialogPackage> linkDataPackages { get; private set; } = new List<LinkDataDialogPackage>();

        public BindingDialogComponentView LeftBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView RightBindingDialogComponentView { get; private set; }
        public TextBlock _TextBlockComponentName { get; set; }
        public ObservableCollection<OptionDialogComponent> Options { get; private set; } = new ObservableCollection<OptionDialogComponent>();
        // TODO сделать понмиание номера элемента при связях
        public LinkedObject Source { get; set; }
        public bool isMove { get; set; }
        public DialogComponentView(Canvas drawingCanvas)
        {
            InitializeComponent();
            canvas = drawingCanvas;
            this._TextBlockComponentName = TextBlockComponentName;

            isMove = false;
        }
        public void AddOption()
        {
            var option = new OptionDialogComponent(canvas, this);
            option.HorizontalAlignment = HorizontalAlignment.Center;
            option.Margin = new Thickness(0, 10, 0, 0);
            OptionStackPanel.Children.Add(option);
            Options.Add(option);
        }
        
        public void AddOption(LinkedObject source)
        {
            var option = new OptionDialogComponent(canvas, this);
            option.HorizontalAlignment = HorizontalAlignment.Center;
            option.Margin = new Thickness(0, 10, 0, 0);
            option.OptionSource = source;
            OptionStackPanel.Children.Add(option);
            Options.Add(option);
        }

        public void RemoveOption(OptionDialogComponent option)
        {
            foreach (var package in option.linkDataOptionPackages)
            {
                package.firstOptionComponent.UnLinkWith(package);
                package.secondeOptionComponent.UnLinkWith(package);
            }
            OptionStackPanel.Children.Remove(option);
            Options.Remove(option);
        }

        public void ShowBindigsDialogComponentsView()
        {
            if (CheckBindingsInit() == true)
            {
                foreach (var view in bindingDialogComponentViews)
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
                 LeftBindingDialogComponentView != null &&
                 RightBindingDialogComponentView != null;
        }
        private void InitEmptyBindings()
        {
    


            if (LeftBindingDialogComponentView == null)
            {
                LeftBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointLeftBindingComponent());
                LeftBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Green);
                if (bindingDialogComponentViews.Contains(LeftBindingDialogComponentView) == false)
                    bindingDialogComponentViews.Add(LeftBindingDialogComponentView);
            }

            if (RightBindingDialogComponentView == null)
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

                // TODO изменено положение какого-то объекта
                isMove = true;

                foreach (var binding in bindingDialogComponentViews)
                {
                    Canvas.SetLeft(binding, offset.X + Canvas.GetLeft(binding));
                    Canvas.SetTop(binding, offset.Y + Canvas.GetTop(binding));
                }

                foreach (var linkData in linkDataPackages)
                {
                    if (linkData.firstDialogComponent == this)
                    {
                        linkData.Lines.First().X1 =
                            offset.X + linkData.Lines.First().X1;
                        linkData.Lines.First().Y1 =
                            linkData.Lines.First().Y1 + offset.Y;
                    }

                    else if (linkData.secondeDialogComponent == this)
                    {
                        linkData.Lines.Last().X2 =
                            offset.X + linkData.Lines.Last().X2;

                        linkData.Lines.Last().Y2 =
                            linkData.Lines.Last().Y2 + offset.Y;
                    }

                    else
                    {
                        throw new ArgumentException("This Dialog Component not linked but have linkData with another objects this is error in logic");
                    }

                }
                foreach (var option in Options)
                {
                    Canvas.SetLeft(option.RightBindingDialogComponentView, offset.X + Canvas.GetLeft(option.RightBindingDialogComponentView));
                    Canvas.SetTop(option.RightBindingDialogComponentView, offset.Y + Canvas.GetTop(option.RightBindingDialogComponentView));

                    Canvas.SetLeft(option.LeftBindingDialogComponentView, offset.X + Canvas.GetLeft(option.LeftBindingDialogComponentView));
                    Canvas.SetTop(option.LeftBindingDialogComponentView, offset.Y + Canvas.GetTop(option.LeftBindingDialogComponentView));

                    foreach (var linkDataOption in option.linkDataOptionPackages)
                    {
                        if (linkDataOption.firstOptionComponent == option)
                        {
                            linkDataOption.Lines.First().X1 =
                                offset.X + linkDataOption.Lines.First().X1;
                            linkDataOption.Lines.First().Y1 =
                                linkDataOption.Lines.First().Y1 + offset.Y;
                        }

                        else if (linkDataOption.secondeOptionComponent == option)
                        {
                            linkDataOption.Lines.Last().X2 =
                                offset.X + linkDataOption.Lines.Last().X2;
                            linkDataOption.Lines.Last().Y2 =
                                linkDataOption.Lines.Last().Y2 + offset.Y;
                        }
                    }
                }
            }
        }


        private Point GetPointLeftBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(this) - ((bindingDialogComponentWidth) + marginBindingDialogCopmonentView),
                y: Canvas.GetTop(this) + ActualHeight / 2f
            );
        }
        private Point GetPointRightBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(this) + ActualWidth + (bindingDialogComponentWidth / 2f) + marginBindingDialogCopmonentView,
                y: Canvas.GetTop(this) + ActualHeight / 2f
            );
        }
        public void Link(LinkDataDialogPackage linkDataPackage)
        {

            if (linkDataPackage.firstDialogComponent == this)
            {
                Source?.Bounds(linkDataPackage.secondeDialogComponent.Source);
                linkDataPackages.Add(linkDataPackage);


            }

            else if (linkDataPackage.secondeDialogComponent == this)
            {
                Source?.Bounds(linkDataPackage.firstDialogComponent.Source);
                linkDataPackages.Add(linkDataPackage);
            }
        }
        public void UnLinkWith(LinkDataDialogPackage linkedPackage)
        {
            foreach (var line in linkedPackage.Lines)
            {
                canvas.Children.Remove(line);
            }

            if (linkedPackage.firstDialogComponent == this)
            {
                Source?.UnBounds(linkedPackage.secondeDialogComponent.Source);
                linkDataPackages.Remove(linkedPackage);
            }

            else if (linkedPackage.secondeDialogComponent == this)
            {
                Source?.UnBounds(linkedPackage.firstDialogComponent.Source);
                linkDataPackages.Remove(linkedPackage);
            }
        }
        public void SetName()
        {
            string fullName = (Source as SayingElementViewDFD).elementOld.text;

            if (fullName.Length <= 7)
            {
                TextBlockComponentName.Text = fullName;
                return;
            }

            int i = 0;
            string shortName = "";
            foreach (var ch in fullName)
            {
                if (i == 7)
                    break;
                shortName += ch;
                i++;
            }
            TextBlockComponentName.Text = shortName;
        }
        public void UpdateNameDialog()
        {
            this.SetName();

            for (int i = 0; i < this.Options.Count; i++)
                this.Options[i].SetName();
        }
        public void Destroy()
        {
            var options = Options.ToList();

            foreach (var option in options)
            {
                option.Destroy();
                Options.Remove(option);
            }

            var packages = linkDataPackages.ToList();

            foreach (var package in packages)
            {
                package.firstDialogComponent.UnLinkWith(package);
                package.secondeDialogComponent.UnLinkWith(package);
            }

            canvas.Children.Remove(this.LeftBindingDialogComponentView);
            canvas.Children.Remove(this.RightBindingDialogComponentView);
            canvas.Children.Remove(this);
        }
    }
}