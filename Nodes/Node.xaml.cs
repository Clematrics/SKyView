using SkyView.Nodes;
using System.Collections.Generic;
using System.Windows.Controls;

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
            l_node.name = (sender as TextBox).Text;
        }
    }
}
