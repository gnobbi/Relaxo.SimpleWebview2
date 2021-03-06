using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;

namespace Relaxo
{
    public class SimpleWebview2 : WebView2
    {
        #region static
        public static string WWWFolder = ".";
        public static string VirtualAddress = "local.example";
        public static string BaseAddress => $"https://{VirtualAddress}/";
        #endregion

        #region Xaml Property

        public static string GetLocalSource(DependencyObject obj)
        {
            return (string)obj.GetValue(LocalSourceProperty);
        }

        public static void SetLocalSource(DependencyObject obj, string value)
        {
            obj.SetValue(LocalSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for LocalSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalSourceProperty =
            DependencyProperty.RegisterAttached("LocalSource", typeof(string), typeof(SimpleWebview2), new PropertyMetadata(""));

        #endregion

        public bool IsReady { get; private set; }
        public SimpleWebview2()
        {
            Trace.WriteLine("Ctor");
            InitializeAsync();
            var pdIsAvailable = DependencyPropertyDescriptor.FromProperty(SimpleWebview2.LocalSourceProperty, typeof(SimpleWebview2));
            pdIsAvailable.AddValueChanged(this, this.LocalSourceChangedHandler);

        }

        private void LocalSourceChangedHandler(object? s, EventArgs e)
        {
            if (s is SimpleWebview2 w)
            {
                var html = GetLocalSource(w);
                LocalSourceChanged(html);
            }
        }

        private void LocalSourceChanged(string htmlOrUri)
        {
            if (IsReady)
            {
                if (!htmlOrUri.StartsWith("http") && (htmlOrUri.EndsWith(".html") || htmlOrUri.EndsWith(".htm")))
                {
                    if (File.Exists(htmlOrUri))
                    {
                        // "./../www/index.html" -> "index.html"
                        var relativePath = htmlOrUri.Replace("../", "").Replace("./", "").Replace($"{WWWFolder}/", "");
                        Source = new Uri(BaseAddress + relativePath);
                    }
                    else
                    {
                        throw new FileNotFoundException("File not found Exception", htmlOrUri);
                    }
                }
                else if (htmlOrUri.StartsWith("http"))
                {
                    Source = new Uri(htmlOrUri);
                }
                else
                {
                    this.NavigateToString(htmlOrUri);
                }
            }

        }
        private async void InitializeAsync()
        {
            await this.EnsureCoreWebView2Async();
            if (Directory.Exists(WWWFolder))
                this.CoreWebView2.SetVirtualHostNameToFolderMapping(VirtualAddress, WWWFolder, Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            IsReady = true;
            var html = GetLocalSource(this);
            LocalSourceChanged(html);
        }
    }
}