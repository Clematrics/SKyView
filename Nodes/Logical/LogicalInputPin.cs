using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {

    [Serializable]
    public class LogicalInputPin : INotifyPropertyChanged {
        public LogicalInputPin(string name) {
            Name = name;
        }

        public LogicalLink SourcePin {
            get { return _SourcePin; }
            set { _SourcePin = value; RaisePropertyChanged("SourcePin"); }
        }
        public string Name {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged("Name"); }
        }

        private LogicalLink _SourcePin;
        private string _Name;

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
