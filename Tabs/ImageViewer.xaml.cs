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

        private Point CurrentGraphPoint = new Point(0, 0);
        private Point StartDragPoint;
        private Point VectorDragPoint;
        private bool IsGraphMoving = false;

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            image.CaptureMouse();
            StartDragPoint = Mouse.GetPosition(this);
            IsGraphMoving = true;
        }
        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsGraphMoving) return;

            VectorDragPoint = (Point)(Mouse.GetPosition(this) - StartDragPoint);
            CurrentGraphPoint = CurrentGraphPoint + (Vector)VectorDragPoint;
            CheckAndUpdateImage();

            StartDragPoint = Mouse.GetPosition(this);
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
            if (CurrentGraphPoint.X < 0)
                CurrentGraphPoint.X = 0;
            if (CurrentGraphPoint.X > imageCanvas.ActualWidth)
                CurrentGraphPoint.X = imageCanvas.ActualWidth;
            if (CurrentGraphPoint.Y < 0)
                CurrentGraphPoint.Y = 0;
            if (CurrentGraphPoint.Y > imageCanvas.ActualHeight)
                CurrentGraphPoint.Y = imageCanvas.ActualHeight;
            Canvas.SetTop(image, CurrentGraphPoint.Y);
            Canvas.SetLeft(image, CurrentGraphPoint.X);
        }
        #endregion


        #region Gestion du zoom de l'image

        private double Zoom = 1;
        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Zoom += e.Delta > 0 ? 0.1 : -0.1 ;
            Matrix matrix = image.RenderTransform.Value;

            matrix.ScaleAt(Zoom, Zoom, 0.5, 0.5);

            image.RenderTransform = new MatrixTransform(matrix);
        }
        #endregion
    }
}
