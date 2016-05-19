using SkyView.Nodes;
using System.Windows;
using System.Windows.Controls;

namespace SkyView {
    /// <summary>
    /// Logique d'interaction pour Link.xaml
    /// </summary>
    public partial class Link : UserControl {
        public Link() {
            InitializeComponent();
            Line.DataContext = this;
        }

        #region LinkData Property
        public LogicalLink LinkData {
            get { return (LogicalLink)GetValue(LinkDataProperty); }
            set { SetValue(LinkDataProperty, value); }
        }
        public static readonly DependencyProperty LinkDataProperty = DependencyProperty.Register(
            "LinkData",
            typeof(LogicalLink),
            typeof(Link),
            new PropertyMetadata(new LogicalLink()));
        #endregion LinkData Property
    }
}
