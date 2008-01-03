using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Animation
{
    public interface IAnimationClip
    {
        void SetAnimation(IKeyFrameAnimation animation, int boneId);
        float Duration { get; }
        IKeyFrameAnimation[] Animations { get; }
        void ValidateAndSort();

        Microsoft.Xna.Framework.Matrix[] GetBoneTransforms(float p);
    }
}
