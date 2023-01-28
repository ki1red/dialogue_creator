using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shell;
using DialogsCreator.Views;
using Path = System.Windows.Shapes.Path;

namespace DialogsCreator
{
    public partial class VisualBindings : Window
    {
        private BindingDialogComponentView startBindingDialogComponentView;
        private BindingDialogComponentView endBindingDialogComponentView;
        private Line currentLine;
        private List<Line> linesCollection = new List<Line>();

        public VisualBindings(string pathToFile)
        {
            InitializeComponent();

            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseDown;
            MainCanvas.MouseRightButtonUp += MainCanvas_MouseUp;
            MainCanvas.MouseMove += MainCanvas_MouseMove;

            DialogComponentView dialogComponentView = new DialogComponentView(MainCanvas);
            MainCanvas.Children.Add(dialogComponentView);
            Canvas.SetLeft(dialogComponentView, 250);
            Canvas.SetTop(dialogComponentView, 150);
            DialogComponentView dialogComponentView2 = new DialogComponentView(MainCanvas);
            MainCanvas.Children.Add(dialogComponentView2);
            Canvas.SetLeft(dialogComponentView2, 25);
            Canvas.SetTop(dialogComponentView2, 25);
            dialogComponentView.ShowBindigsDialogComponentsView();
            dialogComponentView2.ShowBindigsDialogComponentsView();

            dialogComponentView.AddOption();
            dialogComponentView.AddOption();
            dialogComponentView.AddOption();
            dialogComponentView.AddOption();
            foreach(var option in dialogComponentView.Options) 
            { 
                option.ShowBindigsDialogComponentsView();
            }
      
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
           
            if (currentLine != null)
            {
                Point currentPoint = e.GetPosition(MainCanvas);

                currentLine.X2 = currentPoint.X - 3;
                currentLine.Y2 = currentPoint.Y - 3;
            }
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.Source is BindingDialogComponentView)
            {
                if (startBindingDialogComponentView == null)
                {
                    startBindingDialogComponentView = e.Source as BindingDialogComponentView;
                    currentLine = new Line();
                    currentLine.Stroke = new SolidColorBrush(Colors.Black);

                    currentLine.StrokeStartLineCap = PenLineCap.Round;
                    currentLine.StrokeEndLineCap = PenLineCap.Round;

                    currentLine.StrokeThickness = 5;
                    currentLine.X1 = Canvas.GetLeft(startBindingDialogComponentView) + startBindingDialogComponentView.Width / 2;
                    currentLine.Y1 = Canvas.GetTop(startBindingDialogComponentView) + startBindingDialogComponentView.Height / 2;

                    currentLine.X2 = e.GetPosition(MainCanvas).X;
                    currentLine.Y2 = e.GetPosition(MainCanvas).Y;
                    MainCanvas.Children.Add(currentLine);
                }
                else
                {
                    endBindingDialogComponentView = e.Source as BindingDialogComponentView;
                    if (startBindingDialogComponentView != endBindingDialogComponentView && CanLink())
                    {
                        currentLine.X2 = Canvas.GetLeft(endBindingDialogComponentView) + endBindingDialogComponentView.Width / 2;
                        currentLine.Y2 = Canvas.GetTop(endBindingDialogComponentView) + endBindingDialogComponentView.Height / 2;

                        linesCollection.Add(currentLine);
                        startBindingDialogComponentView.LinkWith(endBindingDialogComponentView,linesCollection);

                        linesCollection.Clear();
                        currentLine = null;
                        startBindingDialogComponentView = null;
                        endBindingDialogComponentView = null;
                    }
                }
            }

            else if (startBindingDialogComponentView != null)
            {
                var x = currentLine.X2;
                var y = currentLine.Y2;

                linesCollection.Add(currentLine);

                currentLine = new Line();
                currentLine.Stroke = new SolidColorBrush(Colors.Black);
                currentLine.StrokeThickness = 5;
                currentLine.StrokeStartLineCap = PenLineCap.Round;
                currentLine.StrokeEndLineCap = PenLineCap.Round;
                MainCanvas.Children.Add(currentLine);

                currentLine.X1 = x;
                currentLine.Y1 = y;
                currentLine.X2 = x;
                currentLine.Y2 = y;
            }
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RemoveUnconnectedLines();
        }

        private bool CanLink()
        {
            // Проверки для определения, можно ли соединить startBindingDialogComponentView и endBindingDialogComponentView
            return true;
        }

        private void RemoveUnconnectedLines()
        {
            foreach (var line in linesCollection) 
            {
                MainCanvas.Children.Remove(line);
            }
            startBindingDialogComponentView = null;
            endBindingDialogComponentView = null;
            linesCollection.Clear();
            MainCanvas.Children.Remove(currentLine);
            currentLine = null;
        }
    }
}
