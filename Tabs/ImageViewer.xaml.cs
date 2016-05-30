using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SkyView.Tabs {
    /// <summary>
    /// Logique d'interaction pour ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl {
        public ImageViewer() {
            InitializeComponent();
            GlobalCanvas.DataContext = this;
        }

        #region SourceProperty
        public Image.Image Source {
            get { return (Image.Image)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
                "Source",
                typeof(Image.Image),
                typeof(ImageViewer),
                new PropertyMetadata(new Image.Image(0, 0)));
        #endregion PropertiesProperty

        #region Déplacement de l'image dans l'éditeur

        private Point CurrentGraphPoint = new Point(0, 0);
        private Point StartDragPoint;
        private Point VectorDragPoint;
        private bool IsGraphMoving = false;

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            Image.CaptureMouse();
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

            Image.ReleaseMouseCapture();
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
            if (CurrentGraphPoint.X > GlobalCanvas.ActualWidth)
                CurrentGraphPoint.X = GlobalCanvas.ActualWidth;
            if (CurrentGraphPoint.Y < 0)
                CurrentGraphPoint.Y = 0;
            if (CurrentGraphPoint.Y > GlobalCanvas.ActualHeight)
                CurrentGraphPoint.Y = GlobalCanvas.ActualHeight;
            Canvas.SetTop(Image, CurrentGraphPoint.Y);
            Canvas.SetLeft(Image, CurrentGraphPoint.X);
        }
        #endregion


        #region Gestion du zoom de l'image

        private double Zoom = 1;
        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Zoom += e.Delta > 0 ? .2 : -.2;
            Zoom = Zoom < .2 ? .2 : Zoom;
            Zoom = Zoom > 10 ? 10 : Zoom;
            ScaleTransform scale = new ScaleTransform(Zoom, Zoom);
            Image.LayoutTransform = scale;
        }
        #endregion
    }
}
