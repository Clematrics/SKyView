using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyView.Nodes {
    public enum NodeType {
        Unknown,

        Image,
        Output,

        Add,
        Substract, 
        Multiply, 
        Divide, 
        Over, 
        Invert,

        Constant,
        Noise,
        LinearRamp,
        RadialRamp,

        Channels,
        CombineChannels,

        GrayScale,
        Blur,
        Luminosity,
        Threshold,
        
        ColorSelection,
    }
}
