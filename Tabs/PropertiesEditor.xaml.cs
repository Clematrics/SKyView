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
            Properties = new Collection<NodeProperty>( () => new NodeProperty() );
            nodesAssemblyPropertiesEditor = new NodesAssembly();

            propertiesDescriptor.AddValueChanged(this, Properties_PropertyChanged);

            InitializeComponent();

            ItemsControlScroller.DataContext = this;
        }

        private void Properties_PropertyChanged(object sender, EventArgs e) {
            if (nodesAssemblyPropertiesEditor.IdSelected != 0) nodesAssemblyPropertiesEditor.NodesCollection.Find(x => x.Id == nodesAssemblyPropertiesEditor.IdSelected).Properties = Properties;
        }

        public void IdChanged(object sender, PropertyChangedEventArgs e) {
            if (nodesAssemblyPropertiesEditor.IdSelected == 0)
                Properties = new Collection<NodeProperty>( () => new NodeProperty() );
            else Properties = nodesAssemblyPropertiesEditor.NodesCollection.Find(x => x.Id == nodesAssemblyPropertiesEditor.IdSelected).Properties;
        }

        #region PropertiesProperty
        public Collection<NodeProperty> Properties {
            get { return (Collection<NodeProperty>)GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }
        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(
                "Properties",
                typeof(Collection<NodeProperty>),
                typeof(PropertiesEditor),
                new PropertyMetadata(new Collection<NodeProperty>( () => new NodeProperty() ) ) );
        DependencyPropertyDescriptor propertiesDescriptor = DependencyPropertyDescriptor.FromProperty(PropertiesProperty, typeof(PropertiesEditor));
        #endregion PropertiesProperty

        #region nodesAssembly Property
        public NodesAssembly nodesAssemblyPropertiesEditor {
            get { return (NodesAssembly)GetValue(nodesAssemblyPropertiesEditorProperty); }
            set { SetValue(nodesAssemblyPropertiesEditorProperty, value); }
        }
        public static readonly DependencyProperty nodesAssemblyPropertiesEditorProperty = DependencyProperty.Register(
            "nodesAssemblyPropertiesEditor",
            typeof(NodesAssembly),
            typeof(PropertiesEditor),
            new PropertyMetadata(new NodesAssembly()));
        #endregion nodesAssembly Property
    }

}
