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
        // ===========================================================================================================================
        // ======================== ПЕРЕМЕННЫЕ ДЛЯ ХРАНЕНИЯ ИНФОРМАЦИИ О DFD В ФАЙЛЕ И СТРУКТУРЕ =====================================
        // ===========================================================================================================================

        FileManagerDLAG selFile = new FileManagerDLAG();
        WPFtoDFD modelView;

        // ===========================================================================================================================
        // ================================ ПЕРЕМЕННЫЕ ИЛЬИ ХЗ ДЛЯ ЧЕГО ==============================================================
        // ===========================================================================================================================

        private bool isDrawingLine;
        private Path linePath;
        private Point startPoint;
        private BindingDialogComponentView startBindingDialogComponentView;

        // ===========================================================================================================================
        // ================================ КОНСТРУКТОРЫ ФОРМЫ VISUAL BINDINGS =======================================================
        // ===========================================================================================================================
        public VisualBindings()
        {
            InitializeComponent();

            InitializeComponentsTopMenu();

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

        // ===========================================================================================================================
        // ================================ ИНИЦИАЛИЗАЦИИ ГРУПП КОМПОНЕНТОВ ФОРМЫ ====================================================
        // ===========================================================================================================================
        public void InitializeComponentsTopMenu()
        {
            this.MenuItem_saveFile.IsEnabled = false;
            this.MenuItem_saveAsFile.IsEnabled = false;
            this.MenuItem_objectSettings.IsEnabled = false;
        }
        // ===========================================================================================================================
        // ======================================== РАБОТА С MAINCANVAS ==============================================================
        // ===========================================================================================================================
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


        // ===========================================================================================================================
        // =================================== РАБОТА С ВЕРХНИМ МЕНЮ =================================================================
        // ===========================================================================================================================
        private void MenuItem_openFile_Click(object sender, RoutedEventArgs e)
        {
            selFile.OpenFile();

            if (selFile.file != null)
            {
                this.MenuItem_saveFile.IsEnabled = true;
                this.MenuItem_saveAsFile.IsEnabled = true;
                this.MenuItem_objectSettings.IsEnabled = true;

                //if (r.r == Roots.root.translator || r.r == Roots.root.admin)
                //    this.TranslatingFile.IsEnabled = true;
                //if (r.r == Roots.root.scenarist || r.r == Roots.root.admin)
                //    this.visBindings.IsEnabled = true;
            }
            modelView = new WPFtoDFD(selFile);
            modelView.DesirializationDFD();
        }

        private void MenuItem_createFile_Click(object sender, RoutedEventArgs e)
        {
            selFile.CreateFile();

             if (selFile.file != null)
             {
                this.MenuItem_saveFile.IsEnabled = true;
                this.MenuItem_saveAsFile.IsEnabled = true;
                this.MenuItem_objectSettings.IsEnabled = true;

                //if (r.r == Roots.root.translator || r.r == Roots.root.admin)
                //     this.TranslatingFile.IsEnabled = true;
                // if (r.r == Roots.root.scenarist || r.r == Roots.root.admin)
                //    this.visBindings.IsEnabled = true;
            }

             modelView = new WPFtoDFD(selFile);
             modelView.DesirializationDFD();
        }

        private void MenuItem_saveFile_Click(object sender, RoutedEventArgs e)
        {
            modelView.SerializationDFD();
        }

        private void MenuItem_saveAsFile_Click(object sender, RoutedEventArgs e)
        {
            modelView.SerializationDFD(selFile.path);
        }
        private void AddObject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow(ref modelView);
            window.ShowDialog();

            // TODO Добавлять объект на сцену ТОЛЬКО ЕСЛИ он был создан в окне MainWindow
        }
    }
}
