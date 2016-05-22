using SkyView.Nodes;
using System;
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

        private void Line_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
                MustBeDestructed?.Invoke(LinkData, new EventArgs());
        }

        public event EventHandler MustBeDestructed;
    }
}
