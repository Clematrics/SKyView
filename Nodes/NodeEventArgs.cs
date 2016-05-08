using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {
    public class NodeEventArgs : EventArgs {
        public NodeEventArgs(NodeType type) {
            this.type = type;
        }
        public NodeType type;
    }
}
