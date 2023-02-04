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
    /// Логика взаимодействия для ElemetInBindingsWindow.xaml
    /// </summary>
    public partial class ElemetInBindingsWindow : UserControl
    {
        private LinkDataDialogPackage package;
  /*      private LinkDataOptionPackage optionPackage;*/
        private BindsEditDialogComponentWindow parent;

        public ElemetInBindingsWindow(LinkDataDialogPackage linkDataDialogPackage, BindsEditDialogComponentWindow parent)
        {
            InitializeComponent();
           
            if(linkDataDialogPackage.firstView is DialogComponentView && linkDataDialogPackage.secondeView is DialogComponentView)
            {
                DialogComponentView firstView = (DialogComponentView)linkDataDialogPackage.firstView;
                DialogComponentView secondeView = (DialogComponentView)linkDataDialogPackage.secondeView;
                ItemElementName.Text = $"Dialog link {firstView.TextBlockComponentName.Text} to {secondeView.TextBlockComponentName.Text}";
                this.package = linkDataDialogPackage;
                this.parent = parent;
            }
            else if (linkDataDialogPackage.firstView is DialogComponentView && linkDataDialogPackage.secondeView is OptionDialogComponent)
            {
                DialogComponentView firstView = (DialogComponentView)linkDataDialogPackage.firstView;
                OptionDialogComponent secondeView = (OptionDialogComponent)linkDataDialogPackage.secondeView;
                ItemElementName.Text = $"Dialog link {firstView.TextBlockComponentName.Text} to {secondeView.TextBlockComponentName.Text}";
                this.package = linkDataDialogPackage;
                this.parent = parent;
            }
            else if (linkDataDialogPackage.firstView is OptionDialogComponent && linkDataDialogPackage.secondeView is DialogComponentView)
            {
                OptionDialogComponent firstView = (OptionDialogComponent)linkDataDialogPackage.firstView;
                DialogComponentView secondeView = (DialogComponentView)linkDataDialogPackage.secondeView;
                ItemElementName.Text = $"Dialog link {firstView.TextBlockComponentName.Text} to {secondeView.TextBlockComponentName.Text}";
                this.package = linkDataDialogPackage;
                this.parent = parent;
            }
            else if (linkDataDialogPackage.firstView is OptionDialogComponent && linkDataDialogPackage.secondeView is OptionDialogComponent)
            {
                OptionDialogComponent firstView = (OptionDialogComponent)linkDataDialogPackage.firstView;
                OptionDialogComponent secondeView = (OptionDialogComponent)linkDataDialogPackage.secondeView;
                ItemElementName.Text = $"Dialog link {firstView.TextBlockComponentName.Text} to {secondeView.TextBlockComponentName.Text}";
                this.package = linkDataDialogPackage;
                this.parent = parent;
            }
        }

/*        public ElemetInBindingsWindow(LinkDataOptionPackage linkDataOptionPackage, BindsEditDialogComponentWindow parent)
        {
            InitializeComponent();
            ItemElementName.Text = $"Option link {linkDataOptionPackage.firstOptionComponent.TextBlockComponentName.Text} to {linkDataOptionPackage.secondeOptionComponent.TextBlockComponentName.Text} " ;
            optionPackage = linkDataOptionPackage;
            this.parent = parent;
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (package != null)
                parent.UnLink(package, this);
/*            else if (optionPackage != null)
                parent.UnLink(optionPackage, this);*/
            else
                throw new Exception("Package dialog and option is null");
        }
    }
}
