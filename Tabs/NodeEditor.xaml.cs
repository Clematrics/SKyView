using SkyView.Nodes;
using SkyView.Utils;
using System;
using System.Collections;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SkyView.Tabs {
    /// <summary>
    /// Logique d'interaction pour NodeEditor.xaml
    /// </summary>
    public partial class NodeEditor : UserControl {
        public NodeEditor() {
            InitializeComponent();
            nodes = new Collection<LogicalNode>(() => new LogicalNode());
            idSelected = 0;
            container.DataContext = this;
        }

        public void AddNode(object sender, NodeEventArgs e) {
            NodesAssembly assembly = (NodesAssembly)sender;
            LogicalNode logicalNode = assembly.nodes_collection.Find(x => x.id == e.id);
            Node node = new Node(logicalNode, container);
            Canvas.SetTop(node as UIElement, Canvas.GetTop(container as UIElement) + graphCanvas.ActualHeight / 2);
            Canvas.SetLeft(node as UIElement, Canvas.GetLeft(container as UIElement) + graphCanvas.ActualWidth / 2);

            node.DraggingLinkEvent += linkConnection;
            node.MustBeSelected += changeSelection;

            container.Children.Add(node);
        }
        public void RemoveNode(object sender, NodeEventArgs e) {
            IEnumerator it = container.Children.GetEnumerator();
            it.MoveNext();
            it.MoveNext();
            do {
                if ((it.Current as Node).id == e.id && (it.Current as Node).type != NodeType.Output) {
                    container.Children.Remove(it.Current as UIElement);
                    return;
                }
            } while (it.MoveNext());
        }

        private void changeSelection(object sender, NodeEventArgs e) {
            foreach (UIElement element in container.Children)
                Canvas.SetZIndex(element, 0);
            idSelected = e.id;
            Canvas.SetZIndex(sender as UIElement, 10);
        }

        private Link currentLink;
        private DragInfo firstPin;

        private void linkConnection(object sender, DragInfo info) {
            if (info == DragInfo.FromInput || info == DragInfo.FromOutput) {
                currentLink = new Link();
                if (info == DragInfo.FromInput) {
                    currentLink.startPoint = (sender as InputPin).slot.TransformToAncestor(container).Transform(new Point(4, 5));
                    (sender as InputPin).link = currentLink;
                }
                if (info == DragInfo.FromOutput) { 
                    currentLink.finishPoint = (sender as OutputPin).slot.TransformToAncestor(container).Transform(new Point(4, 5));
                    (sender as OutputPin).links.Add(currentLink);
                }
                linkContainer.Children.Add(currentLink);
                firstPin = info;
            }
            if (info == DragInfo.ToInput || info == DragInfo.ToOutput) {
                if (info == DragInfo.ToInput) {
                    currentLink.startPoint = (sender as InputPin).slot.TransformToAncestor(container).Transform(new Point(4, 5));
                    (sender as InputPin).link = currentLink;
                }
                if (info == DragInfo.ToOutput) {
                    currentLink.finishPoint = (sender as OutputPin).slot.TransformToAncestor(container).Transform(new Point(4, 5));
                    (sender as OutputPin).links.Add(currentLink);
                }
                currentLink = null;
            }
        }

        #region nodes
        public Collection<LogicalNode> nodes {
            get { return (Collection<LogicalNode>)GetValue(nodesProperty); }
            set { SetValue(nodesProperty, value); }
        }
        public static readonly DependencyProperty nodesProperty = DependencyProperty.Register(
                "nodes",
                typeof(Collection<LogicalNode>),
                typeof(NodeEditor),
                new PropertyMetadata(new Collection<LogicalNode>(() => new LogicalNode())));
        #endregion nodes

        #region idSelected
        public long idSelected {
            get { return (long)GetValue(idSelected_property); }
            set { SetValue(idSelected_property, value); }
        }
        public static readonly DependencyProperty idSelected_property = DependencyProperty.Register(
                "idSelected",
                typeof(long),
                typeof(NodeEditor),
                new PropertyMetadata((long)0));
        #endregion idSelected

        #region Déplacement du container dans l'éditeur

        private Point currentGraphPoint = new Point(0,0);
        private Point startDragPoint;
        private Point vectorDragPoint;
        private bool IsGraphMoving = false;

        private void container_MouseDown(object sender, MouseButtonEventArgs e) {
            currentLink = null;
            background.CaptureMouse();
            startDragPoint = Mouse.GetPosition(this);
            IsGraphMoving = true;
        }
        private void container_MouseMove(object sender, MouseEventArgs e) {
            if (currentLink != null && e.LeftButton == MouseButtonState.Pressed) {
                if (firstPin == DragInfo.FromInput)
                    currentLink.finishPoint = Mouse.GetPosition(container);
                if (firstPin == DragInfo.FromOutput)
                    currentLink.startPoint = Mouse.GetPosition(container);
            }

            if (!IsGraphMoving) return;

            vectorDragPoint = (Point)(Mouse.GetPosition(this) - startDragPoint);
            currentGraphPoint = currentGraphPoint + (Vector)vectorDragPoint;
            CheckAndUpdateContainer();

            startDragPoint = Mouse.GetPosition(this);
        }
        private void container_MouseUp(object sender, MouseButtonEventArgs e) {
             background.ReleaseMouseCapture();
             IsGraphMoving = false;
        }

        private void NodeEditor_SizeChanged(object sender, SizeChangedEventArgs e) {
            CheckAndUpdateContainer();
        }

        private void CheckAndUpdateContainer() {
            if (currentGraphPoint.X > 0)
                currentGraphPoint.X = 0;
            if (currentGraphPoint.X < graphCanvas.ActualWidth - 4096)
                currentGraphPoint.X = graphCanvas.ActualWidth - 4096;
            if (currentGraphPoint.Y > 0)
                currentGraphPoint.Y = 0;
            if (currentGraphPoint.Y < graphCanvas.ActualHeight - 4096)
                currentGraphPoint.Y = graphCanvas.ActualHeight - 4096;
            Canvas.SetTop(container, currentGraphPoint.Y);
            Canvas.SetLeft(container, currentGraphPoint.X);
            Canvas.SetTop(linkContainer, currentGraphPoint.Y);
            Canvas.SetLeft(linkContainer, currentGraphPoint.X);
        }
        #endregion
    }
}
