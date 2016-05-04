using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Utils {
    public class Collection<T> : INotifyCollectionChanged, IEnumerable {
        public Collection(Func<T> new_object) {
            this.new_object = new_object;
        }

        private List<CollectionItem<T>> _list = new List<CollectionItem<T>>();
        public List<CollectionItem<T>> List {
            get { return _list; }
            set { if (_list != value) _list = value; }
        } 
        private Func<T> new_object;

        public void Add(T item) {
            _list.Add(new CollectionItem<T>(item, new_object));
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }
        public void Remove(T item) {
            _list.Remove(new CollectionItem<T>(item, new_object));
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }
        public void Add(CollectionItem<T> item) {
            _list.Add(item);
            RaiseCollectionChanged( new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item) );
        }
        public void Remove(CollectionItem<T> item) {
            _list.Remove(item);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        public T Find(Predicate<T> match) {
            foreach (CollectionItem<T> item in _list) {
                if (match(item.Member))
                    return item.Member;
            }
            return default(T);
        }

        public T this[Int32 index] {
            get { return _list[index].Member; }
        }
        public CollectionItem<T> AtIndex(Int32 index) {
             return _list[index];
        }

        #region INotifyCollectionChanged
        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args) {
            if (CollectionChanged != null)
                CollectionChanged(this, args);
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion

        #region IEnumerable
        public List<CollectionItem<T>>.Enumerator GetEnumerator() {
            return _list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator)GetEnumerator();
        }
        #endregion IEnumerable
    }

    public class CollectionItem<T> : CollectionItemBase {
        public CollectionItem(Func<T> new_object) {
            Member = new_object();
        }
        public CollectionItem(T item, Func<T> new_object) {
            Member = new_object();
            Member = item;
        }

        private T _member;
        public T Member {
            get { return _member; }
            set {
                _member = value;
                RaisePropertyChanged("_member");
            }
        }

    }

    public class CollectionItemBase : INotifyPropertyChanged {
        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
