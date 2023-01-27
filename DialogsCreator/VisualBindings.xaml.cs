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
        // pathToFile - Dialog.dlag
        // название файла с озвучкой будет Dialog_ru.dl, который будет лежать в той же папке, что и .dlag
        // при закрытии формы файлы автоматически будут сохраняться, либо сделай кнопку
        private bool isDrawingLine;
        private Path linePath;
        private Point startPoint;
        private BindingDialogComponentView startBindingDialogComponentView;
        public VisualBindings(string pathToFile)
        {
            InitializeComponent();

            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseDown;
            MainCanvas.MouseLeftButtonUp += MainCanvas_MouseUp;
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
            dialogComponentView.TopBindingDialogComponentView.LinkWith(dialogComponentView2.TopBindingDialogComponentView, MainCanvas);
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.Source is BindingDialogComponentView)
            {
                MessageBox.Show("YOu click");
                startBindingDialogComponentView = e.Source as BindingDialogComponentView;
                startPoint = e.GetPosition(MainCanvas);
                isDrawingLine = true;
                linePath = new Path();
                linePath.Stroke = Brushes.Black;
                linePath.StrokeThickness = 5;
                linePath.StrokeStartLineCap = PenLineCap.Round;
                linePath.StrokeEndLineCap = PenLineCap.Round;

                MainCanvas.Children.Add(linePath);
            }
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawingLine)
            {
                Point currentPoint = e.GetPosition(MainCanvas);

                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = startPoint;

                BezierSegment bezierSegment = new BezierSegment();
                bezierSegment.Point1 = startPoint;
                bezierSegment.Point2 = currentPoint;
                bezierSegment.Point3 = currentPoint;

                pathFigure.Segments.Add(bezierSegment);
                pathGeometry.Figures.Add(pathFigure);

                linePath.Data = pathGeometry;
            }
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawingLine)
            {
                isDrawingLine = false;
                if (e.Source is BindingDialogComponentView)
                {
                    var currentMousePosition = e.GetPosition((IInputElement)sender);


                    var hitTestResult = VisualTreeHelper.HitTest(MainCanvas, currentMousePosition);

                    if (hitTestResult != null)
                    {
                        var hitVisual = hitTestResult.VisualHit;

                        if (hitVisual is BindingDialogComponentView)
                        {
                            var otherBindingView = (BindingDialogComponentView)hitVisual;

                            if (startBindingDialogComponentView != null && otherBindingView != startBindingDialogComponentView)
                            {
                                startBindingDialogComponentView.LinkWith(otherBindingView, MainCanvas);
                                MainCanvas.Children.Remove(linePath);
                                linePath = null;
                            }
                        }
                    }

                    MainCanvas.Children.Remove(linePath);
                    linePath = null;
                }
            }
        }
    }
}
