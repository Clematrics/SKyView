using SkyView.Image;
using SkyView.Utils;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

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
            new PropertyMetadata(new LogicalOutputPin("", Filters.NoFilter)));

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
    }
}
