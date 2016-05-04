using SkyView.Utils;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace SkyView.Nodes {
    /// <summary>
    /// Logique d'interaction pour OutputPin.xaml
    /// </summary>
    public partial class OutputPin : UserControl {
        public OutputPin(LogicalOutputPin pin) {
            InitializeComponent();
            name.Content = pin.name;
            links = new Collection<Link>(() => new Link());
        }

        public Collection<Link> links { get; set; }
        public delegate void PositionChangedEventHandler(object sender, UIElement e);

        public void OnPositionChanged(object sender, UIElement e) {
            IEnumerator it = links.GetEnumerator();
            while (it.MoveNext())
                (it.Current as CollectionItem<Link>).Member.startPoint = slot.TransformToAncestor(e).Transform(new Point(4, 5));
        }

        public event DragLinkEventHandler DragFrom;
        public event DragLinkEventHandler DragOn;
        public delegate void DragLinkEventHandler(object sender, DragInfo info);

        private void slot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            DragFrom(this, DragInfo.FromOutput);
        }

        private void slot_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            DragOn(this, DragInfo.ToOutput);
        }
    }
}
