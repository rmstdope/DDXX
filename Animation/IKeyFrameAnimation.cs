using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Animation
{
    public interface IKeyFrameAnimation
    {
        IList<KeyFrame> KeyFrames { get; }
        void Sort();

        Microsoft.Xna.Framework.Matrix GetBoneTransform(float time);
    }
}
