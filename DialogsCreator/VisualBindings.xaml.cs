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
using static System.Net.WebRequestMethods;
using Path = System.Windows.Shapes.Path;

namespace DialogsCreator
{
    public partial class VisualBindings : Window
    {
        // ===========================================================================================================================
        // ============================= ПЕРЕМЕННЫЕ ДЛЯ ВИЗУАЛЬНОГО ОТОБРАЖЕНИЯ В ОКНЕ ===============================================
        // ===========================================================================================================================

        private const string windowTitle = "Создатель диалогов.";
        private const string languageTitle = "Язык файла";

        // ===========================================================================================================================
        // ======================== ПЕРЕМЕННЫЕ ДЛЯ ХРАНЕНИЯ ИНФОРМАЦИИ О DFD В ФАЙЛЕ И СТРУКТУРЕ =====================================
        // ===========================================================================================================================

        private FileManagerDLAG selFile = new FileManagerDLAG();
        private WPFtoDFD modelView;

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

            InitializeBaseComponentsWindow();
            InitializeComponentsTopMenu();
            InitializeSubscribedClickForMenu();
            InitializeSubscribedMouseForCanvas();
            

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

        private void InitializeBaseComponentsWindow()
        {
            this.Title = windowTitle;
            this.Label_informationOfLanguage.Content = null;
        }
        private void InitializeComponentsTopMenu()
        {
            this.MenuItem_saveFile.IsEnabled = false;
            this.MenuItem_saveAsFile.IsEnabled = false;
            this.MenuItem_exportFile.IsEnabled = false; // TODO когда добавим преобразование из dlag в dfd, сделать по умолчанию доступным 
            this.MenuItem_importFile.IsEnabled = false;
            this.MenuItem_closeFile.IsEnabled = false;
            this.MenuItem_objectSettings.IsEnabled = false;
            this.MenuItem_addObject.IsEnabled = true;
            this.MenuItem_editObject.IsEnabled = false;
            this.MenuItem_deleteObject.IsEnabled = false;
        }
        private void InitializeSubscribedClickForMenu()
        {
            MenuItem_createFile.Click += MenuItem_createFile_Click;
            MenuItem_openFile.Click += MenuItem_openFile_Click;
            MenuItem_saveFile.Click += MenuItem_saveFile_Click;
            MenuItem_saveAsFile.Click += MenuItem_saveAsFile_Click;
            MenuItem_closeFile.Click += MenuItem_closeFile_Click;

            MenuItem_createFile.Click += UpdateWindowElements;
            MenuItem_openFile.Click += UpdateWindowElements;
            MenuItem_saveFile.Click += UpdateWindowElements;
            MenuItem_saveAsFile.Click += UpdateWindowElements;
            MenuItem_closeFile.Click += UpdateWindowElements;

            MenuItem_addObject.Click += MenuItem_addObject_Click;
        }
        private void InitializeSubscribedMouseForCanvas()
        {
            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseDown;
            MainCanvas.MouseLeftButtonUp += MainCanvas_MouseUp;
            MainCanvas.MouseMove += MainCanvas_MouseMove;
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
            SaveFileBeforeClosing();

            selFile.OpenFile();
        }
        private void MenuItem_createFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileBeforeClosing();

            if (selFile.CreateFile() == false)
                return;

            SelectLanguageWindow window = new SelectLanguageWindow(); // требование указать язык файла
            do
            {
                window.ShowDialog();
                selFile.language = window.language;
            } while (selFile.language == DialogsCreator.Language.none);
        }
        private void MenuItem_saveFile_Click(object sender, RoutedEventArgs e)
        {
            modelView.SerializationDFD();
        }
        private void MenuItem_saveAsFile_Click(object sender, RoutedEventArgs e)
        {
            modelView.SerializationDFD(selFile.path);
        }
        private void MenuItem_closeFile_Click(object sender, RoutedEventArgs e)
        {
            // TODO Сделать очистку MainCanvas

            selFile = new FileManagerDLAG(); // НЕ УДАЛЯТЬ, ИНАЧЕ НЕ ОТРАБОТАЕТ UpdateWindowElements
            modelView = null;

        }
        private void MenuItem_addObject_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow(ref modelView);
            window.ShowDialog();

            // TODO Добавлять объект на сцену ТОЛЬКО ЕСЛИ он ТОЧНО был создан в окне MainWindow
        }

        // ===========================================================================================================================
        // =================================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ================================================================
        // ===========================================================================================================================

        private void UpdateWindowElements(object sender, EventArgs e)
        {
            if (selFile.file != null)
            {
                this.MenuItem_saveFile.IsEnabled = true;
                this.MenuItem_saveAsFile.IsEnabled = true;
                this.MenuItem_objectSettings.IsEnabled = true;
                MenuItem_closeFile.IsEnabled = true;

                if ((sender as MenuItem).Name == MenuItem_createFile.Name || (sender as MenuItem).Name == MenuItem_openFile.Name)
                {
                    modelView = new WPFtoDFD(selFile);
                    modelView.DesirializationDFD();
                }

                selFile.language = selFile.ToLanguage(modelView.dialog.language);
                this.Title = $"{windowTitle} Открыт файл {selFile.file}.{FileManagerDLAG.type}";
                this.Label_informationOfLanguage.Content = $"{languageTitle}: {selFile.language}";
            }
            else
            {
                this.Title = windowTitle;
                this.Label_informationOfLanguage.Content = null;

                MenuItem_saveAsFile.IsEnabled = false;
                MenuItem_saveFile.IsEnabled = false;
                MenuItem_closeFile.IsEnabled = false;

                MenuItem_objectSettings.IsEnabled = false;
                MenuItem_addObject.IsEnabled = true;
                MenuItem_deleteObject.IsEnabled = false;
                MenuItem_editObject.IsEnabled = false;
            }
        }
        private void SaveFileBeforeClosing()
        {
            if (selFile.file != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Сохранить изменения перед закрытием?", "Сохранение", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_saveFile_Click(null, null);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                    default:
                        return;
                }
            }
        }
    }
}
