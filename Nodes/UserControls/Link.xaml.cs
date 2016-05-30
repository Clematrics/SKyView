using SkyView.Nodes;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SkyView {
    /// <summary>
    /// Logique d'interaction pour Link.xaml
    /// </summary>
    public partial class Link : UserControl, INotifyPropertyChanged {
        public Link() {
            InitializeComponent();
            Path.DataContext = this;

            LinkDataDescriptor.AddValueChanged(this, LinkData_OnLoaded);
        }

        public Point AnchorFromPin {
            get { return _AnchorFromPin; }
            set { _AnchorFromPin = value; RaisePropertyChanged("AnchorFromPin"); }
        }
        public Point AnchorToPin {
            get { return _AnchorToPin; }
            set { _AnchorToPin = value; RaisePropertyChanged("AnchorToPin"); }
        }

        private Point _AnchorFromPin;
        private Point _AnchorToPin;

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

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
        DependencyPropertyDescriptor LinkDataDescriptor = DependencyPropertyDescriptor.FromProperty(LinkDataProperty, typeof(Link));
        #endregion LinkData Property

        private void Line_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
                MustBeDestructed?.Invoke(LinkData, new EventArgs());
        }

        public event EventHandler MustBeDestructed;

        private void LinkData_OnLoaded(object sender, EventArgs e) {
            if (LinkData == null) return;
            LinkData.Input.PropertyChanged += LinkData_OnChange;
            LinkData.Output.PropertyChanged += LinkData_OnChange;
            LinkData_OnChange(this, new EventArgs());
        }

        private void LinkData_OnChange(object sender, EventArgs e) {
            AnchorFromPin = LinkData.Output.Coordinates + new Vector( Math.Abs( LinkData.Input.Coordinates.X - LinkData.Output.Coordinates.X ) / 4 + Math.Abs( LinkData.Input.Coordinates.Y - LinkData.Output.Coordinates.Y ) / 4, 0);
            AnchorToPin   = LinkData.Input.Coordinates + new Vector( -Math.Abs( LinkData.Input.Coordinates.X - LinkData.Output.Coordinates.X ) / 4 - Math.Abs( LinkData.Input.Coordinates.Y - LinkData.Output.Coordinates.Y ) / 4, 0);
        }
    }
}
