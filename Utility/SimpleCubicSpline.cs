using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class SimpleCubicSpline<Type> : SplineBase<Type>
        where Type : IArithmetic
    {
        protected override void DoCalculate()
        {
        }

        protected override Type DoGetValue(float time)
        {
            int k2 = GetStartKey(time);
            int k1 = k2 < 1 ? k2 : k2 - 1;
            int k3 = k2 >= keyFrames.Count - 1 ? k2 : k2 + 1;
            int k4 = k3 >= keyFrames.Count - 1 ? k3 : k3 + 1;
            float len = keyFrames[k3].Time - keyFrames[k2].Time;
            float d;
            if (len == 0)
                d = 0;
            else
                d = (time - keyFrames[k2].Time) / len;
            Type P = (Type)keyFrames[k4].Value.Sub(keyFrames[k3].Value).Sub((keyFrames[k1].Value.Sub(keyFrames[k2].Value)));
            Type Q = (Type)keyFrames[k1].Value.Sub(keyFrames[k2].Value).Sub(P);
            Type R = (Type)keyFrames[k3].Value.Sub(keyFrames[k1].Value);
            Type S = (Type)keyFrames[k2].Value;
            return (Type)P.Mul(d * d * d).Add(Q.Mul(d * d)).Add(R.Mul(d)).Add(S);
        }

        protected override Type DoGetDerivative(float time)
        {
            const float epsilon = 0.001f;
            return (Type)DoGetValue(time + epsilon).Sub(DoGetValue(time)).Mul(1.0f / epsilon);
        }
    }
}
