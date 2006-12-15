using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Dope.DDXX.Utility
{
    public class LinearSpline<Type> : SplineBase<Type>
        where Type: IArithmetic
    {
        public LinearSpline()
        {
        }

        protected override void DoCalculate()
        {
        }

        protected override Type DoGetValue(float time)
        {
            int startKey = GetStartKey(time);
            float delta = (time - keyFrames[startKey].Time) / (keyFrames[startKey + 1].Time - keyFrames[startKey].Time);
            IArithmetic diff = keyFrames[startKey + 1].Value.Sub(keyFrames[startKey].Value);
            diff = diff.Mul(delta);
            return (Type)(keyFrames[startKey].Value).Add(diff);
        }

        protected override Type DoGetDerivative(float time)
        {
            if (time < StartTime || time > EndTime)
                return (Type)(keyFrames[0].Value).Zero();
            int startKey = GetStartKey(time);
            IArithmetic valueDiff = keyFrames[startKey + 1].Value.Sub(keyFrames[startKey].Value);
            return (Type)(valueDiff.Mul(1.0f / (keyFrames[startKey + 1].Time - keyFrames[startKey].Time)));
        }
    }
}
