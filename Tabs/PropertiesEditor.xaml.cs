using SkyView.Nodes;
using SkyView.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SkyView.Tabs {

    /// <summary>
    /// Logique d'interaction pour PropertiesEditor.xaml
    /// </summary>
    public partial class PropertiesEditor : UserControl {
        public PropertiesEditor() {
            properties = new Collection<NodeProperty>( () => new NodeProperty() );
            nodes_collection = new Collection<LogicalNode>( () => new LogicalNode() );
            idSelected = 0;

            idSelected_descriptor.AddValueChanged(this, idSelected_PropertyChanged);
            properties_descriptor.AddValueChanged(this, properties_PropertyChanged);

            InitializeComponent();

            ItemsControlScroller.DataContext = this;
        }

        private void properties_PropertyChanged(object sender, EventArgs e) {
            if (idSelected != 0) nodes_collection.Find(x => x.id == idSelected).properties = properties;
        }

        private void idSelected_PropertyChanged(object sender, EventArgs e) {
                    if (idSelected == 0)
                        properties = new Collection<NodeProperty>( () => new NodeProperty() );
                    else properties = nodes_collection.Find(x => x.id == idSelected).properties;
        }

        #region properties
        public Collection<NodeProperty> properties {
            get { return (Collection<NodeProperty>)GetValue(properties_property); }
            set { SetValue(properties_property, value); }
        }
        public static readonly DependencyProperty properties_property = DependencyProperty.Register(
                "properties",
                typeof(Collection<NodeProperty>),
                typeof(PropertiesEditor),
                new PropertyMetadata(new Collection<NodeProperty>( () => new NodeProperty() ) ) );
        DependencyPropertyDescriptor properties_descriptor = DependencyPropertyDescriptor.FromProperty(properties_property, typeof(PropertiesEditor));
        #endregion properties

        #region nodes_collection
        public Collection<LogicalNode> nodes_collection {
            get { return (Collection<LogicalNode>)GetValue(nodes_collection_property); }
            set { SetValue(nodes_collection_property, value); }
        }
        public static readonly DependencyProperty nodes_collection_property = DependencyProperty.Register(
                "nodes_collection",
                typeof(Collection<LogicalNode>),
                typeof(PropertiesEditor),
                new PropertyMetadata( new Collection<LogicalNode>( () => new LogicalNode() ) ));
        #endregion nodes_collection

        #region idSelected
        public long idSelected {
            get { return (long)GetValue(idSelected_property); }
            set { SetValue(idSelected_property, value); }
        }
        public static readonly DependencyProperty idSelected_property = DependencyProperty.Register(
                "idSelected",
                typeof(long),
                typeof(PropertiesEditor),
                new PropertyMetadata( (long)0 ) );
        DependencyPropertyDescriptor idSelected_descriptor = DependencyPropertyDescriptor.FromProperty(idSelected_property, typeof(PropertiesEditor));
        #endregion idSelected
    }

}
