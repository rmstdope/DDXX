using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public abstract class CubicSplineBase<Type> : SplineBase<Type>
        where Type : IArithmetic
    {
        /// <summary>
        /// The b(i) of the interpolation
        /// </summary>
        protected Type[] b;
        /// <summary>
        /// The c(i) of the interpolation
        /// </summary>
        protected Type[] c;
        /// <summary>
        /// The d(i) of the interpolation
        /// </summary>
        protected Type[] d;

        protected void CubicCommonCalculate()
        {
            b = new Type[keyFrames.Count];
            c = new Type[keyFrames.Count];
            d = new Type[keyFrames.Count];
        }

        protected float[] InitializeH()
        {
            float[] h = new float[keyFrames.Count];
            for (int i = 0; i < h.Length - 1; i++)
            {
                h[i] = keyFrames[i + 1].Time - keyFrames[i].Time;
            }
            return h;
        }

    }
}
