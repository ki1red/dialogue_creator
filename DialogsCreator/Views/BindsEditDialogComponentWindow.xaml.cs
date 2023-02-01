
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
        public BindsEditDialogComponentWindow(DialogComponentView dialogComponentView)
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
        }

        public void UnLink(LinkDataDialogPackage package,ElemetInBindingsWindow children) 
        {
            BindingsComponentStack.Children.Remove(children);
            package.firstDialogComponent.UnLinkWith(package);
            package.secondeDialogComponent.UnLinkWith(package);
        }

        public void UnLink(LinkDataOptionPackage package, ElemetInBindingsWindow children)
        {
            BindingsComponentStack.Children.Remove(children);
            package.firstOptionComponent.UnLinkWith(package);
            package.secondeOptionComponent.UnLinkWith(package);
        }

    }
}
