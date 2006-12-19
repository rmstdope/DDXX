using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Skinning
{
    public class SkinFrame : Frame
    {
        public SkinFrame()
            : base()
        {
            Name = "Unnamed";
        }

        public SkinFrame(string name)
            : base()
        {
            Name = name;
        }
    }
}
