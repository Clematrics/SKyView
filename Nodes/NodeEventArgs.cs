using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {
    public class NodeEventArgs : EventArgs {
        public NodeEventArgs(long id, NodeType type) {
            this.id = id;
            this.type = type;
        }
        public long id;
        public NodeType type;
    }
}
