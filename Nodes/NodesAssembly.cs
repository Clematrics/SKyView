using SkyView.Utils;
using System;
using System.ComponentModel;
using System.Drawing;

namespace SkyView.Nodes {

    [Serializable]
    public class NodesAssembly : INotifyPropertyChanged {

        public NodesAssembly() {
            NodesCollection = new Collection<LogicalNode>( () => new LogicalNode() );
            LinksCollection = new Collection<LogicalLink>( () => new LogicalLink() );
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

        public void Love(LogicalInputPin input, int indexInput, LogicalOutputPin output, int indexOutput) {
            LogicalLink link = new LogicalLink();
            link.Input = input;
            link.Output = output;
            LinksCollection.Add(link);
            if (input.SourcePin != null) {
                LinksCollection.Remove( LinksCollection.FindAtIndex( x => x.Input == input) );
            }
            input.SourcePin = link;
            output.TargetPins.Add(link);
        }

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
