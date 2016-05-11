using System.Windows;
using System.Windows.Controls;

namespace SkyView.Nodes {
    /// <summary>
    /// Logique d'interaction pour Link.xaml
    /// </summary>
    public partial class Link : UserControl {
        public Link() {
            InitializeComponent();
            DataContext = this;
        }

        public Point startPoint { get; set; }
        public Point finishPoint { get; set; }
    }
}
