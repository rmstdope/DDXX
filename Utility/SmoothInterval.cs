using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Utility
{
    public class SmoothInterval
    {
        public static float ScaledSine(float startInterval, 
            float endInterval, float intervalPos, float resultMax)
        {
            float d = (intervalPos - startInterval) / (endInterval - startInterval);
            return (float)Math.Sin(Math.PI * d) * resultMax;
        }

        public static Vector3 SineInterpolation(float startInterval, float endInterval, 
            float intervalPos, Vector3 startPos, Vector3 endPos)
        {
            Vector3 middle = (startPos + endPos) * 0.5f;
            float d = (intervalPos - startInterval) / (endInterval - startInterval);
            float sineD = (float)Math.Cos(Math.PI * d);
            if (d < 0.5f)
            {
                return startPos + (middle - startPos) * (1 - sineD);
            }
            return endPos + (middle - endPos) * (1 + sineD);
        }
    }
}
