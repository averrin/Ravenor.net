using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;
using Newtonsoft.Json.Linq;

namespace Ravenor
{
    public class Page
    {
        public Page(string path, TextEditor viewer, WebBrowser webviewer) {
            Viewer = viewer;
            WebViewer = webviewer;
            Root = path;
            if (File.Exists(Root + @"\__page.cfg")) {
                try
                {
                    Config = JObject.Parse(File.ReadAllText(Root + @"\__page.cfg", Encoding.UTF8));
                    Title = (string) Config["title"] ?? path.Split('\\').Last();
                }catch(Exception)
                {
                    Config = new JObject();
                    Title = path.Split('\\').Last();
                }
            }
            else
            {
                Config = new JObject();
                Title = path.Split('\\').Last();
            }
            Childs = new List<Page>();
            Item = new TreeViewItem { Header = Title };
            Item.Selected += Click;
            foreach (var dir in Directory.EnumerateDirectories(Root)) {
                var p = new Page(dir, viewer, webviewer);
                Childs.Add(p);
                Item.Items.Add(p.Item);
            }
        }

        public JObject Config { set; get; }

        public List<Page> Childs { set; get; }

        public TreeViewItem Item { set; get; }

        public string Root { set; get; }

        public string Title { set; get; }

        private TextEditor Viewer { set; get; }

        private WebBrowser WebViewer { set; get; }

        private void Click(object sender, RoutedEventArgs e) {
            if (Equals(e.Source, Item))
                e.Handled = true;
            var file = Root + @"\__page.text";
            var text = File.ReadAllText(file, Encoding.UTF8);
            Viewer.Text = text;
            //            var uri = new Uri(file);
            //            WebViewer.Navigate(uri);
            WebViewer.NavigateToString("<meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>" + text);
        }
    }

    public class Tree
    {
        public Tree(string root, TextEditor viewer, WebBrowser webviewer) {
            Root = root;
            Childs = new List<Page>();
            foreach (var dir in Directory.EnumerateDirectories(Root)) {
                var p = new Page(dir, viewer, webviewer);
                if (File.Exists(p.Root + @"\__page.text"))
                    Childs.Add(p);
            }
        }

        public List<Page> Childs { set; get; }

        public string Root { set; get; }
    }
}