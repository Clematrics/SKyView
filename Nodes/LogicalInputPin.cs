using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {

    public class LogicalInputPin {
        public LogicalInputPin(string name) {
            this.name = name;
        }
        public LogicalOutputPin source_pin;
        public string name;
    }

}
