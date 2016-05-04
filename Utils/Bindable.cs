using System;
using System.ComponentModel;

namespace SkyView.Utils {

    public class Bindable<T> : INotifyPropertyChanged{

        public Bindable() { }
        //Constructeur et initialisation de _Value par expression lambda
        public Bindable(Func<T> new_object) {
            _Value = new_object();
        }

        private T _Value;
        public T Value {
            get {
                return _Value;
            }
            set {
                _Value = value;
                RaisePropertyChanged("Value");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void RaisePropertyChanged(string property) {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

}
