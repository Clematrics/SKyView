using SkyView.Image;
using SkyView.Utils;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SkyView.Nodes {
    /// <summary>
    /// Logique d'interaction pour OutputPin.xaml
    /// </summary>
    public partial class OutputPin : UserControl {

        public OutputPin() { InitializeComponent(); GlobalOutputPin.DataContext = this; }

        public LogicalOutputPin OutputPinData {
            get { return (LogicalOutputPin)GetValue(OutputPinDataProperty); }
            set { SetValue(OutputPinDataProperty, value); }
        }
        public static readonly DependencyProperty OutputPinDataProperty = DependencyProperty.Register(
            "OutputPinData",
            typeof(LogicalOutputPin),
            typeof(OutputPin),
            new PropertyMetadata(new LogicalOutputPin(null, "", Filters.NoFilter)));

        public void UpdatePositionData(object sender, Vector vector) {
            Canvas ParentCanvas = FindParent<Canvas>(this);
            Point RelativePosition = slot.TransformToAncestor(ParentCanvas).Transform(new Point(8, 5));
            OutputPinData.Coordinates = RelativePosition;
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

        public event LinkDraggingEventHandler BeginDrag;
        private void slot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                BeginDrag?.Invoke(this, PinType.Output);
        }

        public event LinkDraggingEventHandler EndDrag;
        public void ForceEndDragging() {
            EndDrag?.Invoke(this, PinType.Output);
        }
    }
}
