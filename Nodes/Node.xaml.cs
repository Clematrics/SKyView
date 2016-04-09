using SkyView.Nodes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyView {
    /// <summary>
    /// Logique d'interaction pour Node.xaml
    /// </summary>
    public partial class Node : UserControl {
        public Node() {
            InitializeComponent();
        }

        class LogicalNode {
            internal List<NodeProperty> properties;
            internal List<LogicalInputPin> inPins;
            internal List<LogicalOutputPin> outPins;
            internal string name;
        };
        LogicalNode l_node;

        class LogicalOutputPin {
            LogicalOutputPin() {

            }
            string shader;
            List<LogicalInputPin> target_pins;
            string name;
        }
        class LogicalInputPin {
            LogicalOutputPin source_pin;
            string name;
        }

        private void TitleContainer_TextChanged(object sender, TextChangedEventArgs e) {
            //l_node.name = (sender as TextBox).Text;
        }


        #region Gestion du déplacement de la node

        private Point currentGraphPoint;
        private Point offset;
        private bool IsGraphMoving = false;

        private void Navigator_MouseDown(object sender, MouseButtonEventArgs e) {
            Navigator.CaptureMouse();
            offset = Mouse.GetPosition(this);
            IsGraphMoving = true;
        }

        private void Navigator_MouseUp(object sender, MouseButtonEventArgs e) {
            Navigator.ReleaseMouseCapture();
            IsGraphMoving = false;
        }

        private void Navigator_MouseMove(object sender, MouseEventArgs e) {
            if (!IsGraphMoving) return;

            currentGraphPoint = Mouse.GetPosition(Parent as UIElement) - (Vector)offset;

            if (currentGraphPoint.X < 0)
                currentGraphPoint.X = 0;
            if (currentGraphPoint.X > 4096 - ActualWidth)
                currentGraphPoint.X = 4096 - ActualWidth;
            if (currentGraphPoint.Y < 0)
                currentGraphPoint.Y = 0;
            if (currentGraphPoint.Y > 4096 - ActualHeight)
                currentGraphPoint.Y = 4096 - ActualHeight;
            Canvas.SetTop(this, currentGraphPoint.Y);
            Canvas.SetLeft(this, currentGraphPoint.X);

        }
        #endregion
    }
}
