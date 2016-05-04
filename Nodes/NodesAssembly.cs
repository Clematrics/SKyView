using SkyView.Utils;
using System.Collections.Generic;
using System.ComponentModel;

namespace SkyView.Nodes {

    public class NodesAssembly : INotifyPropertyChanged {

        public NodesAssembly() {
            nodes_collection = new Collection<LogicalNode>( () => new LogicalNode() );
            idSelected = 0;
        }

        public Collection<LogicalNode> nodes_collection { get; set; }
        private long _idSelected;
        public long idSelected {
            get { return _idSelected; }
            set { if (_idSelected != value) {
                    _idSelected = value;
                    RaisePropertyChanged("idSelected");
                }
            }
        }

        public void AddNode(NodeType type) {
            LogicalNode newLogicalNode = new LogicalNode(type);
            nodes_collection.Add(newLogicalNode);
            idSelected = newLogicalNode.id;
            OnNodeAdded(this, new NodeEventArgs(idSelected, type));
        }

        public void RemoveNode() {
            OnNodeRemoved(this, new NodeEventArgs(idSelected, NodeType.Unknown));
            nodes_collection.Remove(
                nodes_collection.Find(x => x.id == idSelected));
        }

        public event NodeEventHandler OnNodeAdded;
        public event NodeEventHandler OnNodeRemoved;
        public delegate void NodeEventHandler(object sender, NodeEventArgs e);

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
