using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Xml;
using HtmlAgilityPack;

namespace DialogsCreator
{
    
    public partial class WebWindow : Window
    {
        public WebWindow()
        {
            InitializeComponent();

            string str = GetEmailFromSite("https://tmp-mail.ru/");
        }

        static string GetEmailFromSite(string url)
        {
            string email = "";
            using (WebClient client = new WebClient())
            {
                // Загрузка HTML-кода страницы в строку
                string html = client.DownloadString("https://tmp-mail.ru/");

                // Загрузка HTML-документа в HtmlDocument
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Извлечение необходимых данных из div
                string data = htmlDoc.DocumentNode.SelectSingleNode("//div[@data-v-bc16390c]").InnerHtml;
            }

            return email;
        }
    }
}
