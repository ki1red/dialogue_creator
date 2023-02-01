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
using System.Windows.Markup;
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
        private SelectionObject selectionObject = new SelectionObject();
        private InfoPanel infoPanel = new InfoPanel();

        // ===========================================================================================================================
        // ================================ ПЕРЕМЕННЫЕ ИЛЬИ ХЗ ДЛЯ ЧЕГО ==============================================================
        // ===========================================================================================================================

        private BindingDialogComponentView startBindingDialogComponentView;
        private BindingDialogComponentView endBindingDialogComponentView;
        private RequiredBindingOptionComponentView startReqiredBindingDialogComponentView;
        private RequiredBindingOptionComponentView endReqiredBindingDialogComponentView;
        private Line currentLine;
        private List<Line> linesCollection = new List<Line>();
  
        // ===========================================================================================================================
        // ================================ КОНСТРУКТОРЫ ФОРМЫ VISUAL BINDINGS =======================================================
        // ===========================================================================================================================
        public delegate void SelectedViewtHandler(object obj);
        public event SelectedViewtHandler SelectViewEvent;

        public VisualBindings()
        {
            InitializeComponent();

            InitializeSubscribedBaseComponentsWindow();
            InitializeBaseComponentsWindow();
            InitializeComponentsTopMenu();
            InitializeSubscribedClickForMenu();
            InitializeSubscribedMouseForCanvas();

            SelectViewEvent += selectionObject.Select;

            DialogComponentView dialogComponentView = new DialogComponentView(MainCanvas);
            MainCanvas.Children.Add(dialogComponentView);
            Canvas.SetLeft(dialogComponentView, 250);
            Canvas.SetTop(dialogComponentView, 150);
            dialogComponentView.ShowBindigsDialogComponentsView();
            
            DialogComponentView dialogComponentView2 = new DialogComponentView(MainCanvas);
            MainCanvas.Children.Add(dialogComponentView2);
            Canvas.SetLeft(dialogComponentView2, 25);
            Canvas.SetTop(dialogComponentView2, 25);
            dialogComponentView2.ShowBindigsDialogComponentsView();
            dialogComponentView2.TextBlockComponentName.Text += " 2";
            DialogComponentView dialogComponentView3 = new DialogComponentView(MainCanvas);
            MainCanvas.Children.Add(dialogComponentView3);
            Canvas.SetLeft(dialogComponentView3, 100);
            Canvas.SetTop(dialogComponentView3, 100);
            dialogComponentView3.ShowBindigsDialogComponentsView();
            dialogComponentView3.TextBlockComponentName.Text += " 3";

            dialogComponentView.AddOption();
            dialogComponentView.AddOption();
            dialogComponentView.Source = new TestBindAndUnbinOBJ();
            dialogComponentView2.Source = new TestBindAndUnbindOBJ2();
            dialogComponentView2.AddOption();
            dialogComponentView2.AddOption();

            dialogComponentView3.AddOption();
            dialogComponentView3.AddOption();

            foreach (var option in dialogComponentView.Options)
            {
                option.ShowBindigsDialogComponentsView();
            }

            foreach (var option in dialogComponentView2.Options)
            {
                option.ShowBindigsDialogComponentsView();
            }

            foreach (var option in dialogComponentView3.Options)
            {
                option.ShowBindigsDialogComponentsView();
            }
        }

        // ===========================================================================================================================
        // ================================ ИНИЦИАЛИЗАЦИИ ГРУПП КОМПОНЕНТОВ ФОРМЫ ====================================================
        // ===========================================================================================================================

        private void InitializeSubscribedBaseComponentsWindow()
        {
            this.Closed += SaveFileBeforeClosing(null, null);
            this.Closing += Close;
        }
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
            this.MenuItem_deleteObject.Click += MenuItem_deleteObject_Click;
        }

        private void MenuItem_deleteObject_Click(object sender, RoutedEventArgs e)
        {
            ElementDFD element;// TODO обращение к объекту SelectionObject и взятие из него выбранного ElementDFD
            if (selectionObject.selected == TypeObject.element)
                element = selectionObject.element;
            else
                return;

            modelView.dialog.Delete(element);
            this.MenuItem_deleteObject.IsEnabled = false;
        }

        private void InitializeSubscribedMouseForCanvas()
        {
            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseDown;
            MainCanvas.MouseLeftButtonUp += MainCanvasLeftMouseUp;
            MainCanvas.MouseRightButtonUp += MainCanvas_MouseUp;
            MainCanvas.MouseMove += MainCanvas_MouseMove;

  
        }

        // ===========================================================================================================================
        // ======================================== РАБОТА С MAINCANVAS ==============================================================
        // ===========================================================================================================================

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e) // TODO добавить подписку на событие при нажатии. если нажата ЛКМ , то передавать тип объекта в SelectionObject.Select и InfoPanel.Show
        {
            if (e.Source is BindingDialogComponentView && startReqiredBindingDialogComponentView == null)
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
                        startBindingDialogComponentView.LinkWith(endBindingDialogComponentView, linesCollection);

                        linesCollection.Clear();
                        currentLine = null;
                        startBindingDialogComponentView = null;
                        endBindingDialogComponentView = null;
                    }
                }
            }

            else if(e.Source is RequiredBindingOptionComponentView && startBindingDialogComponentView == null) 
            { 
                if(startReqiredBindingDialogComponentView == null) 
                {
                    startReqiredBindingDialogComponentView = e.Source as RequiredBindingOptionComponentView;
                    currentLine = new Line();
                    currentLine.Stroke = new SolidColorBrush(Colors.Black);

                    currentLine.StrokeStartLineCap = PenLineCap.Round;
                    currentLine.StrokeEndLineCap = PenLineCap.Round;

                    currentLine.StrokeThickness = 5;
                    currentLine.X1 = Canvas.GetLeft(startReqiredBindingDialogComponentView) + startReqiredBindingDialogComponentView.Width / 2;
                    currentLine.Y1 = Canvas.GetTop(startReqiredBindingDialogComponentView) + startReqiredBindingDialogComponentView.Height / 2;

                    currentLine.X2 = e.GetPosition(MainCanvas).X;
                    currentLine.Y2 = e.GetPosition(MainCanvas).Y;
                    MainCanvas.Children.Add(currentLine);
                }
                else 
                {
                    endReqiredBindingDialogComponentView = e.Source as RequiredBindingOptionComponentView;
                    if (startReqiredBindingDialogComponentView != endReqiredBindingDialogComponentView && CanLink())
                    {
                        currentLine.X2 = Canvas.GetLeft(endReqiredBindingDialogComponentView) + endReqiredBindingDialogComponentView.Width / 2;
                        currentLine.Y2 = Canvas.GetTop(endReqiredBindingDialogComponentView) + endReqiredBindingDialogComponentView.Height / 2;

                        linesCollection.Add(currentLine);
                        startReqiredBindingDialogComponentView.LinkWith(endReqiredBindingDialogComponentView, linesCollection);

                        linesCollection.Clear();
                        currentLine = null;
                        startReqiredBindingDialogComponentView = null;
                        endReqiredBindingDialogComponentView = null;
                    }
                }
            }

            else if ((startBindingDialogComponentView != null || startReqiredBindingDialogComponentView != null) && currentLine != null)
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
        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentLine != null)
            {
                Point currentPoint = e.GetPosition(MainCanvas);

                currentLine.X2 = currentPoint.X - 3;
                currentLine.Y2 = currentPoint.Y - 3;
            }
        }
        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.Source is DialogComponentView && currentLine == null)
            {
                var component = e.Source as DialogComponentView;
                var windowInfo = new BindsEditDialogComponentWindow(component);
                windowInfo.ShowDialog();

            }
            SelectViewEvent(e.Source);
            RemoveUnconnectedLines();
        }

        private void MainCanvasLeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            SelectViewEvent(e.Source);
        }

        // ===========================================================================================================================
        // =================================== РАБОТА С ВЕРХНИМ МЕНЮ =================================================================
        // ===========================================================================================================================

        private void MenuItem_openFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileBeforeClosing(null, null);
            selFile.OpenFile();
        }
        private void MenuItem_createFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileBeforeClosing(null, null);

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
            SaveFileBeforeClosing(null, null);
            selFile = new FileManagerDLAG(); // НЕ УДАЛЯТЬ, ИНАЧЕ НЕ ОТРАБОТАЕТ UpdateWindowElements
            modelView = null;

        }
        private void MenuItem_addObject_Click(object sender, RoutedEventArgs e)
        {
            modelView.SerializationDFD();

            MainWindow window = new MainWindow(modelView);
            window.ShowDialog();

            if (window.added == false)
                return;

            modelView.DesirializationDFD();

            // TODO добавить визуальное отображение
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
        public EventHandler SaveFileBeforeClosing(object sender, EventArgs e)
        {
            if (selFile.file != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Сохранить изменения перед закрытием?", "Внимание", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_saveFile_Click(null, null);
                        return null;
                    case MessageBoxResult.No:
                        return null;
                    case MessageBoxResult.Cancel:
                        return null;
                    default:
                        return null;
                }
            }
            else
                return null;
        }
        public void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (selFile.file != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Сохранить изменения перед закрытием?", "Внимание", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_saveFile_Click(null, null);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
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
            startReqiredBindingDialogComponentView = null;
            endReqiredBindingDialogComponentView = null;
            linesCollection.Clear();
            MainCanvas.Children.Remove(currentLine);
            currentLine = null;
        }
    }
}
