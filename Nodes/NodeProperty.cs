using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {
    public enum PropertyType {
        FileAdress,
        Number
    }

    class NodeProperty {
        NodeProperty(string name, PropertyType type) {
            PropertyName = name;
            this.type = type;
            if (type == PropertyType.FileAdress) {
                minValue = 0;
                maxValue = 0;
            }
            else {
                minValue = 0;
                maxValue = 255;
            }
        }
        NodeProperty(string name, PropertyType type, int min, int max) {
            PropertyName = name;
            this.type = type;
            minValue = min;
            maxValue = max;
        }

        string PropertyName;
        PropertyType type;
        int minValue, maxValue;
        int value = 0;
        string adress_value = "";
    }
}
