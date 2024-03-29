using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class NaturalCubicSpline<Type> : CubicSplineBase<Type>
        where Type : IArithmetic
    {
        protected override void DoCalculate()
        {
            CreateArrays();

            float[] l = new float[keyFrames.Count];
            float[] u = new float[keyFrames.Count];
            Type[] z = new Type[keyFrames.Count];
            Type[] y = new Type[keyFrames.Count];

            // Calculate h vector
            float[] h = InitializeH();

            // Calculate y vector
            for (int i = 1; i < y.Length - 1; i++)
            {
                //y[i] = (3 / h[i]) * (keyFrames[i + 1].Value.Sub(keyFrames[i].Value)) - (3 / h[i - 1]) * (keyFrames[i].Value.Sub(keyFrames[i - 1].Value));
                IArithmetic diff1 = keyFrames[i + 1].Value.Sub(keyFrames[i].Value);
                diff1 = diff1.Mul(3 / h[i]);
                IArithmetic diff2 = keyFrames[i].Value.Sub(keyFrames[i - 1].Value);
                diff2 = diff2.Mul(3 / h[i - 1]);
                y[i] = (Type)(diff1.Sub(diff2));
            }

            // Create additional vectors l, z, and c
            l[0] = 1.0f;
            u[0] = 0.0f;
            z[0] = (Type)(keyFrames[0].Value.Zero());
            for (int i = 1; i < l.Length - 1; i++)
            {
                l[i] = 2 * (keyFrames[i + 1].Time - keyFrames[i - 1].Time) - h[i - 1] * u[i - 1];
                u[i] = h[i] / l[i];
                //z[i] = (y[i] - h[i - 1] * z[i - 1]) / l[i];
                //IArithmetic value = z[i - 1].Mul(h[i - 1]);
                z[i] = (Type)(y[i].Sub(z[i - 1].Mul(h[i - 1])).Mul(1.0f / l[i]));//  y[i].Sub(value).Mul(1 / l[i]));
            }
            l[l.Length - 1] = 1.0f;
            z[z.Length - 1] = (Type)(keyFrames[0].Value.Zero()); ;
            c[c.Length - 1] = (Type)(keyFrames[0].Value.Zero()); ;

            for (int i = c.Length - 2; i >= 0; i--) 
            {
                //mC[i] = z[i] - u[i] * mC[i + 1];
                c[i] = (Type)(z[i].Sub(c[i + 1].Mul(u[i])));
                //mB[i] = (mFx[i + 1] - mFx[i]) / h[i] - h[i] * (mC[i + 1] + 2 * mC[i]) / 3;
                b[i] = (Type)(keyFrames[i + 1].Value.Sub(keyFrames[i].Value).Mul(1 / h[i]).Sub(c[i + 1].Add(c[i].Mul(2)).Mul(h[i] / 3)));
                //mD[i] = (mC[i + 1] - mC[i]) / (3 * h[i]);
                d[i] = (Type)(c[i + 1].Sub(c[i]).Mul(1 / (3 * h[i])));
            }
        }
    }
}
