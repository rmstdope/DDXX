using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class ClampedCubicSpline<Type> : CubicSplineBase<Type>
        where Type : IArithmetic
    {
        Type dX0;
        Type dXn;

        public ClampedCubicSpline(Type dX0, Type dXn)
            : base()
        {
            this.dX0 = dX0;
            this.dXn = dXn;
        }
        
        protected override void DoCalculate()
        {
            CreateArrays();

            float[] l = new float[keyFrames.Count];
            float[] u = new float[keyFrames.Count];
            Type[] z = new Type[keyFrames.Count];
            Type[] e = new Type[keyFrames.Count];

            // Calculate h vector
            float[] h = InitializeH();

            // Calculate y vector
            e[0] = (Type)(keyFrames[1].Value.Sub(keyFrames[0].Value).Mul(3 / h[0]).Sub(dX0.Mul(3)));
            e[e.Length - 1] = (Type)(dXn.Mul(3).Sub(keyFrames[e.Length - 1].Value.Sub(keyFrames[e.Length - 2].Value).Mul(3 / h[e.Length - 2])));
            for (int i = 1; i < e.Length - 1; i++)
            {
                e[i] = (Type)(keyFrames[i + 1].Value.Sub(keyFrames[i].Value).Mul(3 / h[i]).Sub(keyFrames[i].Value.Sub(keyFrames[i - 1].Value).Mul(3 / h[i - 1])));
            }

            // Create additional vectors l, z, and c
            l[0] = 2.0f * h[0];
            u[0] = 0.5f;
            z[0] = (Type)(e[0].Mul(1 / l[0]));
            for (int i = 1; i < l.Length - 1; i++)
            {
                l[i] = 2 * (keyFrames[i + 1].Time - keyFrames[i - 1].Time) - h[i - 1] * u[i - 1];
                u[i] = h[i] / l[i];
                //z[i] = (e[i] - h[i - 1] * z[i - 1]) / l[i];
                IArithmetic value = z[i - 1].Mul(h[i - 1]);
                z[i] = (Type)(e[i].Sub((z[i - 1].Mul(h[i - 1]))).Mul(1 / l[i]));//         e[i].Sub(value).Mul(1 / l[i]));
            }
            l[l.Length - 1] = h[h.Length - 2] * (2 - u[u.Length - 2]);
            z[z.Length - 1] = (Type)(e[e.Length - 1].Sub((z[z.Length - 2].Mul(h[h.Length - 2]))).Mul(1.0f / l[l.Length - 1]));
            c[c.Length - 1] = z[z.Length - 1];

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
