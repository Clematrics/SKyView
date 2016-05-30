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

        private Collection<LogicalNode> _NodesCollection;
        private Collection<LogicalLink> _LinksCollection;
        private long _IdSelected;

        public void AddNode(NodeType type, double x, double y) {
            LogicalNode newLogicalNode = new LogicalNode(type, x, y);
            NodesCollection.Add(newLogicalNode);
            IdSelected = newLogicalNode.Id;
        }

        public void RemoveNode() {
            int? index = NodesCollection.FindAtIndex(x => x.Id == IdSelected && x.Type != NodeType.Output);
            if (index == null) return;
            LogicalNode node = NodesCollection[ index ];
            foreach (CollectionItem<LogicalInputPin> input in node.InputPins)
                Divorce(input.Member.SourcePin);
            foreach (CollectionItem<LogicalOutputPin> output in node.OutputPins)
                foreach (CollectionItem<LogicalLink> link in output.Member.TargetPins)
                    Divorce(link.Member);
            NodesCollection.Remove( index );
            IdSelected = 0;
        }

        public void Love(LogicalInputPin input, LogicalOutputPin output) {

            int? index = LinksCollection.FindAtIndex(x => x.Input == input);
            if (index != null)
                LinksCollection.Remove( (int)index );
            
            LogicalLink link = new LogicalLink();
            link.Input = input;
            link.Output = output;
            LinksCollection.Add(link);
            input.SourcePin = link;
            output.TargetPins.Add(link);
        }

        public void Divorce(LogicalLink link) {
            LinksCollection.Remove( LinksCollection.FindAtIndex( x => x == link));
        }

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
