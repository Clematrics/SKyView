using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {

    [Serializable]
    public class LogicalLink {
        public LogicalInputPin Input { get; set; }
        public LogicalOutputPin Output { get; set; }
    }
}
