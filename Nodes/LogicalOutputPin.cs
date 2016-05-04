using System.Collections.Generic;
using SkyView.Image;

namespace SkyView.Nodes {
    public class LogicalOutputPin {
        public LogicalOutputPin(string name, Filter filter) {
            this.filter = filter;
            this.name = name;
        }

        public Filter filter;
        public string shader;
        public List<LogicalInputPin> target_pins;
        public string name;
    }
}
