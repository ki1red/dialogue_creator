
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
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
using System.Windows.Shapes;

namespace DialogsCreator.Views
{
    /// <summary>
    /// Логика взаимодействия для BindsEditDialogComponentWindow.xaml
    /// </summary>
    public partial class BindsEditDialogComponentWindow : Window
    {
        List<LinkDataDialogPackage> packages;

        public BindsEditDialogComponentWindow(DialogComponentView dialogComponentView, List<LinkDataDialogPackage> packages)
        {
            InitializeComponent();

            foreach (var data in dialogComponentView.linkDataPackages)
            {
                BindingsComponentStack.Children.Add(new ElemetInBindingsWindow(data, this));
            }

            foreach (var option in dialogComponentView.Options)
            {
                foreach (var dataOption in option.linkDataOptionPackages)
                {
                    BindingsComponentStack.Children.Add(new ElemetInBindingsWindow(dataOption, this));
                }
            }

            this.packages = packages;
        }

        public void UnLink(LinkDataDialogPackage package,ElemetInBindingsWindow children) 
        {
            BindingsComponentStack.Children.Remove(children);
            if(package.firstView is DialogComponentView) 
            {
                DialogComponentView firstView = (DialogComponentView)package.firstView;
                firstView.UnLinkWith(package);
            }
            else if(package.firstView is OptionDialogComponent) {
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

            packages.Remove(package);
           
        }
    }
}
