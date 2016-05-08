using SkyView.Image;
using SkyView.Utils;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace SkyView.Nodes {
    /// <summary>
    /// Logique d'interaction pour OutputPin.xaml
    /// </summary>
    public partial class OutputPin : UserControl {

        public OutputPin() { InitializeComponent(); GlobalOutputPin.DataContext = this; }

        public Collection<Link> links { get; set; }
        public delegate void PositionChangedEventHandler(object sender, UIElement e);

        public void OnPositionChanged(object sender, UIElement e) {
            IEnumerator it = links.GetEnumerator();
            while (it.MoveNext())
                (it.Current as CollectionItem<Link>).Member.startPoint = slot.TransformToAncestor(e).Transform(new Point(4, 5));
        }

        public event DragLinkEventHandler Drag;
        public delegate void DragLinkEventHandler(UserControl sender, int type);

        private void slot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Drag?.Invoke(this, 1);
        }

        public LogicalOutputPin OutputPinData {
            get { return (LogicalOutputPin)GetValue(OutputPinDataProperty); }
            set { SetValue(OutputPinDataProperty, value); }
        }
        public static readonly DependencyProperty OutputPinDataProperty = DependencyProperty.Register(
            "OutputPinData",
            typeof(LogicalOutputPin),
            typeof(OutputPin),
            new PropertyMetadata(new LogicalOutputPin("", Filters.NoFilter)));
    }
}
