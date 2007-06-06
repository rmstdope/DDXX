using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.TextureBuilder
{
    public class PerlinTurbulence : PerlinNoise
    {
        public PerlinTurbulence(int numOctaves, float baseFrequency, float persistance)
            : base(numOctaves, baseFrequency, persistance)
        {
            createTurbulence = true;
        }
    }
}
