using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;
using System.Windows.Threading;

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
        }

        private async void InitializeAsync()
        {
            await this.EnsureCoreWebView2Async();
            // await this.EnsureCoreWebView2Async()
            // .ContinueWith(t =>
            // {
            // Dispatcher.CurrentDispatcher.Invoke(() =>
            // {
            this.CoreWebView2.SetVirtualHostNameToFolderMapping(VirtualAddress, WWWFolder, Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            Trace.WriteLine(BaseAddress + GetLocalSource(this).Replace("./", ""));
            Source = new Uri(BaseAddress + GetLocalSource(this).Replace("./", ""));
            IsReady = true;
            // });
            // });

        }
    }
}