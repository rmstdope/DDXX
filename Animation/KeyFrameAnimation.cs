using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    public class KeyFrameAnimation : IKeyFrameAnimation
    {
        private IList<KeyFrame> keyFrames;

        public KeyFrameAnimation()
        {
            this.keyFrames = new List<KeyFrame>();
        }

        public KeyFrameAnimation(IList<KeyFrame> keyFrames)
        {
            this.keyFrames = keyFrames;
        }

        public IList<KeyFrame> KeyFrames
        {
            get { return keyFrames; }
        }

        public void Sort()
        {
            (keyFrames as List<KeyFrame>).Sort(CompareKeyframeTimes);
        }

        public Matrix GetBoneTransform(float time)
        {
            int index = 0;
            int i;
            for (i = index + 1; i < keyFrames.Count; i++)
            {
                if (time < keyFrames[i].Time)
                    return keyFrames[i - 1].Transform;
            }
            return keyFrames[i - 1].Transform;
        }

        static private int CompareKeyframeTimes(KeyFrame a, KeyFrame b)
        {
            return a.Time.CompareTo(b.Time);
        }

    }
}
