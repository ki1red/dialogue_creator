using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;
using DialogsCreator.Views;
using static System.Net.WebRequestMethods;
using Path = System.Windows.Shapes.Path;
using Point = System.Windows.Point;

namespace DialogsCreator
{
    public class TestListBoxItem
    {
        public string Name {get; set;}
        public TestListBoxItem(string name)
        {
            Name = name;
        }
    }
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

        private FileManager manager = new FileManager();
        private WPFtoDFD modelView;

        // ===========================================================================================================================
        // ======================== ПЕРЕМЕННЫЕ ДЛЯ ВЗАИМОДЕЙСТВИЯ МЕЖДУ CANVAS И ОСТАЛЬНЫМ ===========================================
        // ===========================================================================================================================
        //!!
        public ObservableCollection<DialogComponentView> elements = null;
        public ObservableCollection<TestListBoxItem> ItemsTextBox { get; set; }
        //!!
        private Point lastClick = new Point();
        private SelectionObject selectionObject = new SelectionObject();
        private InfoPanel infoPanel = new InfoPanel(); // TODO сделать блять наконец

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
        private delegate void SelectedViewtHandler(object obj); // TODO private ?
        private event SelectedViewtHandler SelectViewEvent; // TODO private ?

        public VisualBindings()
        {
            InitializeComponent();
            InitializeComponentsDFD();
            InitializeSubscribedBaseComponentsWindow();
            InitializeBaseComponentsWindow();
            InitializeComponentsTopMenu();
            InitializeSubscribedClickForMenu();
            InitializeSubscribedMouseForCanvas();

            SelectViewEvent += selectionObject.Select;
            // ItemsTextBox = new ObservableCollection<TestListBoxItem>();
            elements = new ObservableCollection<DialogComponentView>();
            ListBoxView.ItemsSource = elements;
        }

        // ===========================================================================================================================
        // ================================ ИНИЦИАЛИЗАЦИИ ГРУПП КОМПОНЕНТОВ ФОРМЫ ====================================================
        // ===========================================================================================================================

        private void InitializeSubscribedBaseComponentsWindow()
        {
            //this.Closed += SaveAndClose(null, null); TODO нахуй пошёл
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
            this.MenuItem_createFile.Click += MenuItem_createFile_Click;
            this.MenuItem_openFile.Click += MenuItem_openFile_Click;
            this.MenuItem_saveFile.Click += MenuItem_saveFile_Click;
            this.MenuItem_saveAsFile.Click += MenuItem_saveAsFile_Click;
            this.MenuItem_closeFile.Click += MenuItem_closeFile_Click;

            this.MenuItem_createFile.Click += UpdateWindowElements;
            this.MenuItem_openFile.Click += UpdateWindowElements;
            this.MenuItem_saveFile.Click += UpdateWindowElements;
            this.MenuItem_saveAsFile.Click += UpdateWindowElements;
            this.MenuItem_closeFile.Click += UpdateWindowElements;

            this.MenuItem_addObject.Click += MenuItem_addObject_Click;
            this.MenuItem_deleteObject.Click += MenuItem_deleteObject_Click;
        }
        private void InitializeSubscribedMouseForCanvas()
        {
            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseDown;
            MainCanvas.MouseLeftButtonUp += MainCanvasLeftMouseUp;
            MainCanvas.MouseRightButtonUp += MainCanvas_MouseUp;
            MainCanvas.MouseMove += MainCanvas_MouseMove;

            MainCanvas.MouseLeftButtonUp += CheckSelectObject;
        }
        private void InitializeComponentsDFD()
        {
            modelView = new WPFtoDFD(manager);
        }

        // ===========================================================================================================================
        // ======================================== РАБОТА С MAINCANVAS ==============================================================
        // ===========================================================================================================================

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
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

