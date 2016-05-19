using SkyView.Nodes;
using SkyView.Utils;
using System.Collections.Generic;
using System.ComponentModel;

namespace SkyView.RenderEngine {

    public class RenderEngine : INotifyPropertyChanged {

        public RenderEngine() {
            Result = new Image.Image(100, 100);
        }

        public Image.Image Result {
            get { return _Result; }
            set { _Result = value;  RaisePropertyChanged("Result"); }
        }
        private Image.Image _Result;

        public void Render (NodesAssembly Assembly) {
            LogicalNode OutputNode = Assembly.NodesCollection.Find(x => x.Type == NodeType.Output);
            int Width = int.Parse(OutputNode.Properties[0].Value);
            int Height = int.Parse(OutputNode.Properties[1].Value);

            LogicalOutputPin StartingPoint = OutputNode.InputPins[0].SourcePin.Output;

            Result = GetFilteredImage(StartingPoint, Width, Height);
        }

        private Image.Image GetFilteredImage(LogicalOutputPin current, int width, int height) {
            List<Image.Image> inputImages = new List<Image.Image>();
            Collection<NodeProperty> parameters = current.Parent.Properties;
            foreach (CollectionItem<LogicalInputPin> input in current.Parent.InputPins )
                inputImages.Add(GetFilteredImage( input.Member.SourcePin.Output, width, height ));
            return current.Filter(width, height, inputImages, parameters);
        }

        #region INotifyPropertyChanged
        protected void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }

}
