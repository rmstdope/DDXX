using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public interface ISpline<Type>
        where Type : IArithmetic
    {
        List<KeyFrame<Type>> KeyFrames { get; }
        float StartTime { get; }
        float EndTime { get; }
        void AddKeyFrame(KeyFrame<Type> keyFrame);
        void Calculate();
        Type GetValue(float time);
        Type GetDerivative(float time);
    }
}
