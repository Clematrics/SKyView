using SkyView.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SkyView.Nodes {
    /// <summary>
    /// Logique d'interaction pour InputPin.xaml
    /// </summary>
    public partial class InputPin : UserControl {

        public InputPin() { InitializeComponent(); GlobalInputPin.DataContext = this; }

        public LogicalInputPin InputPinData {
            get { return (LogicalInputPin)GetValue(InputPinDataProperty); }
            set { SetValue(InputPinDataProperty, value); }
        }
        public static readonly DependencyProperty InputPinDataProperty = DependencyProperty.Register(
            "InputPinData",
            typeof(LogicalInputPin),
            typeof(InputPin),
            new PropertyMetadata(new LogicalInputPin(null, "")));

        public int Index {
            get { return (int)GetValue(InputIndexProperty); }
            set { SetValue(InputIndexProperty, value); }
        }
        public static readonly DependencyProperty InputIndexProperty = DependencyProperty.Register(
            "Index",
            typeof(int),
            typeof(InputPin),
            new PropertyMetadata(-1));

        public event PinSelectionEventHandler PinSelected;
        private void slot_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            PinSelected?.Invoke(this, PinType.Input, Index);
        }

        public void UpdatePositionData(object sender, EventArgs e) {
            Canvas ParentCanvas = FindParent<Canvas>(this);
            Point RelativePosition = slot.TransformToAncestor(ParentCanvas).Transform(new Point(4, 5));
            InputPinData.Coordinates = RelativePosition;
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
