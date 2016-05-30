using SkyView.Image;
using SkyView.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SkyView.Nodes {

    [Serializable]
    public class LogicalNode : INotifyPropertyChanged {

        public LogicalNode() {
            Id = DateTime.Now.ToBinary();
        }
        public LogicalNode(NodeType type, double x, double y) {
            Id = DateTime.Now.ToBinary();
            Type = type;
            Name = getNameFromType(type);
            Properties = getPropertiesFromType(type);
            InputPins = getInputPinsFromType(type);
            OutputPins = getOutputPinsFromType(type);
            X = x; Y = y; Z = 0; IsSelected = false;
        }

        public Collection<NodeProperty> Properties {
            get { return _Properties; }
            set { _Properties = value; RaisePropertyChanged("Properties"); }
        }
        public Collection<LogicalInputPin> InputPins {
            get { return _InputPins; }
            set { _InputPins = value; RaisePropertyChanged("InputPins"); }
        }
        public Collection<LogicalOutputPin> OutputPins {
            get { return _OutputPins; }
            set { _OutputPins = value; RaisePropertyChanged("OutputPins"); }
        }
        public string Name {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged("Name"); }
        }
        public long Id {
            get { return _Id; }
            set { _Id = value; RaisePropertyChanged("Id"); }
        }
        public bool IsSelected {
            get { return _IsSelected; }
            set { _IsSelected = value; RaisePropertyChanged("IsSelected"); }
        }
        public NodeType Type {
            get { return _Type; }
            set { _Type = value; RaisePropertyChanged("Type"); }
        }
        public double X {
            get { return _X; }
            set { _X = value; RaisePropertyChanged("X"); }
        }
        public double Y {
            get { return _Y; }
            set { _Y = value; RaisePropertyChanged("Y"); }
        }
        public int Z {
            get { return _Z; }
            set { _Z = value; RaisePropertyChanged("Z"); }
        }

        private Collection<NodeProperty> _Properties;
        private Collection<LogicalInputPin> _InputPins;
        private Collection<LogicalOutputPin> _OutputPins;
        private string _Name;
        private long _Id;
        private bool _IsSelected;
        private NodeType _Type;
        private double _X;
        private double _Y;
        private int _Z;

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

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
                    newProperties.Add(new NodeProperty("Width", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Height", PropertyType.Number));
                    break;
                case NodeType.Add:
                    break;
                case NodeType.Substract:
                    break;
                case NodeType.Multiply:
                    break;
                case NodeType.Divide:
                    break;
                case NodeType.Over:
                    break;
                case NodeType.Invert:
                    break;
                case NodeType.Constant:
                    newProperties.Add(new NodeProperty("Red constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Green constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Blue constant", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Alpha constant", PropertyType.Number));
                    break;
                case NodeType.Noise:
                    newProperties.Add(new NodeProperty("Seed", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Octaves", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Persistence", PropertyType.Number));
                    newProperties.Add(new NodeProperty("X Offset", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Y Offset", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Depth", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Tile size", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Unit", PropertyType.Number));
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
                case NodeType.CombineChannels:
                    break;
                case NodeType.GrayScale:
                    break;
                case NodeType.Blur:
                    newProperties.Add(new NodeProperty("Horizontal blur", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Vertical blur", PropertyType.Number));
                    newProperties.Add(new NodeProperty("Method", PropertyType.Number));
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
                case NodeType.SetAlpha:
                    newProperties.Add(new NodeProperty("Alpha", PropertyType.Number));
                    break;
                default:
                    return newProperties;
            }
            return newProperties;
        }

        public Collection<LogicalInputPin> getInputPinsFromType(NodeType type) {
            Collection<LogicalInputPin> newInputPins = new Collection<LogicalInputPin>(() => new LogicalInputPin(this, ""));
            switch (type) {
                case NodeType.Unknown:
                    break;
                case NodeType.Image:
                    break;
                case NodeType.Output:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                case NodeType.Add:
                    newInputPins.Add(new LogicalInputPin(this, "A"));
                    newInputPins.Add(new LogicalInputPin(this, "B"));
                    break;
                case NodeType.Substract:
                    newInputPins.Add(new LogicalInputPin(this, "A"));
                    newInputPins.Add(new LogicalInputPin(this, "B"));
                    break;
                case NodeType.Multiply:
                    newInputPins.Add(new LogicalInputPin(this, "A"));
                    newInputPins.Add(new LogicalInputPin(this, "B"));
                    break;
                case NodeType.Divide:
                    newInputPins.Add(new LogicalInputPin(this, "A"));
                    newInputPins.Add(new LogicalInputPin(this, "B"));
                    break;
                case NodeType.Over:
                    newInputPins.Add(new LogicalInputPin(this, "A"));
                    newInputPins.Add(new LogicalInputPin(this, "B"));
                    break;
                case NodeType.Invert:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
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
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                case NodeType.CombineChannels:
                    newInputPins.Add(new LogicalInputPin(this, "Red"));
                    newInputPins.Add(new LogicalInputPin(this, "Green"));
                    newInputPins.Add(new LogicalInputPin(this, "Blue"));
                    newInputPins.Add(new LogicalInputPin(this, "Alpha"));
                    break;
                case NodeType.GrayScale:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                case NodeType.Blur:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                case NodeType.Luminosity:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                case NodeType.Threshold:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                case NodeType.ColorSelection:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                case NodeType.SetAlpha:
                    newInputPins.Add(new LogicalInputPin(this, "Image"));
                    break;
                default:
                    break;
            }
            return newInputPins;
        }

        public Collection<LogicalOutputPin> getOutputPinsFromType(NodeType type) {
            Collection<LogicalOutputPin> newOutputPins = new Collection<LogicalOutputPin>(() => new LogicalOutputPin(this, "", Filters.NoFilter));
            switch (type) {
                case NodeType.Unknown:
                    break;
                case NodeType.Image:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.LoadImage));
                    break;
                case NodeType.Output:
                    break;
                case NodeType.Add:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.AddFilter));
                    break;
                case NodeType.Substract:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.SubstractFilter));
                    break;
                case NodeType.Multiply:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.MultiplyFilter));
                    break;
                case NodeType.Divide:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.DivideFilter));
                    break;
                case NodeType.Over:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.OverlayFilter));
                    break;
                case NodeType.Invert:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.InvertFilter));
                    break;
                case NodeType.Constant:
                    newOutputPins.Add(new LogicalOutputPin(this, "Constant", Filters.ConstantFilter));
                    break;
                case NodeType.Noise:
                    newOutputPins.Add(new LogicalOutputPin(this, "Noise", Filters.NoiseFilter));
                    break;
                case NodeType.LinearRamp:
                    newOutputPins.Add(new LogicalOutputPin(this, "Ramp", Filters.LinearRampFilter));
                    break;
                case NodeType.RadialRamp:
                    newOutputPins.Add(new LogicalOutputPin(this, "Ramp", Filters.RadialRampFilter));
                    break;
                case NodeType.Channels:
                    newOutputPins.Add(new LogicalOutputPin(this, "Red", Filters.GetRedChannel));
                    newOutputPins.Add(new LogicalOutputPin(this, "Green", Filters.GetGreenChannel));
                    newOutputPins.Add(new LogicalOutputPin(this, "Blue", Filters.GetBlueChannel));
                    newOutputPins.Add(new LogicalOutputPin(this, "Alpha", Filters.GetAlphaChannel));
                    break;
                case NodeType.CombineChannels:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.CombineChannels));
                    break;
                case NodeType.GrayScale:
                    newOutputPins.Add(new LogicalOutputPin(this, "GrayScale", Filters.GrayScaleFilter));
                    break;
                case NodeType.Blur:
                    newOutputPins.Add(new LogicalOutputPin(this, "Blur", Filters.BlurFilter));
                    break;
                case NodeType.Luminosity:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.LuminosityFilter));
                    break;
                case NodeType.Threshold:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.ThresholdFilter));
                    break;
                case NodeType.ColorSelection:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.ColorSelectionFilter));
                    break;
                case NodeType.SetAlpha:
                    newOutputPins.Add(new LogicalOutputPin(this, "Image", Filters.SetAlpha));
                    break;
                default:
                    break;
            }
            return newOutputPins;
        }

    }
}
