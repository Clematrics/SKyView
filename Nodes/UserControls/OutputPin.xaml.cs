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

        public int Index {
            get { return (int)GetValue(OutputIndexProperty); }
            set { SetValue(OutputIndexProperty, value); }
        }
        public static readonly DependencyProperty OutputIndexProperty = DependencyProperty.Register(
            "Index",
            typeof(int),
            typeof(OutputPin),
            new PropertyMetadata( -1 ));

        public event PinSelectionEventHandler PinSelected;
        private void slot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            PinSelected?.Invoke(this, PinType.Output, Index);
        }

        public void UpdatePositionData(object sender, EventArgs e) {
            Canvas ParentCanvas = FindParent<Canvas>(this);
            Point RelativePosition = slot.TransformToAncestor(ParentCanvas).Transform(new Point(4, 5));
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
    }
}
