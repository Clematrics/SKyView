using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SkyView.Utils {

    [Serializable]
    public class Collection<T> : INotifyCollectionChanged, INotifyPropertyChanged, IEnumerable {
        public Collection(Func<T> new_object) {
            this.new_object = new_object;
        }

        private ObservableCollection<CollectionItem<T>> _list = new ObservableCollection<CollectionItem<T>>();
        public ObservableCollection<CollectionItem<T>> List {
            get { return _list; }
            set { _list = value; RaisePropertyChanged("List"); }
        } 
        private Func<T> new_object;

        public void Add(T item) {
            List.Add(new CollectionItem<T>(item, new_object));
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }
        public void Remove(T item) {
            List.Remove(new CollectionItem<T>(item, new_object));
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }
        public void Remove(int? index) {
            if (List.Count == 0 || List.Count < index || index < 0 || index == null) return;
            CollectionItem<T> item = List[(int)index];
            List.RemoveAt((int)index);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }
        public void Add(CollectionItem<T> item) {
            List.Add(item);
            RaiseCollectionChanged( new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item) );
        }
        public void Remove(CollectionItem<T> item) {
            List.Remove(item);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        public T Find(Predicate<T> match) {
            foreach (CollectionItem<T> item in List) {
                if (match(item.Member))
                    return item.Member;
            }
            return default(T);
        }

        public int? FindAtIndex(Predicate<T> match) {
            int index = 0;
            foreach (CollectionItem<T> item in List) {
                if (match(item.Member))
                    return index;
                else index++;
            }
            return null;
        }

        public T this[int? index] {
            get { if (index != null) return List[(int)index].Member; return default(T); }
        }
        public CollectionItem<T> AtIndex(int index) {
             return List[index];
        }

        #region INotifyCollectionChanged
        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args) {
            CollectionChanged?.Invoke(this, args);
        }
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IEnumerable
        public IEnumerator<CollectionItem<T>> GetEnumerator() {
            return List.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion IEnumerable
    }

    [Serializable]
    public class CollectionItem<T> : CollectionItemBase {
        public CollectionItem(Func<T> new_object) {
            Member = new_object();
        }
        public CollectionItem(T item, Func<T> new_object) {
            Member = new_object();
            Member = item;
        }

        private T _Member;
        public T Member {
            get { return _Member; }
            set {
                _Member = value;
                RaisePropertyChanged("Member");
            }
        }
    }

    [Serializable]
    public class CollectionItemBase : INotifyPropertyChanged {
        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
