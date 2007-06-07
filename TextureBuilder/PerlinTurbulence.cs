using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.TextureBuilder
{
    public class PerlinTurbulence : PerlinNoise
    {
        public PerlinTurbulence()
            : base()
        {
            createTurbulence = true;
        }
    }
}
