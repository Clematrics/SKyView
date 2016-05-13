using SkyView.Nodes;
using SkyView.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyView {
    /// <summary>
    /// Logique d'interaction pour Node.xaml
    /// </summary>
    public partial class Node : UserControl {

        public Node() { InitializeComponent(); GlobalNode.DataContext = this; }

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
        public delegate void NodeEventHandler(object sender);

        public event PinSelectionEventHandler PinSelection;
        private void AnnouncePinSelection( object sender, PinType type, int index ) {
            PinSelection?.Invoke(sender, type, index);
        }

        #region Gestion du déplacement de la node

        private Point CurrentGraphPoint;
        private Point Offset;
        private bool IsGraphMoving = false;

        private void Navigator_MouseDown(object sender, MouseButtonEventArgs e) {
            Navigator.CaptureMouse();
            Offset = Mouse.GetPosition(this);
            IsGraphMoving = true;
            MustBeSelected?.Invoke(NodeData);
        }

        private void Navigator_MouseUp(object sender, MouseButtonEventArgs e) {
            Navigator.ReleaseMouseCapture();
            IsGraphMoving = false;
        }

        private void Navigator_MouseMove(object sender, MouseEventArgs e) {
            if (!IsGraphMoving) return;

            CurrentGraphPoint = new Point(NodeData.X, NodeData.Y) + (Vector)Mouse.GetPosition(this) - (Vector)Offset;

            if (CurrentGraphPoint.X < 0)
                CurrentGraphPoint.X = 0;
            if (CurrentGraphPoint.X > 4096 - ActualWidth)
                CurrentGraphPoint.X = 4096 - ActualWidth;
            if (CurrentGraphPoint.Y < 0)
                CurrentGraphPoint.Y = 0;
            if (CurrentGraphPoint.Y > 4096 - ActualHeight)
                CurrentGraphPoint.Y = 4096 - ActualHeight;
            NodeData.X = CurrentGraphPoint.X;
            NodeData.Y = CurrentGraphPoint.Y;

        }
        #endregion
    }
}