            lastClick = new Point(e.GetPosition(MainCanvas).X, e.GetPosition(MainCanvas).Y);
        }

        internal void MainCanvas_MouseMove(object sender, MouseEventArgs e)
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
            if (SaveAndClose())
            {
                if (!manager.OpenFile())
                    return;
                // TODO нужна переочистка канваса
            }
        }
        private void MenuItem_createFile_Click(object sender, RoutedEventArgs e)
        {
            if (SaveAndClose())
            {
                if (!manager.CreateFile())
                    return;

                SelectLanguageWindow window = new SelectLanguageWindow(); // требование указать язык файла
                do
                {
                    window.ShowDialog();
                    manager.language = window.language;
                } while (manager.language == DialogsCreator.Language.none);
                // TODO нужна переочистка канваса
            }
        }
        private void MenuItem_saveFile_Click(object sender, RoutedEventArgs e)
        {
            modelView.SerializationDFD();
        }
        private void MenuItem_saveAsFile_Click(object sender, RoutedEventArgs e)
        {
            modelView.SerializationDFD(manager.path);
        }
        private void MenuItem_closeFile_Click(object sender, RoutedEventArgs e)
        {
            // TODO Сделать очистку MainCanvas
            SaveAndClose();
            

        }
        private void MenuItem_addObject_Click(object sender, RoutedEventArgs e)
        {
            //modelView.SerializationDFD();

            MainWindow window = new MainWindow();
            window.ShowDialog();

            if (window.added == false)
                return;

            //modelView.DesirializationDFD();
            modelView.AddEmptyElement(window.element);
            modelView.ReplaceCoords(ref modelView.dialog.elements[modelView.dialog.elements.Length - 1], lastClick);

            // TODO добавить визуальное отображение
            AddObjectToView(window.element);
        }
        private void MenuItem_deleteObject_Click(object sender, RoutedEventArgs e)
        {
            DialogComponentView element;// TODO обращение к объекту SelectionObject и взятие из него выбранного ElementDFD
            if (selectionObject.selected == TypeObject.element)
                element = selectionObject.element;
            else
                return;

            modelView.dialog.Delete(modelView.dialog.Search((element.Source as SayingElementViewDFD).idElement));
            // TODO сюда поместить View Delete
            element.Destroy();
            //this.MenuItem_deleteObject.IsEnabled = false;
        }

        // ===========================================================================================================================
        // =================================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ================================================================
        // ===========================================================================================================================

        private void UpdateWindowElements(object sender, EventArgs e)
        {
            if (manager.isOpen)
            {
                this.MenuItem_saveFile.IsEnabled = true;
                this.MenuItem_saveAsFile.IsEnabled = true;
                this.MenuItem_closeFile.IsEnabled = true;

                this.MenuItem_objectSettings.IsEnabled = true;
                this.MenuItem_addObject.IsEnabled = false;
                this.MenuItem_deleteObject.IsEnabled = false;
                this.MenuItem_editObject.IsEnabled = false;

                if ((sender as MenuItem).Name == MenuItem_createFile.Name || (sender as MenuItem).Name == MenuItem_openFile.Name)
                {
                    //modelView = new WPFtoDFD(manager); // TODO точно нужно?
                    modelView.DesirializationDFD();

                    InitializingDialogComponentsView();

                }

                //manager.language = manager.ToLanguage(modelView.dialog.language); // TODO точно нужно?
                this.Title = $"{windowTitle} Открыт файл {manager.file}.{FileManager.type}";
                this.Label_informationOfLanguage.Content = $"{languageTitle}: {manager.language}";
            }
            else
            {
                this.Title = windowTitle;
                this.Label_informationOfLanguage.Content = null;

                this.MenuItem_saveAsFile.IsEnabled = false;
                this.MenuItem_saveFile.IsEnabled = false;
                this.MenuItem_closeFile.IsEnabled = false;

                this.MenuItem_objectSettings.IsEnabled = false;
                this.MenuItem_addObject.IsEnabled = false;
                this.MenuItem_deleteObject.IsEnabled = false;
                this.MenuItem_editObject.IsEnabled = false;
            }
        }
        public bool SaveAndClose()
        {
            if (manager.isOpen && !manager.isSave)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Сохранить изменения перед закрытием?", "Внимание", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_saveFile_Click(null, null); // TODO а можно другой метод ?
                        manager.CloseFile();
                        return true;
                    case MessageBoxResult.No:
                        manager.CloseFile();
                        return true;
                    case MessageBoxResult.Cancel:
                        return false;
                    default:
                        return false; // TODO это при крестике? тогда false
                }
            }
            else
            {
                manager.CloseFile();
                return true;
            }
        }
        public void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (manager.isOpen && !manager.isSave)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Сохранить изменения перед закрытием?", "Внимание", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_saveFile_Click(null, null);
                        manager.CloseFile();
                        break;
                    case MessageBoxResult.No:
                        manager.CloseFile();
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true; // не закрываем окно
                        break;
                    default:
                        e.Cancel = true; // не закрываем окно
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
        private void InitializingDialogComponentsView()
        {
            if (!manager.isOpen || modelView.id == -1) // TODO удалено || modelView == null ||
                throw new Exception("Не удалось отрисовать View при запуске файла");

            // TODO удаление старых компонентов канваса
            if (elements != null)
            {

                var objInScene = elements.ToList();
                foreach (var obj in objInScene)
                {
                    obj.Destroy();
                    elements.Remove(obj);
                }
            }

            for (int i = 0; i < modelView.id; i++)
            {
                ElementDFD el = modelView.dialog.elements[i];

                elements.Add(new DialogComponentView(MainCanvas));
                MainCanvas.Children.Add(elements[i]);
                Canvas.SetLeft(elements[i], el.point.X);
                Canvas.SetTop(elements[i], el.point.Y);
                elements[i].ShowBindigsDialogComponentsView();

                elements[i].Source = new SayingElementViewDFD(el.idElement, el.question); // инициализация вопроса
                elements[i].SetName();

                //ItemsTextBox.Add(new TestListBoxItem(el.question.text));

                foreach (var answer in el.answers) // инициализация ответов
                {
                    elements[i].AddOption(new SayingElementViewDFD(el.idElement, answer));
                    elements[i].Options[elements[i].Options.Count - 1].ShowBindigsDialogComponentsView();
                    elements[i].Options[elements[i].Options.Count - 1].SetName();
                }
            }
        }
        private void AddObjectToView(ElementDFD element)
        {
            elements.Add(new DialogComponentView(MainCanvas));

            int pos = elements.Count - 1;

            MainCanvas.Children.Add(elements[pos]);
            Canvas.SetLeft(elements[pos], element.point.X);
            Canvas.SetTop(elements[pos], element.point.Y);
            elements[pos].ShowBindigsDialogComponentsView();

            elements[pos].Source = new SayingElementViewDFD(element.idElement, element.question); // инициализация вопроса
            elements[pos].SetName();

            foreach (var answer in element.answers) // инициализация ответов
            {
                elements[pos].AddOption(new SayingElementViewDFD(element.idElement, answer));
                elements[pos].Options[elements[pos].Options.Count - 1].ShowBindigsDialogComponentsView();
                elements[pos].Options[elements[pos].Options.Count - 1].SetName();
            }
        }
        private void CheckSelectObject(object sender, MouseButtonEventArgs e) // TODO убедиться в корректности работы
        {
            if (selectionObject.selected == TypeObject.element)
            {
                this.MenuItem_addObject.IsEnabled = false;
                this.MenuItem_deleteObject.IsEnabled = true;
            }
            else
            {
                this.MenuItem_addObject.IsEnabled = true;
                this.MenuItem_deleteObject.IsEnabled = false;
            }
        }
    }
}
