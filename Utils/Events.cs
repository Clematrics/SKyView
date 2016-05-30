using System.Windows;

namespace SkyView.Utils {

    public enum PinType {
        Input,
        Output,
    }
    public delegate void LinkDraggingEventHandler( object sender, PinType type );
    public delegate void NodePositionChangedEventHandler( object sender, Vector vectorMovment );
}
