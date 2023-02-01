﻿using Microsoft.VisualBasic.FileIO;
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
    /// 

    public class LinkDataOptionPackage
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
    }
    public partial class OptionDialogComponent : UserControl
    {
        public RequiredBindingOptionComponentView LeftBindingDialogComponentView { get; private set; }
        public RequiredBindingOptionComponentView RightBindingDialogComponentView { get; private set; }

        private const int marginBindingDialogCopmonentView = 10;
        private const int bindingDialogComponentWidth = 10;

        public DialogComponentView parent { get; private set; }
        private Canvas canvas;

        public LinkedObject OptionSource { get; set; }

        public List<LinkDataOptionPackage> linkDataOptionPackages { get; private set; } = new List<LinkDataOptionPackage>();

        public OptionDialogComponent(Canvas drawingCanvas, DialogComponentView componentView)
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

        public void LinkWith(LinkDataOptionPackage linkDataOption) 
        {
            linkDataOptionPackages.Add(linkDataOption);

            if (linkDataOption.firstOptionComponent == this) 
            {
                OptionSource?.Bounds(linkDataOption.secondeOptionComponent.OptionSource);
            }
            else if (linkDataOption.secondeOptionComponent == this)
            {
                OptionSource?.Bounds(linkDataOption.firstOptionComponent.OptionSource);
            }    
            else
                throw new InvalidOperationException("You link different object but this object is not one from them");
        }

        public void UnLinkWith(LinkDataOptionPackage linkedPackage) 
        {
            foreach (var line in linkedPackage.Lines)
            {
                parent.canvas.Children.Remove(line);
            }
            if (linkedPackage.firstOptionComponent == this)
            {
                OptionSource?.UnBounds(linkedPackage.secondeOptionComponent.OptionSource);
            }
            else if (linkedPackage.secondeOptionComponent == this)
            {
                OptionSource?.UnBounds(linkedPackage.firstOptionComponent.OptionSource);
            }

            linkDataOptionPackages.Remove(linkedPackage);
        }

        private void InitEmptyBindings()
        {

            if (LeftBindingDialogComponentView == null)
            {
                LeftBindingDialogComponentView = new RequiredBindingOptionComponentView(this, canvas, GetPointLeftBindingComponent());
                LeftBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Green);

            }

            if (RightBindingDialogComponentView == null)
            {
                RightBindingDialogComponentView = new RequiredBindingOptionComponentView(this, canvas, GetPointRightBindingComponent());
                RightBindingDialogComponentView.ShapeView.Stroke = new SolidColorBrush(Colors.Blue);
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
                x: Canvas.GetLeft(parent) + (bindingDialogComponentWidth / 2f),
                y: Canvas.GetTop(parent) + (this.OptionCanvas.Height / 2f) + (parent.DialogComponentCanvas.Height / 2f) + ((GetIndex() + 1) * this.OptionCanvas.Height + 10 * (GetIndex() + 1)) - 3 
            );
        }

        private Point GetPointRightBindingComponent()
        {
            return new Point(
                x: Canvas.GetLeft(parent) + this.OptionCanvas.Width + (bindingDialogComponentWidth / 2f) + marginBindingDialogCopmonentView + 25,
                y: Canvas.GetTop(parent) + (this.OptionCanvas.Height / 2f) + (parent.DialogComponentCanvas.Height / 2f) + ((GetIndex() + 1) * this.OptionCanvas.Height + 10 * (GetIndex() + 1)) - 3
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
            string fullName = (OptionSource as SayingElementViewDFD).sayingElement.text;

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
        public void Destroy() 
        {
            var packages = linkDataOptionPackages.ToList();

            foreach (var package in packages)
            {
                package.firstOptionComponent.UnLinkWith(package);
                package.secondeOptionComponent.UnLinkWith(package);
            }

            canvas.Children.Remove(LeftBindingDialogComponentView);
            canvas.Children.Remove(RightBindingDialogComponentView);
            canvas.Children.Remove(this);
        }
    }
}