using System;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace GitHubUserList
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient client = new HttpClient();
        public class User
        {
            public int Id { get; set; }
            public string Avatar_url { get; set; }
            public bool IsLogin { get; set; }
            public bool IsSiteAdmin { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            Run();
        }       

        async void Run()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpResponseMessage response = await client.GetAsync("https://api.github.com/users");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
              
                    tempData.Text = responseBody;
                    Console.WriteLine(responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

    }
}
