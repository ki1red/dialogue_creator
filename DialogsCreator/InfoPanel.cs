using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DialogsCreator
{
    public class InfoPanel
    {
        private const int width = 600;
        private const int height = 600;
        private const int fontSize = 16;

        private ListBox panel;
        private ElementDFD element;
        public InfoPanel(ref ListBox panel) 
        { 
            this.panel = panel; 
            this.element = null;

            //panel.Background = Brushes.DarkBlue;
        }

        public void Show(ElementDFD element)
        {
            if (element == this.element)
                return;

            this.Close();

            Grid grid = new Grid();
            //panel.Children.Add(grid);

            this.element = element;

            Label label = new Label();
            label.Content = $"Автор: {element.author}";
            label.FontSize = fontSize;
            label.Foreground = Brushes.White;
            label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            grid.Children.Add(label);

            label = new Label();
            label.Content = $"Вопрос: {element.question.text}";
            label.FontSize = fontSize;
            label.Foreground = Brushes.White;
            label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            grid.Children.Add(label);


            if (this.element.pathToImage != null)
            {

                label = new Label();
                label.Content = $"Картинка: {element.pathToImage}";
                label.FontSize = fontSize;
                label.Foreground = Brushes.White;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                grid.Children.Add(label);
            }
            if (this.element.pathToSound != null)
            {
                label = new Label();
                label.Content = $"Звук: {element.pathToSound}";
                label.FontSize = fontSize;
                label.Foreground = Brushes.White;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                grid.Children.Add(label);
            }
            for (int i = 0; i < this.element.answers.Length; i++)
            {
                label = new Label();
                label.Content = $"Ответ {i+1}: {element.answers[i].text}";
                label.FontSize = fontSize;
                label.Foreground = Brushes.White;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                grid.Children.Add(label);

            }

            panel.Width = width;
            panel.Height = height;
            grid.Width = width;
            grid.Height = height;

        }
        public void Close()
        {
            //panel.Children.Clear();
            this.element = null;

            panel.Width = 0;
            panel.Height = 0;
        }
    }
}
