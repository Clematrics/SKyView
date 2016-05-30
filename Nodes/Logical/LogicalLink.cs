using SkyView.Image;
using System;
using System.ComponentModel;

namespace SkyView.Nodes {

    [Serializable]
    public class LogicalLink : INotifyPropertyChanged {
        public LogicalLink() { Input = new LogicalInputPin(null, ""); Output = new LogicalOutputPin(null, "", Filters.NoFilter); }
        public LogicalLink(LogicalInputPin input) { Input = input; Output = new LogicalOutputPin(null, "", Filters.NoFilter); }
        public LogicalLink(LogicalOutputPin output) { Output = output; Input = new LogicalInputPin(null, ""); }

        public LogicalInputPin Input {
            get { return _Input;  }
            set { _Input = value; RaisePropertyChanged("Input"); }
        }
        public LogicalOutputPin Output {
            get { return _Output; }
            set { _Output = value; RaisePropertyChanged("Output"); }
        }

        private LogicalInputPin _Input;
        private LogicalOutputPin _Output;

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
