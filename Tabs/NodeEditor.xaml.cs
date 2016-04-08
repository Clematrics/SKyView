using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyView.Tabs {
    /// <summary>
    /// Logique d'interaction pour NodeEditor.xaml
    /// </summary>
    public partial class NodeEditor : UserControl {
        public NodeEditor() {
            InitializeComponent();
        }

        #region Déplacement du container dans l'éditeur

        private Point currentGraphPoint = new Point(0,0);
        private Point startDragPoint;
        private Point vectorDragPoint;
        private bool IsGraphMoving = false;

        private void container_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            container.CaptureMouse();
            startDragPoint = Mouse.GetPosition(this);
            IsGraphMoving = true;
        }
        private void container_MouseMove(object sender, MouseEventArgs e) {
            if (!IsGraphMoving) return;

            vectorDragPoint = (Point)(Mouse.GetPosition(this) - startDragPoint);
            currentGraphPoint = currentGraphPoint + (Vector)vectorDragPoint;
            CheckAndUpdateContainer();

            startDragPoint = Mouse.GetPosition(this);
        }
        private void container_MouseUp(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton != MouseButtonState.Released) return;

            container.ReleaseMouseCapture();
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
        }
        #endregion
    }
}
