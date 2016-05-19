using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SkyView.Nodes {

    [Serializable]
    public class LogicalInputPin : INotifyPropertyChanged {
        public LogicalInputPin(LogicalNode parent, string name) {
            Parent = parent;
            Name = name;
        }

        public LogicalNode Parent;

        public LogicalLink SourcePin {
            get { return _SourcePin; }
            set { _SourcePin = value; RaisePropertyChanged("SourcePin"); }
        }
        public string Name {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged("Name"); }
        }
        public Point Coordinates {
            get { return _Coordinates; }
            set { _Coordinates = value; RaisePropertyChanged("Coordinates"); }
        }

        private LogicalLink _SourcePin;
        private string _Name;
        private Point _Coordinates;

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
