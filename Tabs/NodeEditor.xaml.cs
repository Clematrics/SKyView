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

        private List<UserControl> TempLink;
        private int IndexFirstPin;
        private PinType TypeFirstPin;

        private void Node_PinSelection(object sender, PinType type, int index) {
            TempLink.Add(sender as UserControl);
            if (TempLink.Count == 1) {
                TypeFirstPin = type;
                IndexFirstPin = index;
            }
            if (TempLink.Count == 2 && TypeFirstPin != type) {
                if (TypeFirstPin == PinType.Input)
                    NodesAssemblyNodeEditor.Love((TempLink[0] as InputPin).InputPinData, IndexFirstPin, (TempLink[1] as OutputPin).OutputPinData, index);
                else if (TypeFirstPin == PinType.Output)
                    NodesAssemblyNodeEditor.Love((TempLink[1] as InputPin).InputPinData, IndexFirstPin, (TempLink[0] as OutputPin).OutputPinData, index);
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

        public Point CurrentPosition = new Point(0,0);
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

        private void Node_MustBeSelected(object sender) {
            NodesAssemblyNodeEditor.IdSelected = (sender as LogicalNode).Id;
        }
    }
}
