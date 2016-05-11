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
    }
}
