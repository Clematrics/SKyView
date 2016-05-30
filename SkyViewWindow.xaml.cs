using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

using SkyView.Utils;
using SkyView.Nodes;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace SkyView {
    /// <summary>
    /// Logique d'interaction pour SkyViewWindow.xaml
    /// </summary>
    public partial class SkyViewWindow : Window, INotifyPropertyChanged {
        #region Constructeur
        /// <summary>
        /// Constructeur de la fenêtre principale de travail
        /// </summary>
        public SkyViewWindow() {

            InitializeComponent();

            SharedNodesAssembly = new NodesAssembly();
            SharedNodesAssembly.PropertyChanged += Properties.IdChanged;
            SharedNodesAssembly.AddNode(NodeType.Output, 0, 0);
            Renderer = new RenderEngine.RenderEngine();

            SourceInitialized += win_SourceInitialized;
            // Ajout d'un événement lorsque WindowState change d'état
            DependencyPropertyDescriptor.FromProperty(WindowStateProperty, typeof(Window)).AddValueChanged(this, OnWindowStateChanged);
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;


            GlobalWindow.DataContext = this;
        }
        #endregion

        #region Propriétés partagées
        public NodesAssembly SharedNodesAssembly {
            get { return _SharedNodesAssembly; }
            set { _SharedNodesAssembly = value;  RaisePropertyChanged("SharedNodesAssembly"); }
        }
        private NodesAssembly _SharedNodesAssembly;
        #endregion

        #region Gestion du rendu
        public RenderEngine.RenderEngine Renderer { get; set; }
        #endregion

        #region Gestion du clic sur le logo ou la barre de fenêtre
        private void Logo_MouseDown(object sender, MouseButtonEventArgs e) {
            // Appel du gestionnaire de déplacement de fenêtre
            WindowMoveManager(sender, e);
        }
        private void WindowBar_MouseDown(object sender, MouseButtonEventArgs e) {
            // Appel du gestionnaire de déplacement de fenêtre
            WindowMoveManager(sender, e);
        }
        #endregion

        #region Gestion du déplacement de la fenêtre
        /// <summary>
        /// Gère le déplacement de la fenêtre en fonction de son état, et des interactions entre l'utilisateur et la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMoveManager(object sender, MouseButtonEventArgs e) {
            // Si on double-clique sur un des UIElement appelant la fonction, on change l'état de la fenêtre en fonction de son état actuel
            if (e.ClickCount >= 2)
                WindowState =
                    WindowState == WindowState.Maximized ?
                    WindowState.Minimized
                  : WindowState.Maximized;
            // Sinon, si un bouton est pressé
            else if (e.ButtonState == MouseButtonState.Pressed) {
                try {
                    // Si la fenêtre est dans son état "agrandi", on la positionne correctement sous la souris lorsqu'elle est déplacée
                    if (WindowState == WindowState.Maximized) {
                        // On obtient les informations de position sur la souris
                        double mouseX = e.GetPosition(this).X, mouseY = e.GetPosition(this).Y;
                        // On récupère les informations actuelles sur sa taille
                        double lastActualWidth = ActualWidth, lastActualHeight = ActualHeight;
                        // Puis on actualise son état
                        WindowState = WindowState.Normal;
                        // Et on la repositionne correctement sur l'écran où elle était agrandie et sous la souris
                        int screen = getActualScreen();
                        Rectangle bounds = Screen.AllScreens[screen].Bounds;
                        Left = e.GetPosition(this).X - mouseX * Width / lastActualWidth + bounds.X;
                        Top = e.GetPosition(this).Y - mouseY * Height / lastActualHeight + bounds.Y;
                    }
                    //Dans tous les cas, on autorise son déplacement à la souris
                    DragMove();
                }
                catch (Exception) { /* ignore */ }
            }
        }
        #endregion

        #region Gestion des états de la fenêtre via les boutons ou le mouvement
        /// <summary>
        /// Méthode appelée par la propriété de dépendance lorsque l'état de la fenêtre change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowStateChanged(object sender, EventArgs e) {
            // On change l'icône du bouton "agrandir"
            Maximize.Content = FindResource(
                WindowState == WindowState.Maximized ?
                "IconMinimize"
                : "IconMaximize");
            // On fait une update de l'état de la fenêtre agrandie pour la replacer correctement sur l'écran
            if (WindowState == WindowState.Maximized) {
                // Pour éviter les boucles, on supprime la méthode de la propriété de dépendance de l'état de la fenêtre
                DependencyPropertyDescriptor.FromProperty(WindowStateProperty, typeof(Window)).RemoveValueChanged(this, OnWindowStateChanged);
                // Puis on update l'état de fenêtre
                WindowState = WindowState.Normal;
                WindowState = WindowState.Maximized;
                // Et on rajoute la méthode à la propriété de dépendance
                DependencyPropertyDescriptor.FromProperty(WindowStateProperty, typeof(Window)).AddValueChanged(this, OnWindowStateChanged);
            }
            else ResizeMode = ResizeMode.CanResizeWithGrip;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }
        private void Maximize_Click(object sender, RoutedEventArgs e) {
            // Mise à jour de l'état de la fenêtre
            WindowState =
                WindowState == WindowState.Normal ?
                WindowState.Maximized
              : WindowState.Normal;
        }
        private void Minimize_Click(object sender, RoutedEventArgs e) {
            // Mise à jour de l'état de la fenêtre
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region Utils
        /// <summary>
        /// Retoune l'index de l'écran dans lequel la fenêtre se trouve, dans Screen.AllScreens
        /// </summary>
        /// <returns></returns>
        private int getActualScreen() {
            int actualScreen = -1;
            // On cherche le premier écran dans lequel la fenêtre se trouve
            for (int i = 0; i < Screen.AllScreens.Length; i++) {
                Screen screen = Screen.AllScreens[i];
                // Si la fenêtre est contenue dans l'écran analysé actuellement, on associe l'itération actuelle de la boucle for à actualScreen, puis on sort de la boucle
                if (RestoreBounds.IntersectsWith(new Rect(screen.Bounds.X,
                                                          screen.Bounds.Y,
                                                          screen.Bounds.Width,
                                                          screen.Bounds.Height))) {
                    actualScreen = i;
                    break;
                }

            }
            //Si aucun écran n'a été trouvé, on lance une InvalidOperationException
            if (actualScreen == -1) throw new InvalidOperationException();
            return actualScreen;
        }
        #endregion

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Correction du masquage de la barre des tâches lors de la maximisation de la fenêtre.
        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Windows.Point lpPoint);
        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        private static IntPtr WindowProc(System.IntPtr hwnd,
                                                int msg,
                                                System.IntPtr wParam,
                                                System.IntPtr lParam,
                                                ref bool handled) {
            switch (msg) {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        void win_SourceInitialized(object sender, EventArgs e) {
            System.IntPtr handle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam) {

            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Ajuste la taille maximale et la position de la fenêtre pour remplir correctement l'espace de travail de l'écran correspondant
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero) {

                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            /// <summary>
            /// Coordonnée X
            /// </summary>
            public int x;
            /// <summary>
            /// Coordonnée Y
            /// </summary>
            public int y;

            /// <summary>
            /// Construction du point de coordonnées X, Y
            /// </summary>
            public POINT(int x, int y) {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        void win_Loaded(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Maximized;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO {         
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));          
            public RECT rcMonitor = new RECT();         
            public RECT rcWork = new RECT();          
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public static readonly RECT Empty = new RECT();

            public int Width {
                get { return Math.Abs(right - left); }  // Abs nécessaire pour BIDI OS
            }
            public int Height {
                get { return bottom - top; }
            }
            public RECT(int left, int top, int right, int bottom) {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc) {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }
            public bool IsEmpty {
                get {
                    // BUG : Sur BIDI OS (hébreu et arabique) left > right
                    return left >= right || top >= bottom;
                }
            }
            /// <summary> Retourne une information lisible sur l'instance de la structure </summary>
            public override string ToString() {
                if (this == RECT.Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary> Détermine si deux objets sont égaux (comparaison du type)</summary>
            public override bool Equals(object obj) {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }

            /// <summary>Retourne le HashCode de l'instance de la structure</summary>
            public override int GetHashCode() {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary> Détermine si deux rectangles sont indentiques</summary>
            public static bool operator ==(RECT rect1, RECT rect2) {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Détermine si deux rectangles sont différents (comparaison des membres)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) {
                return !(rect1 == rect2);
            }
        }
        #endregion

        #region implémentation des ajouts de nouvelles nodes via l'InsertNodeTool

        private void Img_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Image, x, y);
        }

        private void Add_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Add, x, y);
        }

        private void Sub_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Substract, x, y);
        }

        private void Mul_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Multiply, x, y);
        }

        private void Div_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Divide, x, y);
        }

        private void Ovr_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Over, x, y);
        }

        private void Inv_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Invert, x, y);
        }

        private void Con_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Constant, x, y);
        }

        private void Noi_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Noise, x, y);
        }

        private void Lrp_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.LinearRamp, x, y);
        }

        private void Rrp_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.RadialRamp, x, y);
        }

        private void Chn_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Channels, x, y);
        }

        private void Com_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.CombineChannels, x, y);
        }

        private void Grs_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.GrayScale, x, y);
        }
        private void Blr_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Blur, x, y);
        }

        private void Lum_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Luminosity, x, y);
        }

        private void Thr_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.Threshold, x, y);
        }

        private void Sel_Click(object sender, RoutedEventArgs e) {
            double x =  -  Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = - Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.ColorSelection, x, y);
        }


        private void Alh_Click(object sender, RoutedEventArgs e) {
            double x = -Editor.CurrentPosition.X + Editor.ActualWidth / 2;
            double y = -Editor.CurrentPosition.Y + Editor.ActualHeight / 2;
            SharedNodesAssembly.AddNode(NodeType.SetAlpha, x, y);
        }

        #endregion implémentation des ajouts de nouvelles nodes via l'InsertNodeTool

        private void Del_Click(object sender, RoutedEventArgs e) {
            SharedNodesAssembly.RemoveNode();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            Renderer.Assembly = SharedNodesAssembly;
            Thread thread = new Thread( new ThreadStart( Renderer.Render ));
            thread.Start();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".svp";
            dialog.Filter = "SVP Files (*.svp)|*.svp|All Files (*.*)|*.*";

            DialogResult Result = dialog.ShowDialog();
            if (Result == System.Windows.Forms.DialogResult.OK) {
                string filepath = dialog.FileName;

                BinaryFormatter reader = new BinaryFormatter();
                using (FileStream flux = new FileStream(filepath, FileMode.Open, FileAccess.Read)) {
                    flux.Seek(0, SeekOrigin.Begin);
                    SharedNodesAssembly = (NodesAssembly)reader.Deserialize(flux);
                    flux.Close();
                }
                SharedNodesAssembly.PropertyChanged += Properties.IdChanged;
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".svp";
            dialog.Filter = "SVP Files (*.svp)|*.svp";

            DialogResult Result = dialog.ShowDialog();
            if (Result == System.Windows.Forms.DialogResult.OK) {
                string filepath = dialog.FileName;

                BinaryFormatter writer = new BinaryFormatter();
                FileStream flux = null;
                try {
                    flux = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                    writer.Serialize(flux, SharedNodesAssembly);
                }
                catch {

                }
                finally {
                    if (flux != null)
                        flux.Close();
                }
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";

            DialogResult Result = dialog.ShowDialog();
            if (Result == System.Windows.Forms.DialogResult.OK) {
                string filepath = dialog.FileName;

                Image.Image image = Renderer.Result;
                Bitmap bmp = new Bitmap(image.Width, image.Height);
                for (int i = 0; i < image.Width * image.Height; i++) {
                    bmp.SetPixel(i % image.Width, (i / image.Width), Color.FromArgb(image.data[i].A, image.data[i].R, image.data[i].G, image.data[i].B));
                }
                switch (dialog.FilterIndex) {
                    case 1:
                        bmp.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        bmp.Save(filepath, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        bmp.Save(filepath, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
