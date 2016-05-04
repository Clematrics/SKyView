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
        Blend, 
        Invert,
        Replace,

        Constant,
        Noise,
        LinearRamp,
        RadialRamp,

        Channels,

        Blur,
        Luminosity,
        Threshold,
        
        ColorSelection,
    }
}
