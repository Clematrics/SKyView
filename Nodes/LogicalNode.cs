using SkyView.Utils;
using System;
using System.Collections.Generic;

namespace SkyView.Nodes {

    public class LogicalNode {

        public LogicalNode() {
            id = DateTime.Now.ToBinary();
        }
        public LogicalNode(NodeType type) {
            id = DateTime.Now.ToBinary();
            this.type = type;
            name = getNameFromType(type);
            properties = getPropertiesFromType(type);
            inPins = getInputPinsFromType(type);
            outPins = getOutputPinsFromType(type);
        }

        public Collection<NodeProperty> properties { get; set; }
        public Collection<LogicalInputPin> inPins { get; set; }
        public Collection<LogicalOutputPin> outPins { get; set; }
        public string name { get; set; }
        public long id { get; set; }
        public NodeType type { get; }

        public string getNameFromType(NodeType type) {
            return type.ToString();
        }

        public Collection<NodeProperty> getPropertiesFromType(NodeType type) {
            Collection<NodeProperty> newProperties = new Collection<NodeProperty>(() => new NodeProperty());
            switch (type) {
                case NodeType.Unknown:
                    break;
                case NodeType.Image:
                    newProperties.Add(new NodeProperty("File path", PropertyType.FilePath));
                    break;
                case NodeType.Output:
                    break;
                case NodeType.Add:
                    break;
                case NodeType.Substract:
                    break;
                case NodeType.Multiply:
                    break;
                case NodeType.Divide:
                    break;
                case NodeType.Blend:
                    break;
                case NodeType.Invert:
                    break;
                case NodeType.Replace:
                    break;
                case NodeType.Constant:
                    newProperties.Add(new NodeProperty("Red constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Green constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Blue constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Alpha constant", PropertyType.Number));
                    break;
                case NodeType.Noise:
                    newProperties.Add(new NodeProperty("Seed", PropertyType.Number));
                    break;
                case NodeType.LinearRamp:
                    newProperties.Add(new NodeProperty("White pixel X", PropertyType.Number));
                    newProperties.Add(new NodeProperty("White pixel Y", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Black pixel X", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Black pixel Y", PropertyType.Number));
                    break;
                case NodeType.RadialRamp:
                    newProperties.Add(new NodeProperty("White pixel X", PropertyType.Number));
                    newProperties.Add(new NodeProperty("White pixel Y", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Black pixel X", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Black pixel Y", PropertyType.Number));
                    break;
                case NodeType.Channels:
                    break;
                case NodeType.Blur:
                    newProperties.Add(new NodeProperty("Horizontal blur", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Vertical blur", PropertyType.Number));
                    break;
                case NodeType.Luminosity:
                    newProperties.Add(new NodeProperty("Luminosity", PropertyType.Number));
                    break;
                case NodeType.Threshold:
                    newProperties.Add(new NodeProperty("Threshold", PropertyType.Number));
                    break;
                case NodeType.ColorSelection:
                    newProperties.Add(new NodeProperty("Red constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Green constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Blue constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Alpha constant", PropertyType.Number));
                    break;
                default:
                    return newProperties;
            }
            return newProperties;
        }

        public Collection<LogicalInputPin> getInputPinsFromType(NodeType type) {
            Collection<LogicalInputPin> newInputPins = new Collection<LogicalInputPin>(() => new LogicalInputPin(""));
            switch (type) {
                case NodeType.Unknown:
                    break;
                case NodeType.Image:
                    break;
                case NodeType.Output:
                    newInputPins.Add(new LogicalInputPin("Image"));
                    break;
                case NodeType.Add:
                    newInputPins.Add(new LogicalInputPin("A"));
                    newInputPins.Add(new LogicalInputPin("B"));
                    break;
                case NodeType.Substract:
                    newInputPins.Add(new LogicalInputPin("A"));
                    newInputPins.Add(new LogicalInputPin("B"));
                    break;
                case NodeType.Multiply:
                    newInputPins.Add(new LogicalInputPin("A"));
                    newInputPins.Add(new LogicalInputPin("B"));
                    break;
                case NodeType.Divide:
                    newInputPins.Add(new LogicalInputPin("A"));
                    newInputPins.Add(new LogicalInputPin("B"));
                    break;
                case NodeType.Blend:
                    newInputPins.Add(new LogicalInputPin("A"));
                    newInputPins.Add(new LogicalInputPin("B"));
                    break;
                case NodeType.Invert:
                    newInputPins.Add(new LogicalInputPin("Image"));
                    break;
                case NodeType.Replace:
                    newInputPins.Add(new LogicalInputPin("Image"));
                    newInputPins.Add(new LogicalInputPin("Mask"));
                    break;
                case NodeType.Constant:
                    break;
                case NodeType.Noise:
                    break;
                case NodeType.LinearRamp:
                    break;
                case NodeType.RadialRamp:
                    break;
                case NodeType.Channels:
                    break;
                case NodeType.Blur:
                    break;
                case NodeType.Luminosity:
                    newInputPins.Add(new LogicalInputPin("Image"));
                    break;
                case NodeType.Threshold:
                    newInputPins.Add(new LogicalInputPin("Image"));
                    break;
                case NodeType.ColorSelection:
                    newInputPins.Add(new LogicalInputPin("Image"));
                    break;
                default:
                    break;
            }
            return newInputPins;
        }

        public Collection<LogicalOutputPin> getOutputPinsFromType(NodeType type) {
            Collection<LogicalOutputPin> newOutputPins = new Collection<LogicalOutputPin>(() => new LogicalOutputPin(""));
            switch (type) {
                case NodeType.Unknown:
                    break;
                case NodeType.Image:
                    newOutputPins.Add( new LogicalOutputPin("Image") );
                    break;
                case NodeType.Output:
                    break;
                case NodeType.Add:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Substract:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Multiply:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Divide:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Blend:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Invert:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Replace:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Constant:
                    newOutputPins.Add(new LogicalOutputPin("Constant"));
                    break;
                case NodeType.Noise:
                    newOutputPins.Add(new LogicalOutputPin("Noise"));
                    break;
                case NodeType.LinearRamp:
                    newOutputPins.Add(new LogicalOutputPin("Ramp"));
                    break;
                case NodeType.RadialRamp:
                    newOutputPins.Add(new LogicalOutputPin("Ramp"));
                    break;
                case NodeType.Channels:
                    newOutputPins.Add(new LogicalOutputPin("Red"));
                    newOutputPins.Add(new LogicalOutputPin("Green"));
                    newOutputPins.Add(new LogicalOutputPin("Blue"));
                    newOutputPins.Add(new LogicalOutputPin("Alpha"));
                    break;
                case NodeType.Blur:
                    newOutputPins.Add(new LogicalOutputPin("Blur"));
                    break;
                case NodeType.Luminosity:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.Threshold:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                case NodeType.ColorSelection:
                    newOutputPins.Add(new LogicalOutputPin("Image"));
                    break;
                default:
                    break;
            }
            return newOutputPins;
        }

    }
}
