using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFWeb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Button btn1;
        // Relaxo.SimpleWebview2 webview2;
        public MainWindow()
        {
            Relaxo.SimpleWebview2.WWWFolder = "www";
            InitializeComponent();
            btn1.Click += (s, e) =>
            {
                Relaxo.SimpleWebview2.SetLocalSource(webview2, $"<h1>Hell World {DateTime.Now} </h1>");
            };
        }
    }
}
