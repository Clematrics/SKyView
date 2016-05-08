using System.Windows;
using System.Windows.Controls;

namespace SkyView.Nodes {
    /// <summary>
    /// Logique d'interaction pour InputPin.xaml
    /// </summary>
    public partial class InputPin : UserControl {

        public InputPin() { InitializeComponent(); GlobalInputPin.DataContext = this; }

        public Link link { get; set; }
        public delegate void PositionChangedEventHandler(object sender, UIElement e);

        public void OnPositionChanged(object sender, UIElement e) {
            if (link != null)
                link.finishPoint = slot.TransformToAncestor(e).Transform(new Point(4, 5));
        }

        public event DragLinkEventHandler Drag;
        public delegate void DragLinkEventHandler(UserControl sender, int type);

        private void slot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Drag?.Invoke(this, 2);
        }

        public LogicalInputPin InputPinData {
            get { return (LogicalInputPin)GetValue(InputPinDataProperty); }
            set { SetValue(InputPinDataProperty, value); }
        }
        public static readonly DependencyProperty InputPinDataProperty = DependencyProperty.Register(
            "InputPinData",
            typeof(LogicalInputPin),
            typeof(InputPin),
            new PropertyMetadata(new LogicalInputPin("")));
    }
}
