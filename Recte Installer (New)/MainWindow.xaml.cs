using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;

namespace Recte_Installer_New
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public static string HttpGet(string url)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(
                    HttpRequestHeader.UserAgent,
                    "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"
                );
                return webClient.DownloadString(url);
            }
        }

        string version = HttpGet("https://recte.xyz/currentVer.txt");

        void ExtractFile(string file, string destination)
        {
            try
            {
                ZipFile.ExtractToDirectory(file, destination);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        WebClient webClient = new WebClient();

        async void DownloadFile(string file, string destination)
        {
            webClient.DownloadFileAsync(new Uri(file), destination);
            while (webClient.IsBusy)
                await Task.Delay(1000);
        }

        int currentprogress = 0;

        async void ChangeProgress(string Text, int Progress)
        {
            LoadingText.Content = Text;

            // Originally made by MCGamin1738 for Olympus
            for (int i = currentprogress; i <= Progress; i++)
            {
                LoadingBar.Value = i;
                currentprogress = Convert.ToInt32(currentprogress);
                await Task.Delay(5);
            }
        }
        //Literally Writted By ChatGPT
        static void DeleteFoldersWithSubstring(string directory, string substring)
        {
            try
            {
                foreach (string folder in Directory.GetDirectories(directory))
                {
                    if (folder.Contains(substring))
                    {
                        Directory.Delete(folder, true);
                        Console.WriteLine($"Deleted folder: {folder}");
                    }
                    else
                    {
                        DeleteFoldersWithSubstring(folder, substring);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        async void Button1_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeProgress("Detecting If Recte Already Exists", 10);
            string currentDirectory = Directory.GetCurrentDirectory();
            DeleteFoldersWithSubstring(currentDirectory, "Recte");
            ChangeProgress("Deleting Directory", 20);
               

            ChangeProgress("Starting Download", 25);
            ChangeProgress("Downloading...", 30);
            webClient.DownloadFileAsync(new Uri("https://recte.xyz/Recte.zip"), "Recte.zip");
            
            while (webClient.IsBusy)
                await Task.Delay(1000);
            
            ChangeProgress("Extracting", 70);
            ExtractFile("Recte.zip", "Recte " + version);
            ChangeProgress("Extracted", 75);
            ChangeProgress("Deleting Used .zip File", 10);
            try
            {
                File.Delete("Recte.zip");
            }
            catch { }
            ChangeProgress("Complete :)", 100);
        }
    }
}
