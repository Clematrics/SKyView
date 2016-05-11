using SkyView.Utils;
using System;
using System.ComponentModel;
using System.Drawing;

namespace SkyView.Nodes {

    [Serializable]
    public class NodesAssembly : INotifyPropertyChanged {

        public NodesAssembly() {
            NodesCollection = new Collection<LogicalNode>( () => new LogicalNode() );
            IdSelected = 0;
        }

        public Collection<LogicalNode> NodesCollection {
            get { return _NodesCollection; }
            set { _NodesCollection = value; RaisePropertyChanged("NodesCollection"); }
        }
        public Collection<LogicalLink> LinksCollection {
            get { return _LinksCollection; }
            set { _LinksCollection = value; RaisePropertyChanged("LinksCollection"); }
        }
        public long IdSelected {
            get { return _IdSelected; }
            set { _IdSelected = value; RaisePropertyChanged("IdSelected"); }
        }
        public Size ProjectSize {
            get { return _ProjectSize; }
            set { _ProjectSize = value; RaisePropertyChanged("ProjectSize"); }
        }

        private Collection<LogicalNode> _NodesCollection;
        private Collection<LogicalLink> _LinksCollection;
        private long _IdSelected;
        private Size _ProjectSize;

        public void AddNode(NodeType type, double x, double y) {
            LogicalNode newLogicalNode = new LogicalNode(type, x, y);
            NodesCollection.Add(newLogicalNode);
            IdSelected = newLogicalNode.Id;
        }

        public void RemoveNode() {
            NodesCollection.Remove(
                NodesCollection.FindAtIndex(x => x.Id == IdSelected && x.Type != NodeType.Output));
            IdSelected = 0;
        }

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
