namespace SkyView.Nodes {

    public enum DragInfo {
        FromInput,
        FromOutput,
        ToInput,
        ToOutput
    }

    public enum PropertyType {
        FilePath,
        Number
    }

    public class NodeProperty {
        public NodeProperty() {
        }
        public NodeProperty(string name, PropertyType type) {
            PropertyName = name;
            this.type = type;
            if (type == PropertyType.FilePath) {
                minValue = 0;
                maxValue = 0;
            }
            else {
                minValue = 0;
                maxValue = 255;
            }
            value = "0";
        }
        public NodeProperty(string name, PropertyType type, int min, int max) {
            PropertyName = name;
            this.type = type;
            minValue = min;
            maxValue = max;
        }

        public string PropertyName { get; set; }
        public PropertyType type { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public string value { get; set; }
    }

}
