using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Dope.DDXX.Utility
{
    public class LinearSpline<Type>
        where Type: IArithmetic
    {
        private List<KeyFrame<Type>> keyFrames;

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

        public LinearSpline()
        {
            keyFrames = new List<KeyFrame<Type>>();
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

        public void Calculate()
        {
            if (keyFrames.Count == 0)
                throw new DDXXException("Calculate called on empty spline.");
        }

        public Type GetValue(float time)
        {
            if (keyFrames.Count == 0)
                throw new DDXXException("Calculate called on empty spline.");
            if (time <= StartTime)
                return keyFrames[0].Value;
            if (time >= EndTime)
                return keyFrames[keyFrames.Count - 1].Value;
            int startKey = GetStartKey(time);
            float delta = (time - keyFrames[startKey].Time) / (keyFrames[startKey + 1].Time - keyFrames[startKey].Time);
            IArithmetic diff = keyFrames[startKey + 1].Value.Sub(keyFrames[startKey].Value);
            diff = diff.Mul(delta);
            return (Type)(keyFrames[startKey].Value).Add(diff);
        }

        public Type GetDerivative(float time)
        {
            if (time < StartTime || time > EndTime)
                return (Type)(keyFrames[0].Value).Zero();
            if (keyFrames.Count == 0)
                throw new DDXXException("Calculate called on empty spline.");
            int startKey = GetStartKey(time);
            if (time == EndTime)
                startKey--;
            IArithmetic valueDiff = keyFrames[startKey + 1].Value.Sub(keyFrames[startKey].Value);
            return (Type)(valueDiff.Mul(1.0f / (keyFrames[startKey + 1].Time - keyFrames[startKey].Time)));
        }

        private int GetStartKey(float time)
        {
            int startKey = 0;
            while (startKey < (keyFrames.Count - 1) &&
                   keyFrames[startKey + 1].Time <= time)
                startKey++;
            return startKey;
        }

    }
}
