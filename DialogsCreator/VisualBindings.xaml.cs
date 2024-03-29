﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        private bool isEdit = false;

        // ===========================================================================================================================
        // ======================== ПЕРЕМЕННЫЕ ДЛЯ ВЗАИМОДЕЙСТВИЯ МЕЖДУ CANVAS И ОСТАЛЬНЫМ ===========================================
        // ===========================================================================================================================

        public ObservableCollection<DialogComponentView> elements;
        private Point lastClick = new Point();
        private SelectionObject selectionObject;

        // ===========================================================================================================================
        // ================================ ПЕРЕМЕННЫЕ ИЛЬИ ХЗ ДЛЯ ЧЕГО ==============================================================
        // ===========================================================================================================================

        private BindingDialogComponentView startBindingDialogComponentView;
        private BindingDialogComponentView endBindingDialogComponentView;
        private Line currentLine;
        private List<Line> linesCollection = new List<Line>();

        // ===========================================================================================================================
        // ================================ КОНСТРУКТОРЫ ФОРМЫ VISUAL BINDINGS =======================================================
        // ===========================================================================================================================
        private delegate void SelectedViewtHandler(object obj);
        private event SelectedViewtHandler SelectViewEvent;
        private object selectedView = null;

        public List<LinkDataDialogPackage> LinkDataDialogPackages = new List<LinkDataDialogPackage>();

        bool isPanning; // для передвижения по Scroll
        internal VisualBindings()
        {
            InitializeComponent();
            InitializeComponentsDFD();
            InitializeSubscribedBaseComponentsWindow();
            InitializeBaseComponentsWindow();
            InitializeComponentsTopMenu();
            InitializeSubscribedClickForMenu();
            InitializeSubscribedMouseForCanvas();

            elements = new ObservableCollection<DialogComponentView>();
            ListBoxView.ItemsSource = elements;
        }

        // ===========================================================================================================================
        // ================================ ИНИЦИАЛИЗАЦИИ ГРУПП КОМПОНЕНТОВ ФОРМЫ ====================================================
        // ===========================================================================================================================

        internal void InitializeSubscribedBaseComponentsWindow()
        {
            this.Closing += Close;
        }
        internal void InitializeBaseComponentsWindow()
        {
            this.Title = windowTitle;
            this.Label_informationOfLanguage.Content = null;

            this.ScrollViewer.IsEnabled = false;
            this.ListBoxView.Visibility = Visibility.Hidden;
            this.ListBox_info.Visibility = Visibility.Hidden;
        }
        internal void InitializeComponentsTopMenu()
        {
            this.MenuItem_saveFile.IsEnabled = false;
            this.MenuItem_saveAsFile.IsEnabled = false;
            this.MenuItem_exportFile.IsEnabled = false; // TODO когда добавим преобразование из dlag в dfd, сделать по умолчанию доступным 
            this.MenuItem_importFile.IsEnabled = false;
            this.MenuItem_closeFile.IsEnabled = false;
            this.MenuItem_objectSettings.IsEnabled = false;
            this.MenuItem_addObject.IsEnabled = false;
            this.MenuItem_cloneObject.IsEnabled = false;
            this.MenuItem_editObject.IsEnabled = false;
            this.MenuItem_deleteObject.IsEnabled = false;
        }
        internal void InitializeSubscribedClickForMenu()
        {
            this.MenuItem_createFile.Click += MenuItem_createFile_Click;
            this.MenuItem_openFile.Click += MenuItem_openFile_Click;
            this.MenuItem_saveFile.Click += MenuItem_saveFile_Click;
            this.MenuItem_saveAsFile.Click += MenuItem_saveAsFile_Click;
            this.MenuItem_closeFile.Click += MenuItem_closeFile_Click;
            this.MenuItem_exportFile.Click += MenuItem_exportFile_Click;

            this.MenuItem_createFile.Click += UpdateWindowElements;
            this.MenuItem_openFile.Click += UpdateWindowElements;
            this.MenuItem_saveFile.Click += UpdateWindowElements;
            this.MenuItem_saveAsFile.Click += UpdateWindowElements;
            this.MenuItem_closeFile.Click += UpdateWindowElements;

            this.MenuItem_addObject.Click += MenuItem_addObject_Click;
            this.MenuItem_cloneObject.Click += MenuItem_cloneObject_Click;
            this.MenuItem_deleteObject.Click += MenuItem_deleteObject_Click;
            this.MenuItem_editObject.Click += MenuItem_editObject_Click;
        }

        internal void InitializeSubscribedMouseForCanvas()
        {
            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseDown;
            MainCanvas.MouseLeftButtonUp += MainCanvasLeftMouseUp;
            MainCanvas.MouseRightButtonUp += MainCanvas_MouseUp;
            MainCanvas.MouseMove += MainCanvas_MouseMove;

            MainCanvas.MouseLeftButtonUp += CheckSelectObject;
        }
        internal void InitializeSubsribedClickForScrollViewer()
        {
            //TODO перенести подписки из xaml
        }
        internal void InitializeComponentsDFD()
        {
            modelView = new WPFtoDFD(manager);
        }

        // ===========================================================================================================================
        // ======================================== РАБОТА СО SCROLLVIEWER ===========================================================
        // ===========================================================================================================================

        internal void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectionObject.selected != TypeObject.none)
                return;

            lastClick = e.GetPosition(MainCanvas);
            isPanning = false;
            ScrollViewer.CaptureMouse();
        }
        internal void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (selectionObject.selected != TypeObject.none)
                return;

            if (!isPanning && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(MainCanvas);
                if (Math.Abs(currentPoint.X - lastClick.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(currentPoint.Y - lastClick.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    isPanning = true;
                }
            }

            if (isPanning)
            {
                Point currentPoint = e.GetPosition(MainCanvas);
                double deltaX = currentPoint.X - lastClick.X;
                double deltaY = currentPoint.Y - lastClick.Y;
                ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - deltaX);
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - deltaY);
            }
        }
        internal void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectionObject.selected != TypeObject.none)
                return;

            isPanning = false;
            ScrollViewer.ReleaseMouseCapture();
        }

        // ===========================================================================================================================
        // ======================================== РАБОТА С MAINCANVAS ==============================================================
        // ===========================================================================================================================

        internal void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
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

                    currentLine.StrokeThickness = 2;
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
                        try{ 
                            LinkDataDialogPackages.Add(startBindingDialogComponentView.LinkWith(endBindingDialogComponentView, linesCollection)); 
                        }
                        catch(Exception exp)
                        {
                            MessageBox.Show($"{exp.Message}","Error!");
                            foreach(Line line in linesCollection) 
                            {
                                MainCanvas.Children.Remove(line);
                            }
                        }
                        linesCollection.Clear();
                        currentLine = null;
                        startBindingDialogComponentView = null;
                        endBindingDialogComponentView = null;
                    }
                }
            }
 
            else if ((startBindingDialogComponentView != null) && currentLine != null)
            {
                var x = currentLine.X2;
                var y = currentLine.Y2;

                linesCollection.Add(currentLine);

                currentLine = new Line();
                currentLine.Stroke = new SolidColorBrush(Colors.Black);
                currentLine.StrokeThickness = 2;
                currentLine.StrokeStartLineCap = PenLineCap.Round;
                currentLine.StrokeEndLineCap = PenLineCap.Round;
                MainCanvas.Children.Add(currentLine);

                currentLine.X1 = x;
                currentLine.Y1 = y;
                currentLine.X2 = x;
                currentLine.Y2 = y;
            }

            
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
        internal void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.Source is DialogComponentView && currentLine == null)
            {
                var component = e.Source as DialogComponentView;
                var windowInfo = new BindsEditDialogComponentWindow(component,LinkDataDialogPackages);
                windowInfo.ShowDialog();

            }

            RemoveUnconnectedLines();
        }
        internal void MainCanvasLeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source != selectedView)
            {
                if (selectedView is DialogComponentView)
                {
                    var dilogCopmponent = selectedView as DialogComponentView;
                    dilogCopmponent?.UnSelect();
                }

                if (e.Source is DialogComponentView)
                {
                    var dilogCopmponent = e.Source as DialogComponentView;
                    dilogCopmponent?.Select();
                }

                lastClick = new Point(e.GetPosition(MainCanvas).X, e.GetPosition(MainCanvas).Y);
                selectedView = e.Source;
                SelectViewEvent(e.Source);
            }
        }
        internal void ListBoxView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            var hit = listBox.InputHitTest(e.GetPosition(listBox)) as FrameworkElement;
            var item = hit.DataContext as DialogComponentView;

            if (item != null)
            {
                ScrollViewer.ScrollToHorizontalOffset(Canvas.GetLeft(item) - 150);
                ScrollViewer.ScrollToVerticalOffset(Canvas.GetTop(item) - 150);
            }
        }

        // ===========================================================================================================================
        // =================================== РАБОТА С ВЕРХНИМ МЕНЮ =================================================================
        // ===========================================================================================================================

        internal void MenuItem_openFile_Click(object sender, RoutedEventArgs e)
        {
            if (SaveAndClose())
            {
                if (!manager.OpenFile())
                    return;
                selectionObject = new SelectionObject(modelView.dialog, ref ListBox_info, ref ListBoxView);
                SelectViewEvent += selectionObject.Select;

                SetStartPositionInScrollViewer();
            }
        }
        internal void MenuItem_createFile_Click(object sender, RoutedEventArgs e)
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

                selectionObject = new SelectionObject(modelView.dialog, ref ListBox_info, ref ListBoxView);
                SelectViewEvent += selectionObject.Select;

                SetStartPositionInScrollViewer();
            }
        }
        internal void MenuItem_saveFile_Click(object sender, RoutedEventArgs e)
        {
            UpdatePointsViews();
            modelView.SerializationDFD();
            isEdit = false;
        }
        internal void MenuItem_saveAsFile_Click(object sender, RoutedEventArgs e)
        {
            UpdatePointsViews();
            modelView.SerializationDFD(manager.path);
            isEdit = false;
        }
        internal void MenuItem_closeFile_Click(object sender, RoutedEventArgs e)
        {
            SaveAndClose();
        }
        internal void MenuItem_exportFile_Click(object sender, RoutedEventArgs e)
        {
            ExportWindow window = new ExportWindow(manager.path, manager.file, "dfd", modelView.dialog);
            window.ShowDialog();
        }
        internal void MenuItem_addObject_Click(object sender, RoutedEventArgs e)
        {

            MainWindow window = new MainWindow();
            window.ShowDialog();

            if (window.added == false)
                return;

            modelView.AddEmptyElement(window.element);
            modelView.ReplaceCoords(ref modelView.dialog.elements[modelView.dialog.elements.Length - 1], lastClick);

            AddObjectToView(window.element);

            isEdit = true;
            this.MenuItem_addObject.IsEnabled = false;
        }
        private void MenuItem_cloneObject_Click(object sender, RoutedEventArgs e)
        {
            int id = (selectionObject.element.Source as SayingElementViewDFD).idElement;
            ElementDFD element = modelView.dialog.Search(id);

            SayingElementDFD question = new SayingElementDFD(0, element.question.text, TypeSayingElementDFD.question, new SayingElementDFD(), new SayingElementDFD[0]);
            SayingElementDFD[] answers = new SayingElementDFD[element.answers.Length];
            int i = 0;
            foreach (var a in element.answers)
            {
                answers[i] = new SayingElementDFD(0, a.text, TypeSayingElementDFD.answer, new SayingElementDFD(), new SayingElementDFD[0]);
                    i++;
            }

            ElementDFD clone = new ElementDFD(0, element.pathToSound, element.pathToImage, element.author, question, answers, new Point(0,0));
            modelView.AddEmptyElement(clone);
            modelView.ReplaceCoords(ref modelView.dialog.elements[modelView.dialog.elements.Length - 1], lastClick);
            AddObjectToView(clone);

            isEdit = true;
            this.MenuItem_cloneObject.IsEnabled = false;
        }
        internal void MenuItem_deleteObject_Click(object sender, RoutedEventArgs e)
        {
            DialogComponentView element;
            if (selectionObject.selected == TypeObject.element)
                element = selectionObject.element;
            else
                return;
           
            foreach(var link in element.Destroy())
            {
                LinkDataDialogPackages.Remove(link);
            }
            
            modelView.DeleteId((element.Source as SayingElementViewDFD).idElement);


            if (element != null)
                elements.Remove(element);
            modelView.UpdateLinkeds(ref elements);

            selectionObject.Select(null);

            isEdit = true;
            this.MenuItem_deleteObject.IsEnabled = false;
        }
        internal void MenuItem_editObject_Click(object sender, RoutedEventArgs e)
        {
            if (selectionObject.selected != TypeObject.element)
                return;

            DialogComponentView delement = selectionObject.element;
                

            int id = (delement.Source as SayingElementViewDFD).idElement;
            ref ElementDFD element = ref modelView.dialog.Search(id);

            EditWindow window = new EditWindow(element);
            window.ShowDialog();

            if (!window.isEdit)
                return;

            delement.UpdateNameDialog();

            selectionObject.Select(delement);

            isEdit = true;
            this.MenuItem_editObject.IsEnabled = false;
        }

        // ===========================================================================================================================
        // =================================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ================================================================
        // ===========================================================================================================================

        internal void UpdateWindowElements(object sender, EventArgs e)
        {
            if (manager.isOpen)
            {
                this.MenuItem_saveFile.IsEnabled = true;
                this.MenuItem_saveAsFile.IsEnabled = true;
                this.MenuItem_closeFile.IsEnabled = true;

                this.MenuItem_objectSettings.IsEnabled = true;
                this.MenuItem_addObject.IsEnabled = false;
                this.MenuItem_cloneObject.IsEnabled = false;
                this.MenuItem_deleteObject.IsEnabled = false;
                this.MenuItem_editObject.IsEnabled = false;

                if ((sender as MenuItem).Name == MenuItem_createFile.Name || (sender as MenuItem).Name == MenuItem_openFile.Name)
                {
                    modelView.DesirializationDFD();
                    InitializingDialogComponentsView();

                }

                if (modelView.dialog.elements.Length > 0)
                    this.MenuItem_exportFile.IsEnabled = true;

                this.Title = $"{windowTitle} Открыт файл {manager.file}.{FileManager.type}";
                this.Label_informationOfLanguage.Content = $"{languageTitle}: {manager.language}";
                this.ScrollViewer.IsEnabled = true;
            }
            else
            {
                this.Title = windowTitle;
                this.Label_informationOfLanguage.Content = null;

                this.MenuItem_saveAsFile.IsEnabled = false;
                this.MenuItem_saveFile.IsEnabled = false;
                this.MenuItem_closeFile.IsEnabled = false;
                this.MenuItem_exportFile.IsEnabled = false;

                this.MenuItem_objectSettings.IsEnabled = false;
                this.MenuItem_addObject.IsEnabled = false;
                this.MenuItem_cloneObject.IsEnabled = false;
                this.MenuItem_deleteObject.IsEnabled = false;
                this.MenuItem_editObject.IsEnabled = false;

                this.ScrollViewer.IsEnabled = false;
                this.ListBoxView.Visibility = Visibility.Hidden;
                this.ListBox_info.Visibility = Visibility.Hidden;
            }
        }
        internal bool SaveAndClose()
        {
            if (CheckedMoved())
                isEdit = true;

            if ((manager.isOpen && !manager.isSave) || isEdit)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Сохранить изменения перед закрытием?", "Внимание", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        UpdatePointsViews();
                        modelView.SerializationDFD();
                        isEdit = false;
                        manager.CloseFile();
                        ClearCanvas();
                        return true;
                    case MessageBoxResult.No:
                        ClearCanvas();
                        isEdit = false;
                        manager.CloseFile();
                        return true;
                    case MessageBoxResult.Cancel:
                        return false;
                    default:
                        return false;
                }
            }
            else
            {
                manager.CloseFile();
                ClearCanvas();
                isEdit = false;
                return true;
            }
        }
        internal void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CheckedMoved())
                isEdit = true;

            if ((manager.isOpen && !manager.isSave) || isEdit)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Сохранить изменения перед закрытием?", "Внимание", MessageBoxButton.YesNoCancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        UpdatePointsViews();
                        modelView.SerializationDFD();
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
        internal bool CanLink()
        {
            // Проверки для определения, можно ли соединить startBindingDialogComponentView и endBindingDialogComponentView
            return true;
        }
        internal void RemoveUnconnectedLines()
        {
            foreach (var line in linesCollection)
            {
                MainCanvas.Children.Remove(line);
            }
            startBindingDialogComponentView = null;
            endBindingDialogComponentView = null;
            //startReqiredBindingDialogComponentView = null;
            //endReqiredBindingDialogComponentView = null; 
            linesCollection.Clear();
            MainCanvas.Children.Remove(currentLine);
            currentLine = null;
        }
        internal void InitializingDialogComponentsView()
        {
            if (!manager.isOpen || modelView.id == -1)
                throw new Exception("Не удалось отрисовать View при запуске файла");

            ClearCanvas();

            for (int i = 0; i < modelView.id; i++)
            {
                ElementDFD el = modelView.dialog.elements[i];
                var elV = new DialogComponentView(MainCanvas, el.idElement);

                elements.Add(elV);
                MainCanvas.Children.Add(elV);
                Canvas.SetLeft(elV, el.point.X);
                Canvas.SetTop(elV, el.point.Y);
                elV.UpdateLayout();
                elV.ShowBindigsDialogComponentsView();
                elV.Source = new SayingElementViewDFD(el.idElement, el.question);
                elV.SetName();
        
                foreach (var answer in el.answers) // инициализация ответов
                {
                    elV.AddOption(new SayingElementViewDFD(el.idElement, answer));
                    elV.Options[elV.Options.Count - 1].SetName();
                    elV.Options[elV.Options.Count - 1].UpdateLayout();
                    elV.Options[elV.Options.Count - 1].ShowBindigsDialogComponentsView();

                }
            }



            foreach (var link in modelView.dialog.linkeds)
            {

                List<Line> lines = new List<Line>();

                BindingDialogComponentView outViewBinding;
                BindingDialogComponentView inViewBinding ;

                var outDialog = elements.FirstOrDefault(dialog => dialog.Id == link.OutIdDialogView) ?? throw new NullReferenceException("Dialog not found in Scene");
                var inDialog = elements.FirstOrDefault(dialog => dialog.Id == link.InIdDialogView) ?? throw new NullReferenceException("Dialog not found in Scene");

                if (link.OutIdOptionView != -1)
                    outViewBinding = (
                            outDialog.Options.FirstOrDefault(option => option.Id == link.OutIdOptionView) ??
                            throw new NullReferenceException("Option not found in Scene")
                        )
                        .bindingDialogComponentViews
                        .FirstOrDefault(bindingView => bindingView.Id == link.OutIdBindingView) 
                        ?? throw new NullReferenceException("Binding View not found in Scene");

                else
                    outViewBinding = outDialog
                        .bindingDialogComponentViews
                        .FirstOrDefault(bindingView => bindingView.Id == link.OutIdBindingView) 
                        ?? throw new NullReferenceException("Binding View not found in Scene");
                
                if(link.InIdOptionView != -1)
                    inViewBinding = (
                        inDialog.Options.FirstOrDefault(option => option.Id == link.InIdOptionView) ??
                        throw new NullReferenceException("Option not found in Scene")
                    )
                    .bindingDialogComponentViews
                    .FirstOrDefault(bindingView => bindingView.Id == link.InIdBindingView) 
                    ?? throw new NullReferenceException("Option not found in Scene");
                else inViewBinding = inDialog
                        .bindingDialogComponentViews
                        .FirstOrDefault(bindingView => bindingView.Id == link.InIdBindingView) 
                        ?? throw new NullReferenceException("Binding View not found in Scene");

                foreach (Vector4 vector in link.LinesCoords)
                {
                    Line line = new Line();

                    lines.Add(line);
                    MainCanvas.Children.Add(line);
                    
                    line.Stroke = new SolidColorBrush(Colors.Black);
                    line.StrokeThickness = 2;
                    line.StrokeStartLineCap = PenLineCap.Round;
                    line.StrokeEndLineCap = PenLineCap.Round;



                    line.X1 = vector.X1;
                    line.Y1 = vector.Y1;
                    line.X2 = vector.X2;
                    line.Y2 = vector.Y2;

                    line.UpdateLayout();
                }

                LinkDataDialogPackages.Add(outViewBinding.LinkWith(inViewBinding,lines));
            }
        }
        internal void AddObjectToView(ElementDFD element)
        {
            elements.Add(new DialogComponentView(MainCanvas, element.idElement));

            int pos = elements.Count - 1;

            MainCanvas.Children.Add(elements[pos]);
            Canvas.SetLeft(elements[pos], element.point.X);
            Canvas.SetTop(elements[pos], element.point.Y);
            elements[pos].UpdateLayout();
            elements[pos].ShowBindigsDialogComponentsView();
            elements[pos].Source = new SayingElementViewDFD(element.idElement, element.question); // инициализация вопроса
            elements[pos].SetName();
       
            foreach (var answer in element.answers) // инициализация ответов
            {
                elements[pos].AddOption(new SayingElementViewDFD(element.idElement, answer));
                elements[pos].Options[elements[pos].Options.Count - 1].SetName();
                elements[pos].Options[elements[pos].Options.Count - 1].UpdateLayout();
                elements[pos].Options[elements[pos].Options.Count - 1].ShowBindigsDialogComponentsView();
                
            }
        }
        internal void CheckSelectObject(object sender, MouseButtonEventArgs e)
        {
            if (selectionObject.selected == TypeObject.element)
            {
                this.MenuItem_addObject.IsEnabled = false;
                this.MenuItem_cloneObject.IsEnabled = true;
                this.MenuItem_deleteObject.IsEnabled = true;
                this.MenuItem_editObject.IsEnabled = true;
            }
            else
            {
                this.MenuItem_addObject.IsEnabled = true;
                this.MenuItem_cloneObject.IsEnabled = false;
                this.MenuItem_deleteObject.IsEnabled = false;
                this.MenuItem_editObject.IsEnabled = false;
            }
        }
        private void ClearCanvas()
        {
            if (elements != null && elements.Count > 0)
            {
                var objInScene = elements.ToList();
                foreach (var obj in objInScene)
                {
                    foreach (var link in obj.Destroy())
                    {
                        LinkDataDialogPackages.Remove(link);
                    }
                    elements.Remove(obj);
                }
            }

            LinkDataDialogPackages.Clear();
        }
        private void UpdatePointsViews()
        {
            for (int i = 0; i < modelView.dialog.elements.Length; i++)
            {
                ref ElementDFD element = ref modelView.dialog.elements[i];
                double y = Canvas.GetTop(elements[i]);
                double x = Canvas.GetLeft(elements[i]);
                Point point = new Point(x, y);
                modelView.ReplaceCoords(ref element, point);
                elements[i].isMove = false;
            }

            // TODO сбор данных об LinkViews
            modelView.dialog.linkeds = LinkDataDialogPackages.Select(linesCollection => linesCollection.toLinkDataDialogPackageSerialize()).ToArray();

            modelView.dialog.positionCanvas = new Point(ScrollViewer.HorizontalOffset, ScrollViewer.VerticalOffset);
        }
        internal bool CheckedMoved()
        {
            foreach (var view in elements)
            {
                if (view.isMove)
                    return true;
            }
            return false;
        }
        internal void SetStartPositionInScrollViewer()
        {
            Point point = modelView.dialog.positionCanvas;
            if (point.X == 0 && point.Y == 0)
            {
                ScrollViewer.ScrollToHorizontalOffset(MainCanvas.Width / 10);
                ScrollViewer.ScrollToVerticalOffset(MainCanvas.Height / 2);
            }
            else
            {
                ScrollViewer.ScrollToHorizontalOffset(point.X);
                ScrollViewer.ScrollToVerticalOffset(point.Y);
            }
        }
    }
}
