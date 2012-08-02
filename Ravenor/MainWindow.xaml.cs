using System.Windows;

namespace Ravenor
{
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
            var config = new app();
            var path = config["organizer"].ToString();

            var tree = new Tree(path, TextViewer, WebViewer);

            foreach (var item in tree.Childs) {
                PageTree.Items.Add(item.Item);
            }
        }
    }
}