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
    }
}
