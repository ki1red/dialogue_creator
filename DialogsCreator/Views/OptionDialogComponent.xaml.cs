using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    /// 

/*    public class LinkDataOptionPackage
    {
        public OptionDialogComponent firstOptionComponent { get; private set; }
        public OptionDialogComponent secondeOptionComponent { get; private set; }
        public RequiredBindingOptionComponentView firstReqiredBindingDialogComponentView { get; private set; }
        public RequiredBindingOptionComponentView secondeReqiredBindingDialogComponentView { get; private set; }
        public List<Line> Lines { get; private set; }

        public LinkDataOptionPackage(
            OptionDialogComponent firstOptionComponent,
            OptionDialogComponent secondeOptionComponent,
            RequiredBindingOptionComponentView firstReqiredBindingDialogComponentView,
            RequiredBindingOptionComponentView secondeReqiredBindingDialogComponentView,
            List<Line> lines)
        {
            this.firstOptionComponent = firstOptionComponent;
            this.secondeOptionComponent = secondeOptionComponent;
            this.firstReqiredBindingDialogComponentView = firstReqiredBindingDialogComponentView;
            this.secondeReqiredBindingDialogComponentView = secondeReqiredBindingDialogComponentView;
            Lines = lines;
        }
    }*/
    public partial class OptionDialogComponent : UserControl
    {
        public BindingDialogComponentView LeftBindingDialogComponentView { get; private set; }
        public BindingDialogComponentView RightBindingDialogComponentView { get; private set; }

        public List<BindingDialogComponentView> bindingDialogComponentViews = new List<BindingDialogComponentView>();

        private const int marginBindingDialogCopmonentViewLeft = 25;
        private const int marginBindingDialogCopmonentViewRight = 5;
        private const int marginBindingDialogCopmonentTop = -37;

        public DialogComponentView parent { get; private set; }
        private Canvas canvas;
        public LinkedObject OptionSource { get; set; }
        public List<LinkDataDialogPackage> linkDataOptionPackages { get; private set; } = new List<LinkDataDialogPackage>();
        public string _textNameOption { get => TextBlockComponentName.Text; }
        public int Id;

        public OptionDialogComponent(Canvas drawingCanvas, DialogComponentView componentView,int id)
        {
            InitializeComponent();
            parent = componentView;
            canvas = drawingCanvas;
            Id = id;
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
        public void LinkWith(LinkDataDialogPackage linkDataOption)
        {
            if (linkDataOption.firstView is DialogComponentView && linkDataOption.secondeView is DialogComponentView)
            {
                throw new InvalidOperationException("How you get DialogComponentView && DialogComponentView in Option Link, You Eblan?");
            }
            else if (linkDataOption.firstView is DialogComponentView && linkDataOption.secondeView is OptionDialogComponent)
            {
                DialogComponentView firstView = (DialogComponentView)linkDataOption.firstView;
                OptionDialogComponent secondeView = (OptionDialogComponent)linkDataOption.secondeView;
                OptionSource?.Bounds(firstView.Source, secondeView.OptionSource);

            }
            else if (linkDataOption.firstView is OptionDialogComponent && linkDataOption.secondeView is DialogComponentView)
            {
                OptionDialogComponent firstView = (OptionDialogComponent)linkDataOption.firstView;
                DialogComponentView secondeView = (DialogComponentView)linkDataOption.secondeView;
                OptionSource?.Bounds(firstView.OptionSource, secondeView.Source);

            }
            else if (linkDataOption.firstView is OptionDialogComponent && linkDataOption.secondeView is OptionDialogComponent)
            {
                OptionDialogComponent firstView = (OptionDialogComponent)linkDataOption.firstView;
                OptionDialogComponent secondeView = (OptionDialogComponent)linkDataOption.secondeView;
                OptionSource?.Bounds(firstView.OptionSource, secondeView.OptionSource);

            }

            else
                throw new InvalidOperationException("You link different object but this object is not one from them");

            linkDataOptionPackages.Add(linkDataOption);
        }
        public void UnLinkWith(LinkDataDialogPackage linkedPackage)
        {
            foreach (var line in linkedPackage.Lines)
            {
                parent.canvas.Children.Remove(line);
            }

            if (linkedPackage.firstView == this)
            {
                if (linkedPackage.secondeView is DialogComponentView)
                {
                    var view = (DialogComponentView)linkedPackage.secondeView;
                    OptionSource?.UnBounds(view.Source);
                }

                else if (linkedPackage.secondeView is OptionDialogComponent) 
                {
                    var view = (OptionDialogComponent)linkedPackage.secondeView;
                    OptionSource?.UnBounds(view.OptionSource);
                }

                else throw new InvalidOperationException("EBlan what is it view in Link?");
            }
            else if (linkedPackage.secondeView == this)
            {
                if (linkedPackage.firstView is DialogComponentView)
                {
                    var view = (DialogComponentView)linkedPackage.firstView;
                    OptionSource?.UnBounds(view.Source);
                }
                
                else if (linkedPackage.firstView is OptionDialogComponent) {
                    var view = (OptionDialogComponent)linkedPackage.firstView;
                    OptionSource?.UnBounds(view.OptionSource);
                }
                else throw new InvalidOperationException("EBlan what is it view in Link?");
            }

            linkDataOptionPackages.Remove(linkedPackage);
        }
        private void InitEmptyBindings()
        {

            if (LeftBindingDialogComponentView == null)
            {
                LeftBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointLeftBindingComponent(),TypePointBindingView.InputTypePoint);
                bindingDialogComponentViews.Add(LeftBindingDialogComponentView);
            }

            if (RightBindingDialogComponentView == null)
            {
                RightBindingDialogComponentView = new BindingDialogComponentView(this, canvas, GetPointRightBindingComponent(),TypePointBindingView.OutTypePoint);
                bindingDialogComponentViews.Add(RightBindingDialogComponentView);
            }
        }
        private int GetIndex()
        {
            for (int i = 0; i < parent.Options.Count(); i++)
            {
                if (parent.Options[i] == this)
                    return i;
            }

            throw new ArgumentException("Option don't find in parent");
        }

        private Point GetPointLeftBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(parent) - marginBindingDialogCopmonentViewLeft,
                y: Canvas.GetTop(parent) + parent.ActualHeight + marginBindingDialogCopmonentTop
            );
        }
        private Point GetPointRightBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(parent) + parent.ActualWidth + marginBindingDialogCopmonentViewRight,
                y: Canvas.GetTop(parent) + parent.ActualHeight + marginBindingDialogCopmonentTop
            );
        }
        private bool CheckBindingsInit()
        {
            return
                 LeftBindingDialogComponentView != null &&
                 RightBindingDialogComponentView != null;
        }

        public void SetName()
        {
            string fullName = (OptionSource as SayingElementViewDFD).elementOld.text;

            if (fullName.Length <= 24)
            {
                TextBlockComponentName.Text = fullName;
                return;
            }

            int i = 0;
            string shortName = "";
            foreach (var ch in fullName)
            {
                if (i == 24)
                    break;
                shortName += ch;
                i++;
            }

            TextBlockComponentName.Text = shortName;
        }
        public void Destroy()
        {
            var packages = linkDataOptionPackages.ToList();

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

            canvas.Children.Remove(LeftBindingDialogComponentView);
            canvas.Children.Remove(RightBindingDialogComponentView);
            canvas.Children.Remove(this);
        }
    }
}