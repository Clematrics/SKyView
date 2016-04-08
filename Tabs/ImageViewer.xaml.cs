using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkyView.Tabs {
    /// <summary>
    /// Logique d'interaction pour ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl {
        public ImageViewer() {
            InitializeComponent();
        }

        #region Déplacement de l'image dans l'éditeur

        private Point currentGraphPoint = new Point(0, 0);
        private Point startDragPoint;
        private Point vectorDragPoint;
        private bool IsGraphMoving = false;

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            image.CaptureMouse();
            startDragPoint = Mouse.GetPosition(this);
            IsGraphMoving = true;
        }
        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsGraphMoving) return;

            vectorDragPoint = (Point)(Mouse.GetPosition(this) - startDragPoint);
            currentGraphPoint = currentGraphPoint + (Vector)vectorDragPoint;
            CheckAndUpdateImage();

            startDragPoint = Mouse.GetPosition(this);
        }
        private void image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Released) return;

            image.ReleaseMouseCapture();
            IsGraphMoving = false;
        }

        private void ImageViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckAndUpdateImage();
        }

        private void CheckAndUpdateImage()
        {
            if (currentGraphPoint.X < 0)
                currentGraphPoint.X = 0;
            if (currentGraphPoint.X > imageCanvas.ActualWidth)
                currentGraphPoint.X = imageCanvas.ActualWidth;
            if (currentGraphPoint.Y < 0)
                currentGraphPoint.Y = 0;
            if (currentGraphPoint.Y > imageCanvas.ActualHeight)
                currentGraphPoint.Y = imageCanvas.ActualHeight;
            Canvas.SetTop(image, currentGraphPoint.Y);
            Canvas.SetLeft(image, currentGraphPoint.X);
        }
        #endregion

    }
}
