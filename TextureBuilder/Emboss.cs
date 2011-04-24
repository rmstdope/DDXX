using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dope.DDXX.TextureBuilder
{
    public class Emboss : Convolution
    {
        public Emboss()
            : base(new float[3, 3] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } }, false, true)
        {
        }
    }
}
