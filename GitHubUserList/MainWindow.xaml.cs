using System;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Documents;

namespace GitHubUserList
{
    public partial class MainWindow : Window
    {
        static HttpClient client = new HttpClient();
        public class User
        {
            public string Login { get; set; }
            public string Avatar_url { get; set; }
            //public bool IsLogin { get; set; }
            public bool IsSiteAdmin { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            Run();
        }       

        async void Run(int start=0,int per_page=0)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                    
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                    client.Timeout = TimeSpan.FromSeconds(30);
              
                    string url = "https://api.github.com/users";
                    if (start > 0 || per_page > 0)
                        url += "?";
                    if (start > 0 && per_page > 0)
                        url += "since=" + start + "&" + "per_page=" + per_page;
                    else if (start > 0)
                        url += "since=" + start;
                    else if (per_page > 0)
                        url += "per_page=" + per_page;
                    //Console.WriteLine(url);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();              

                    List<User> userS = JsonConvert.DeserializeObject<List<User>>(responseBody);
                    UpdateUI(userS);           
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        void SelectButton_Click(object sender, RoutedEventArgs e)
        {       
            if (!Int32.TryParse(since.Text, out int sinceVal))
                return;
            if (!Int32.TryParse(perPage.Text, out int perPageVal))
                return;
         
            Run(sinceVal,perPageVal);
        }

        void UpdateUI(List<User> userS)
        {  
            ListBox listBox = new ListBox();
            foreach (var user in userS)
            {
                listBox.Items.Add(CreateRichTextBox(user));         
            } 
            mainGrid.Children.Add(listBox);
        }

        RichTextBox CreateRichTextBox(User user)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(user.Avatar_url, UriKind.Absolute));

            image.Width = 50;

            RichTextBox box = new RichTextBox();
            box.Height = 90;
            box.Width = 500;
            InlineUIContainer container = new InlineUIContainer(image);
            Paragraph paragraph = new Paragraph(container);
            paragraph.Inlines.Add(" Login:" + user.Login + " IsSiteAdmin:" + user.IsSiteAdmin);
            
            box.Document.Blocks.Add(paragraph);

            return box;
        }
        
    }
}
