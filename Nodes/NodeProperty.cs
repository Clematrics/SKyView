using System;
using System.ComponentModel;

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

    [Serializable]
    public class NodeProperty : INotifyPropertyChanged {
        public NodeProperty() {
        }
        public NodeProperty(string name, PropertyType type) {
            PropertyName = name;
            Type = type;
            if (type == PropertyType.FilePath) {
                MinValue = 0;
                MaxValue = 0;
            }
            else {
                MinValue = 0;
                MaxValue = 255;
            }
            Value = "0";
        }
        public NodeProperty(string name, PropertyType type, int min, int max) {
            PropertyName = name;
            Type = type;
            MinValue = min;
            MaxValue = max;
        }

        public string PropertyName {
            get { return _PropertyName; }
            set { _PropertyName = value; RaisePropertyChanged("PropertyName"); }
        }
        public PropertyType Type {
            get { return _Type; }
            set { _Type = value; RaisePropertyChanged("Type"); }
        }
        public int MinValue {
            get { return _MinValue; }
            set { _MinValue = value; RaisePropertyChanged("MinValue"); }
        }
        public int MaxValue {
            get { return _MaxValue; }
            set { _MaxValue = value; RaisePropertyChanged("MaxValue"); }
        }
        public string Value {
            get { return _Value; }
            set { _Value = value; RaisePropertyChanged("Value"); }
        }

        private string _PropertyName;
        private PropertyType _Type;
        private int _MinValue;
        private int _MaxValue;
        private string _Value;

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
