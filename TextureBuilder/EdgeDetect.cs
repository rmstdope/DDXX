using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dope.DDXX.TextureBuilder
{
    public class EdgeDetect : Convolution
    {
        public EdgeDetect()
            : base(new float[3, 3] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } }, false, false)
        {
        }
    }
}
