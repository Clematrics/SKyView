namespace SkyView.Utils {

    public enum PinType {
        Input,
        Output,
    }
    public delegate void PinSelectionEventHandler( object sender, PinType type, int index );
}
