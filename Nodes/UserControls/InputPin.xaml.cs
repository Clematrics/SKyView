using SkyView.Utils;
using System.Windows;
using System.Windows.Controls;

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
            new PropertyMetadata(new LogicalInputPin("")));

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
    }
}
