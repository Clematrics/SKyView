using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {
    public class LogicalOutputPin {
        public LogicalOutputPin(string name) {
            this.name = name;
        }
        public string shader;
        public List<LogicalInputPin> target_pins;
        public string name;
    }
}
