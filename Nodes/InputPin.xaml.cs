using System;
using System.Windows;
using System.Windows.Controls;

namespace SkyView.Nodes {
    /// <summary>
    /// Logique d'interaction pour InputPin.xaml
    /// </summary>
    public partial class InputPin : UserControl {
        public InputPin(LogicalInputPin pin) {
            InitializeComponent();
            name.Content = pin.name;
        }

        public Link link { get; set; }
        public delegate void PositionChangedEventHandler(object sender, UIElement e);

        public void OnPositionChanged(object sender, UIElement e) {
            if (link != null)
                link.finishPoint = slot.TransformToAncestor(e).Transform(new Point(4, 5));
        }

        public event DragLinkEventHandler DragFrom;
        public event DragLinkEventHandler DragOn;
        public delegate void DragLinkEventHandler(object sender, DragInfo info);

        private void slot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            DragFrom(this, DragInfo.FromInput);
        }

        private void slot_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            DragOn(this, DragInfo.ToInput);
        }
    }
}
