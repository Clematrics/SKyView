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

        public event NodeEventHandler MustBeSelected;
        public delegate void NodeEventHandler(object sender);

        #region Gestion du déplacement de la node

        public event NodePositionChangedEventHandler PositionChanged;

        private Point CurrentGraphPoint;
        private Point Offset;
        private bool IsGraphMoving = false;

        private void Navigator_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount >= 2) {
                TitleDisplay.Visibility = Visibility.Hidden;
                TitleContainer.Visibility = Visibility.Visible;
                return;
            }
            TitleDisplay.Visibility = Visibility.Visible;
            TitleContainer.Visibility = Visibility.Hidden;
            RectTitle.CaptureMouse();
            Offset = Mouse.GetPosition(this);
            IsGraphMoving = true;
            if (NodeData.IsSelected == false)
                MustBeSelected?.Invoke(NodeData);
        }

        private void Navigator_MouseUp(object sender, MouseButtonEventArgs e) {
            RectTitle.ReleaseMouseCapture();
            IsGraphMoving = false;
        }

        private void Navigator_MouseMove(object sender, MouseEventArgs e) {
            if (!IsGraphMoving) return;

            CurrentGraphPoint = new Point(NodeData.X, NodeData.Y) + (Vector)Mouse.GetPosition(this) - (Vector)Offset;
            PositionChanged?.Invoke(this, (Vector)Mouse.GetPosition(this) - (Vector)Offset);

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

        private void InputPin_Loaded(object sender, RoutedEventArgs e) {
            PositionChanged += (sender as InputPin).UpdatePositionData;
            PositionChanged?.Invoke(this, new Vector(0, 0));
        }

        private void OutputPin_Loaded(object sender, RoutedEventArgs e) {
            PositionChanged += (sender as OutputPin).UpdatePositionData;
            PositionChanged?.Invoke(this, new Vector(0, 0));
        }

        private void InputPin_Unloaded(object sender, RoutedEventArgs e) {
            PositionChanged -= (sender as InputPin).UpdatePositionData;
        }

        private void OutputPin_Unloaded(object sender, RoutedEventArgs e) {
            PositionChanged -= (sender as OutputPin).UpdatePositionData;
        }

        private void TitleContainer_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Tab) {
                TitleDisplay.Visibility = Visibility.Visible;
                TitleContainer.Visibility = Visibility.Hidden;
            }
        }

        public event LinkDraggingEventHandler BeginDrag;
        private void Pin_BeginDrag(object sender, PinType type) {
            BeginDrag?.Invoke(sender, type);
        }
        public event LinkDraggingEventHandler EndDrag;
        private void Pin_EndDrag(object sender, PinType type) {
            EndDrag?.Invoke(sender, type);
        }
    }
}
