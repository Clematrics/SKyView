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

        public Node() { InitializeComponent(); GlobalNode.DataContext = this; }
        public Node (LogicalNode node) {
            InitializeComponent();

            //foreach (CollectionItem<LogicalInputPin> input in node.InPins) {
            //    InputPin pin = new InputPin(input.Member);
            //    pin.Drag += DraggingLink;
            //    positionChanged += pin.OnPositionChanged;
            //    inputs.Children.Add(pin);
            //}
            //foreach (CollectionItem<LogicalOutputPin> output in node.OutPins) {
            //    OutputPin pin = new OutputPin(output.Member);
            //    pin.Drag += DraggingLink;
            //    positionChanged += pin.OnPositionChanged;
            //    outputs.Children.Add(pin);
            //}
            GlobalNode.DataContext = this;
        }

        #region NodeData
        public LogicalNode NodeData {
            get { return (LogicalNode)GetValue(NodeDataProperty); }
            set { SetValue(NodeDataProperty, value); }
        }

        public static readonly DependencyProperty NodeDataProperty = DependencyProperty.Register(
            "NodeData",
            typeof(LogicalNode),
            typeof(Node),
            new PropertyMetadata(new LogicalNode()));
        #endregion NodeData

        public event DragLinkEventHandler DraggingLinkEvent;
        public delegate void DragLinkEventHandler(UserControl sender, int type);
        private void DraggingLink(UserControl sender, int type) {
            DraggingLinkEvent(sender, type);
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
            MustBeSelected?.Invoke(NodeData, new NodeEventArgs(NodeType.Unknown));
        }

        private void Navigator_MouseUp(object sender, MouseButtonEventArgs e) {
            Navigator.ReleaseMouseCapture();
            IsGraphMoving = false;
        }

        private void Navigator_MouseMove(object sender, MouseEventArgs e) {
            if (!IsGraphMoving) return;

            currentGraphPoint = new Point(NodeData.X, NodeData.Y) + (Vector)Mouse.GetPosition(this) - (Vector)offset;

            if (currentGraphPoint.X < 0)
                currentGraphPoint.X = 0;
            if (currentGraphPoint.X > 4096 - ActualWidth)
                currentGraphPoint.X = 4096 - ActualWidth;
            if (currentGraphPoint.Y < 0)
                currentGraphPoint.Y = 0;
            if (currentGraphPoint.Y > 4096 - ActualHeight)
                currentGraphPoint.Y = 4096 - ActualHeight;
            NodeData.X = currentGraphPoint.X;
            NodeData.Y = currentGraphPoint.Y;

        }
        private event PositionChangedEventHandler positionChanged;
        private delegate void PositionChangedEventHandler(object sender, UIElement ancestor);
        #endregion
    }
}
