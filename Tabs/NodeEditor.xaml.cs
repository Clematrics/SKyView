using SkyView.Nodes;
using SkyView.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public LogicalLink TemporaryLink { get; set; }
        private bool IsDraggingLink;

        private DependencyObject StartingPin;
        private PinType FirstTypePin;

        private void Node_BeginDrag(object sender, PinType type) {
            StartingPin = (sender as DependencyObject);
            FirstTypePin = type;
            if (FirstTypePin == PinType.Input)
                TemporaryLink = new LogicalLink((sender as InputPin).InputPinData);
            else if (FirstTypePin == PinType.Output)
                TemporaryLink = new LogicalLink((sender as OutputPin).OutputPinData);
            IsDraggingLink = true;
            Mouse.Capture(BackgroundContainer, CaptureMode.Element);
        }

        private void Node_EndDrag(object sender, PinType type) {
            if (FirstTypePin == PinType.Input && type == PinType.Output)
                NodesAssemblyNodeEditor.Love((StartingPin as InputPin).InputPinData, (sender as OutputPin).OutputPinData);
            else if (FirstTypePin == PinType.Output && type == PinType.Input)
                NodesAssemblyNodeEditor.Love((sender as InputPin).InputPinData, (StartingPin as OutputPin).OutputPinData);
            TemporaryLink = null;
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

        public Point CurrentPosition = new Point(0,0);
        private Point StartDraggingPoint;
        private Point VectorDraggingPoint;
        private bool IsGraphMoving = false;

        private void BackgroundContainer_MouseDown(object sender, MouseButtonEventArgs e) {
            //if (e.ClickCount > 2) {
            //    SelectedNodes = new List<LogicalNode>();
            //    NodesAssemblyNodeEditor.IdSelected = 0;
            //    foreach (CollectionItem<LogicalNode> node in NodesAssemblyNodeEditor.NodesCollection.List) {
            //        node.Member.Z = 0;
            //        node.Member.IsSelected = false;
            //    }
            //}
            BackgroundContainer.CaptureMouse();
            StartDraggingPoint = Mouse.GetPosition(this);
            IsGraphMoving = true;
        }
        private void BackgroundContainer_MouseMove(object sender, MouseEventArgs e) {
            if (IsGraphMoving) {
                VectorDraggingPoint = (Point)(Mouse.GetPosition(this) - StartDraggingPoint);
                CurrentPosition = CurrentPosition + (Vector)VectorDraggingPoint;
                CheckAndUpdateContainer();

                StartDraggingPoint = Mouse.GetPosition(this);
            }
            else if (IsDraggingLink && TemporaryLink != null) {
                if (FirstTypePin == PinType.Input)
                    TemporaryLink.Output.Coordinates = Mouse.GetPosition(TemporaryLinkContainer);
                else TemporaryLink.Input.Coordinates = Mouse.GetPosition(TemporaryLinkContainer);
                TemporaryDisplayedLink.LinkData = TemporaryLink;
            }
        }
        private void BackgroundContainer_MouseUp(object sender, MouseButtonEventArgs e) {
            if (IsGraphMoving) {
                BackgroundContainer.ReleaseMouseCapture();
                IsGraphMoving = false;
            }
            else if (IsDraggingLink) {
                Mouse.Capture(null);
                VisualTreeHelper.HitTest(this, null, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(Mouse.GetPosition(this)));
                if (HitResult is Polygon) {
                    InputPin input = FindParent<InputPin>(HitResult);
                    if (input != null)
                        input.ForceEndDragging();
                    OutputPin output = FindParent<OutputPin>(HitResult);
                    if (output != null)
                        output.ForceEndDragging();
                }

                TemporaryLink = null;
                TemporaryDisplayedLink.LinkData = new LogicalLink();
                IsDraggingLink = false;
            }
        }

        private DependencyObject HitResult;
        public HitTestResultBehavior MyHitTestResult(HitTestResult result) {
            if (result.VisualHit.GetType() == typeof(Polygon)) {
                HitResult = result.VisualHit;
                return HitTestResultBehavior.Stop;
            }
            return HitTestResultBehavior.Continue;
        }
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
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
            Canvas.SetTop(TemporaryLinkContainer, CurrentPosition.Y);
            Canvas.SetLeft(TemporaryLinkContainer, CurrentPosition.X);
        }

        #endregion

        List<LogicalNode> SelectedNodes = new List<LogicalNode>();
        private void Node_MustBeSelected(object sender) {
            if (InputManager.Current.PrimaryKeyboardDevice.IsKeyDown(Key.LeftCtrl) || InputManager.Current.PrimaryKeyboardDevice.IsKeyDown(Key.RightCtrl)) {
                SelectedNodes.Add((sender as LogicalNode));
                NodesAssemblyNodeEditor.IdSelected = (sender as LogicalNode).Id;
                (sender as LogicalNode).IsSelected = true;
                (sender as LogicalNode).Z = 1;
            }
            else {
                SelectedNodes = new List<LogicalNode>();
                NodesAssemblyNodeEditor.IdSelected = (sender as LogicalNode).Id;
                foreach (CollectionItem<LogicalNode> node in NodesAssemblyNodeEditor.NodesCollection.List) {
                    node.Member.Z = 0;
                    node.Member.IsSelected = false;
                }
                (sender as LogicalNode).IsSelected = true;
                (sender as LogicalNode).Z = 1;
            }
        }

        private void Link_MustBeDestructed(object sender, EventArgs e) {
            NodesAssemblyNodeEditor.Divorce(sender as LogicalLink);
        }

        private void Node_PositionChanged(object sender, Vector vectorMovment) {
            foreach(LogicalNode node in SelectedNodes) {
                node.X += vectorMovment.X;
                node.Y += vectorMovment.Y;
                if (node.X < 0)
                    node.X = 0;
                if (node.X > 4096 - (sender as Node).ActualWidth)
                    node.X = 4096 - (sender as Node).ActualWidth;
                if (node.Y < 0)
                    node.Y = 0;
                if (node.Y > 4096 - (sender as Node).ActualHeight)
                    node.Y = 4096 - (sender as Node).ActualHeight;
            }
        }
    }
}
