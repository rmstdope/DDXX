using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class AspectRatio
    {
        int width;
        int height;

        public enum Ratios
        {
            RATIO_INVALID,
            RATIO_4_3,
            RATIO_16_9,
            RATIO_16_10
        }

        public AspectRatio(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Ratios Ratio
        {
            get
            {
                if (width * 3 == height * 4)
                    return Ratios.RATIO_4_3;
                if (width * 9== height * 16)
                    return Ratios.RATIO_16_9;
                if (width * 10 == height * 16)
                    return Ratios.RATIO_16_10;
                return Ratios.RATIO_INVALID;
            }
        }
    }
}
