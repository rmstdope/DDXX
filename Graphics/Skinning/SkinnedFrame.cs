using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class SkinnedFrame : Frame
    {
        private Matrix matrix;

        public SkinnedFrame(string name)
            : base()
        {
            Name = name;
        }

        public Matrix CombinedTransformationMatrix
        {
            get
            {
                return matrix;
            }
            set
            {
                matrix = value;
            }
        }

    }
}
