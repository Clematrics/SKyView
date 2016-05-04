using SkyView.Nodes;
using SkyView.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyView {
    /// <summary>
    /// Logique d'interaction pour Node.xaml
    /// </summary>
    public partial class Node : UserControl {
        public Node (LogicalNode node, UIElement container) {
            InitializeComponent();
            id = node.id;
            name = node.name;
            type = node.type;
            this.container = container;

            foreach (CollectionItem<LogicalInputPin> input in node.inPins) {
                InputPin pin = new InputPin(input.Member);
                pin.DragFrom += DraggingLink;
                pin.DragOn += DraggingLink;
                positionChanged += pin.OnPositionChanged;
                inputs.Children.Add(pin);
            }
            foreach (CollectionItem<LogicalOutputPin> output in node.outPins) {
                OutputPin pin = new OutputPin(output.Member);
                pin.DragFrom += DraggingLink;
                pin.DragOn += DraggingLink;
                positionChanged += pin.OnPositionChanged;
                outputs.Children.Add(pin);
            }
            TitleContainer.DataContext = this;
        }

        public long id;
        public string name { get; set; }
        public NodeType type { get; }
        private UIElement container { get; set; }

        public event DragLinkEventHandler DraggingLinkEvent;
        public delegate void DragLinkEventHandler(object sender, DragInfo info);
        private void DraggingLink(object sender, DragInfo info) {
            DraggingLinkEvent(sender, info);
        }

        public event NodeEventHandler MustBeSelected;
        public delegate void NodeEventHandler(object sender, NodeEventArgs e);

        #region Gestion du déplacement de la node

        private Point currentGraphPoint;
        private Point offset;
        private bool IsGraphMoving = false;

        private void Navigator_MouseDown(object sender, MouseButtonEventArgs e) {
            Navigator.CaptureMouse();
            offset = Mouse.GetPosition(this);
            IsGraphMoving = true;
            MustBeSelected(this, new NodeEventArgs(id, NodeType.Unknown));
        }

        private void Navigator_MouseUp(object sender, MouseButtonEventArgs e) {
            Navigator.ReleaseMouseCapture();
            IsGraphMoving = false;
        }

        private void Navigator_MouseMove(object sender, MouseEventArgs e) {
            if (!IsGraphMoving) return;

            currentGraphPoint = Mouse.GetPosition(Parent as UIElement) - (Vector)offset;

            if (currentGraphPoint.X < 0)
                currentGraphPoint.X = 0;
            if (currentGraphPoint.X > 4096 - ActualWidth)
                currentGraphPoint.X = 4096 - ActualWidth;
            if (currentGraphPoint.Y < 0)
                currentGraphPoint.Y = 0;
            if (currentGraphPoint.Y > 4096 - ActualHeight)
                currentGraphPoint.Y = 4096 - ActualHeight;
            Canvas.SetTop(this, currentGraphPoint.Y);
            Canvas.SetLeft(this, currentGraphPoint.X);

            positionChanged(this, container);
        }
        private event PositionChangedEventHandler positionChanged;
        private delegate void PositionChangedEventHandler(object sender, UIElement ancestor);
        #endregion
    }
}
