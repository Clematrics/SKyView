using SkyView.Nodes;
using SkyView.Utils;
using System;
using System.Collections;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SkyView.Tabs {
    /// <summary>
    /// Logique d'interaction pour NodeEditor.xaml
    /// </summary>
    public partial class NodeEditor : UserControl {
        public NodeEditor() {
            NodesAssemblyNodeEditor = new NodesAssembly();

            InitializeComponent();
            GlobalCanvas.DataContext = this;
        }

        //public void AddNode(object sender, NodeEventArgs e) {
        //    NodesAssembly assembly = (NodesAssembly)sender;
        //    LogicalNode logicalNode = assembly.nodesCollection.Find(x => x.id == e.id);
        //    Node node = new Node(logicalNode, container);
        //    Canvas.SetTop(node as UIElement, Canvas.GetTop(container as UIElement) + graphCanvas.ActualHeight / 2);
        //    Canvas.SetLeft(node as UIElement, Canvas.GetLeft(container as UIElement) + graphCanvas.ActualWidth / 2);

        //    node.DraggingLinkEvent += linkConnection;
        //    node.MustBeSelected += changeSelection;

        //    container.Children.Add(node);
        //}
        //public void RemoveNode(object sender, NodeEventArgs e) {
        //    IEnumerator it = container.Children.GetEnumerator();
        //    it.MoveNext();
        //    it.MoveNext();
        //    do {
        //        if ((it.Current as Node).id == e.id && (it.Current as Node).type != NodeType.Output) {
        //            container.Children.Remove(it.Current as UIElement);
        //            return;
        //        }
        //    } while (it.MoveNext());
        //}

        //private void changeSelection(object sender, NodeEventArgs e) {
        //    foreach (UIElement element in container.Children)
        //        Canvas.SetZIndex(element, 0);
        //    nodesAssemblyNodeEditor.idSelected = e.id;
        //    Canvas.SetZIndex(sender as UIElement, 10);
        //}

        private List<UserControl> TempLink;
        private int TypeFirstPin;

        private void LinkConnection(UserControl sender, int type) {
            TempLink.Add(sender);
            if (TempLink.Count == 1)
                TypeFirstPin = type;
            if (TempLink.Count == 2) {
                if (TypeFirstPin != type) {

                }
            }
            TempLink = new List<UserControl>();
        }

        #region NodesAssembly Property
        public NodesAssembly NodesAssemblyNodeEditor {
            get { return (NodesAssembly)GetValue(NodesAssemblyProperty); }
            set { SetValue(NodesAssemblyProperty, value); }
        }
        public static readonly DependencyProperty NodesAssemblyProperty = DependencyProperty.Register(
            "NodesAssemblyNodeEditor",
            typeof(NodesAssembly),
            typeof(NodeEditor),
            new PropertyMetadata(new NodesAssembly()));
        #endregion NodesAssembly Property

        #region Déplacement du container dans l'éditeur

        private Point CurrentPosition = new Point(0,0);
        private Point StartDraggingPoint;
        private Point VectorDraggingPoint;
        private bool IsGraphMoving = false;

        private void BackgroundContainer_MouseDown(object sender, MouseButtonEventArgs e) {
            BackgroundContainer.CaptureMouse();
            StartDraggingPoint = Mouse.GetPosition(this);
            IsGraphMoving = true;
        }
        private void BackgroundContainer_MouseMove(object sender, MouseEventArgs e) {

            if (!IsGraphMoving) return;

            VectorDraggingPoint = (Point)(Mouse.GetPosition(this) - StartDraggingPoint);
            CurrentPosition = CurrentPosition + (Vector)VectorDraggingPoint;
            CheckAndUpdateContainer();

            StartDraggingPoint = Mouse.GetPosition(this);
        }
        private void BackgroundContainer_MouseUp(object sender, MouseButtonEventArgs e) {
             BackgroundContainer.ReleaseMouseCapture();
             IsGraphMoving = false;
        }

        private void NodeEditor_SizeChanged(object sender, SizeChangedEventArgs e) {
            CheckAndUpdateContainer();
        }

        private void CheckAndUpdateContainer() {
            if (CurrentPosition.X > 0)
                CurrentPosition.X = 0;
            if (CurrentPosition.X < GlobalCanvas.ActualWidth - 4096)
                CurrentPosition.X = GlobalCanvas.ActualWidth - 4096;
            if (CurrentPosition.Y > 0)
                CurrentPosition.Y = 0;
            if (CurrentPosition.Y < GlobalCanvas.ActualHeight - 4096)
                CurrentPosition.Y = GlobalCanvas.ActualHeight - 4096;
            Canvas.SetTop(BackgroundContainer, CurrentPosition.Y);
            Canvas.SetLeft(BackgroundContainer, CurrentPosition.X);
            Canvas.SetTop(NodeContainer, CurrentPosition.Y);
            Canvas.SetLeft(NodeContainer, CurrentPosition.X);
            Canvas.SetTop(LinksContainer, CurrentPosition.Y);
            Canvas.SetLeft(LinksContainer, CurrentPosition.X);
        }

        #endregion

        private void Node_MustBeSelected(object sender, NodeEventArgs e) {
            NodesAssemblyNodeEditor.IdSelected = (sender as LogicalNode).Id;
        }
    }
}
