using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public abstract class SplineBase<Type> : ISpline<Type>
        where Type : IArithmetic
    {
        protected List<KeyFrame<Type>> keyFrames;

        public SplineBase()
        {
            keyFrames = new List<KeyFrame<Type>>();
        }

        public float StartTime
        {
            get
            {
                if (keyFrames.Count == 0)
                    throw new DDXXException("StartTime called on spline without key frames.");
                return keyFrames[0].Time;
            }
        }

        public float EndTime
        {
            get
            {
                if (keyFrames.Count == 0)
                    throw new DDXXException("EndTime called on spline without key frames.");
                return keyFrames[keyFrames.Count - 1].Time;
            }
        }

        public void AddKeyFrame(KeyFrame<Type> keyFrame)
        {
            if (keyFrames.Exists(delegate(KeyFrame<Type> element)
                                 { return element.Time == keyFrame.Time; }))
                throw new DDXXException("Added two key frames with the same time.");
            keyFrames.Add(keyFrame);
            keyFrames.Sort(delegate(KeyFrame<Type> element1, KeyFrame<Type> element2)
                           {
                               if (element1.Time > element2.Time)
                                   return 1;
                               if (element1.Time < element2.Time)
                                   return -1;
                               return 0;
                           });
        }

        protected int GetStartKey(float time)
        {
            int startKey = 0;
            while (startKey < (keyFrames.Count - 2) &&
                   keyFrames[startKey + 1].Time <= time)
                startKey++;
            return startKey;
        }

        public void Calculate()
        {
            if (keyFrames.Count == 0)
                throw new DDXXException("Calculate called on empty spline.");
            DoCalculate();
        }

        public Type GetValue(float time)
        {
            if (keyFrames.Count == 0)
                throw new DDXXException("Calculate called on empty spline.");
            if (time <= StartTime)
                return keyFrames[0].Value;
            if (time >= EndTime)
                return keyFrames[keyFrames.Count - 1].Value;
            return DoGetValue(time);
        }

        public Type GetDerivative(float time)
        {
            if (keyFrames.Count == 0)
                throw new DDXXException("Calculate called on empty spline.");
            return DoGetDerivative(time);
        }

        protected abstract void DoCalculate();
        protected abstract Type DoGetValue(float time);
        protected abstract Type DoGetDerivative(float time);

    }
}
