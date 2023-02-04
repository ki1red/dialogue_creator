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
        public object firstView { get; private set; }
        public object secondeView { get; private set; }
        public BindingDialogComponentView firstBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView secondeBindingDialogComponentView { get; private set; }
        public List<Line> Lines { get; private set; } 

        public LinkDataDialogPackage(
            object firstDialogComponent,
            object secondeDialogComponent,
            BindingDialogComponentView firstBindingDialogComponentView,
            BindingDialogComponentView secondeBindingDialogComponentView,
            List<Line>lines)
        {
            this.firstView = firstDialogComponent;
            this.secondeView = secondeDialogComponent;
            this.firstBindingDialogComponentView = firstBindingDialogComponentView;
            this.secondeBindingDialogComponentView = secondeBindingDialogComponentView;
            Lines = lines;
        }

        public LinkDataDialogPackageSerialize toLinkDataDialogPackageSerialize() 
        {
            int outViewId;
            int inViewId;
            
            int outBindingId = firstBindingDialogComponentView.Id;
            int inBindingId = secondeBindingDialogComponentView.Id;
            
            int outOptionId;
            int inOptionId;

            if (firstView is DialogComponentView)
            {
                outViewId = (firstView as DialogComponentView ?? throw new NullReferenceException("FristView is null")).Id;
            }
            else if (firstView is OptionDialogComponent)
            {
               outViewId = (firstView as OptionDialogComponent ?? throw new NullReferenceException("FristView is null")).parent.Id;
               outOptionId = (firstView as OptionDialogComponent ?? throw new NullReferenceException("FristView is null")).Id;
            }
            else throw new ArgumentException("Unknow view in LinkDatapackage");

            if (secondeView is DialogComponentView)
            {
                inViewId = (secondeView as DialogComponentView ?? throw new NullReferenceException("FristView is null")).Id;
            }
            else if (secondeView is OptionDialogComponent)
            {
                inViewId = (secondeView as OptionDialogComponent ?? throw new NullReferenceException("FristView is null")).parent.Id;
                inOptionId = (secondeView as OptionDialogComponent ?? throw new NullReferenceException("FristView is null")).Id;
            }
            else throw new ArgumentException("Unknow view in LinkDatapackage");

            Vector4[] vectorArray = Lines.Select(line => new Vector4(line.X1, line.Y1, line.X2, line.Y2)).ToArray();
            return new LinkDataDialogPackageSerialize(outViewId,outBindingId,inViewId,inBindingId,vectorArray);
        }
    }

    public partial class DialogComponentView : UserControl
    {
        private const int marginBindingDialogCopmonentViewLeft = 25;
        private const int marginBindingDialogCopmonentViewRight = 5;
        private const int marginBindingDialogCopmonentTop = 0;
       
        public bool IsSelectObj { get;private set; } = false ;

        public Canvas canvas { get; private set; }
        private Point _mousePosition;
        private bool _isMouseDown = false;

        public List<BindingDialogComponentView> bindingDialogComponentViews = new List<BindingDialogComponentView>();
        public List<LinkDataDialogPackage> linkDataPackages { get; private set; } = new List<LinkDataDialogPackage>();
        public BindingDialogComponentView LeftBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView RightBindingDialogComponentView { get; private set; }
        public TextBlock _TextBlockComponentName { get; set; }
        public ObservableCollection<OptionDialogComponent> Options { get; private set; } = new ObservableCollection<OptionDialogComponent>();
        // TODO сделать понмиание номера элемента при связях
        public LinkedObject Source { get; set; }
        public bool isMove { get; set; }

        public int Id { get; private set; }

        public DialogComponentView(Canvas drawingCanvas, int id)
        {
            InitializeComponent();
            canvas = drawingCanvas;
            this._TextBlockComponentName = TextBlockComponentName;

            isMove = false;
            Id = id;
        }
        public void AddOption()
        {
            var option = new OptionDialogComponent(canvas, this, Options.Count);
            option.HorizontalAlignment = HorizontalAlignment.Center;
            option.Margin = new Thickness(0, 10, 0, 0);
            OptionStackPanel.Children.Add(option);
            Options.Add(option);
            if(Options.Count > 0) 
            {
                RightBindingDialogComponentView.Visibility = Visibility.Hidden;
            }
            else
            {
                RightBindingDialogComponentView.Visibility = Visibility.Visible;
            }
        }
        
        public void AddOption(LinkedObject source)
        {
            var option = new OptionDialogComponent(canvas, this, Options.Count);
            option.HorizontalAlignment = HorizontalAlignment.Center;
            option.Margin = new Thickness(0, 10, 0, 0);
            option.OptionSource = source;
            OptionStackPanel.Children.Add(option);
            Options.Add(option);
            if (Options.Count > 0)
            {
                RightBindingDialogComponentView.Visibility = Visibility.Hidden;
            }
            else
            {
                RightBindingDialogComponentView.Visibility = Visibility.Visible;
            }
        }

        public void RemoveOption(OptionDialogComponent option)
        {
            foreach (var package in option.linkDataOptionPackages)
            {
                if (package.firstView is DialogComponentView)
                {
                    DialogComponentView firstView = (DialogComponentView)package.firstView;
                    firstView.UnLinkWith(package);
                }
                else if (package.firstView is OptionDialogComponent)
                {
                    OptionDialogComponent firstView = (OptionDialogComponent)package.firstView;
                    firstView.UnLinkWith(package);
                }

                if (package.secondeView is DialogComponentView)
                {
                    DialogComponentView secondeView = (DialogComponentView)package.secondeView;
                    secondeView.UnLinkWith(package);
                }
                else if (package.secondeView is OptionDialogComponent)
                {
                    OptionDialogComponent secondeView = (OptionDialogComponent)package.secondeView;
                    secondeView.UnLinkWith(package);
                }
            }
            OptionStackPanel.Children.Remove(option);
            Options.Remove(option);
            if (Options.Count > 0)
            {
                RightBindingDialogComponentView.Visibility = Visibility.Hidden;
            }
            else
            {
                RightBindingDialogComponentView.Visibility = Visibility.Visible;
            }
        }

        public void Select() 
        {
            OutSideBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x13, 0xA9, 0xF7));
            OutSideBorder.Background = new SolidColorBrush(Color.FromArgb(0x3F, 0x13, 0xA9, 0xF7));
            IsSelectObj = true;
        }

        public void UnSelect()
        {
            OutSideBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x02, 0x02, 0x02));
            OutSideBorder.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x10, 0x010, 0x10));
            IsSelectObj = false;
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
                LeftBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointLeftBindingComponent(), TypePointBindingView.InputTypePoint, 0);
                if (bindingDialogComponentViews.Contains(LeftBindingDialogComponentView) == false)
                    bindingDialogComponentViews.Add(LeftBindingDialogComponentView);
            }

            if (RightBindingDialogComponentView == null)
            {
                RightBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointRightBindingComponent(), TypePointBindingView.OutTypePoint, 1);
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
                    if (linkData.firstView == this)
                    {
                        linkData.Lines.First().X1 =
                            offset.X + linkData.Lines.First().X1;
                        linkData.Lines.First().Y1 =
                            linkData.Lines.First().Y1 + offset.Y;
                    }

                    else if (linkData.secondeView == this)
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
                        if (linkDataOption.firstView == option)
                        {
                            linkDataOption.Lines.First().X1 =
                                offset.X + linkDataOption.Lines.First().X1;
                            linkDataOption.Lines.First().Y1 =
                                linkDataOption.Lines.First().Y1 + offset.Y;
                        }

                        else if (linkDataOption.secondeView == option)
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
                x: Canvas.GetLeft(this) - marginBindingDialogCopmonentViewLeft,
                y: Canvas.GetTop(this) + Header.ActualHeight / 2f + marginBindingDialogCopmonentTop
            );
        }
        private Point GetPointRightBindingComponent()
        {
          
            return new Point(
                x: Canvas.GetLeft(this) + ActualWidth + marginBindingDialogCopmonentViewRight,
                y: Canvas.GetTop(this) + Header.ActualHeight / 2f + marginBindingDialogCopmonentTop
            );
        }
        public void Link(LinkDataDialogPackage linkDataPackage)
        {
            if (linkDataPackage.firstView is DialogComponentView && linkDataPackage.secondeView is DialogComponentView)
            {
                DialogComponentView firstView = (DialogComponentView)linkDataPackage.firstView;
                DialogComponentView secondeView = (DialogComponentView)linkDataPackage.secondeView;
                Source?.Bounds(firstView.Source, secondeView.Source);
            }
            else if (linkDataPackage.firstView is DialogComponentView && linkDataPackage.secondeView is OptionDialogComponent)
            {
                DialogComponentView firstView = (DialogComponentView)linkDataPackage.firstView;
                OptionDialogComponent secondeView = (OptionDialogComponent)linkDataPackage.secondeView;
                Source?.Bounds(firstView.Source, secondeView.OptionSource);

            }
            else if (linkDataPackage.firstView is OptionDialogComponent && linkDataPackage.secondeView is DialogComponentView)
            {
                OptionDialogComponent firstView = (OptionDialogComponent)linkDataPackage.firstView;
                DialogComponentView secondeView = (DialogComponentView)linkDataPackage.secondeView;
                Source?.Bounds(firstView.OptionSource, secondeView.Source);

            }
            else if (linkDataPackage.firstView is OptionDialogComponent && linkDataPackage.secondeView is OptionDialogComponent)
            {
                throw new Exception("You Eblan how you get in linkDataPackage OptionDialogComponent && OptionDialogComponent");

            }

            linkDataPackages.Add(linkDataPackage);
        }
        public void UnLinkWith(LinkDataDialogPackage linkedPackage)
        {
            foreach (var line in linkedPackage.Lines)
            {
                canvas.Children.Remove(line);
            }

            if (linkedPackage.firstView == this)
            {
                if (linkedPackage.secondeView is DialogComponentView)
                {
                    DialogComponentView secondeView = (DialogComponentView)linkedPackage.secondeView;
               
                    Source?.UnBounds(secondeView.Source);
                }
                else if (linkedPackage.secondeView is OptionDialogComponent)
                {
                    OptionDialogComponent secondeView = (OptionDialogComponent)linkedPackage.secondeView;
                    Source?.UnBounds(secondeView.OptionSource);
                }
            }

            else if (linkedPackage.secondeView == this)
            {

                if (linkedPackage.firstView is DialogComponentView)
                {
                    DialogComponentView firstView = (DialogComponentView)linkedPackage.firstView;
                    Source?.UnBounds(firstView.Source);
                }
                else if (linkedPackage.firstView is OptionDialogComponent)
                {
                    OptionDialogComponent firstView = (OptionDialogComponent)linkedPackage.firstView;
                    Source?.UnBounds(firstView.OptionSource);
                }
            }
            else
                throw new Exception("Bad link two view is not have this view in link");

            linkDataPackages.Remove(linkedPackage);
        }
        public void SetName()
        {
            string fullName = (Source as SayingElementViewDFD).elementOld.text;

            if (fullName.Length <= 20)
            {
                TextBlockComponentName.Text = fullName;
                return;
            }

            int i = 0;
            string shortName = "";
            foreach (var ch in fullName)
            {
                if (i == 20)
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
        public List<LinkDataDialogPackage> Destroy()
        {
            var options = Options.ToList();
            var packagesInObj = linkDataPackages.ToList();
            foreach (var option in options)
            {
                packagesInObj.AddRange(option.Destroy());
                Options.Remove(option);
            }

            var packages = linkDataPackages.ToList();

            foreach (var package in packages)
            {
                if (package.firstView is DialogComponentView)
                {
                    DialogComponentView firstView = (DialogComponentView)package.firstView;
                    firstView.UnLinkWith(package);
                }
                else if (package.firstView is OptionDialogComponent)
                {
                    OptionDialogComponent firstView = (OptionDialogComponent)package.firstView;
                    firstView.UnLinkWith(package);
                }

                if (package.secondeView is DialogComponentView)
                {
                    DialogComponentView secondeView = (DialogComponentView)package.secondeView;
                    secondeView.UnLinkWith(package);
                }
                else if (package.secondeView is OptionDialogComponent)
                {
                    OptionDialogComponent secondeView = (OptionDialogComponent)package.secondeView;
                    secondeView.UnLinkWith(package);
                }
            }

            canvas.Children.Remove(this.LeftBindingDialogComponentView);
            canvas.Children.Remove(this.RightBindingDialogComponentView);
            canvas.Children.Remove(this);
            
            return packagesInObj;
        }
    }
}