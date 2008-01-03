using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    public class AnimationClip : IAnimationClip
    {
        private float duration;
        private IKeyFrameAnimation[] animations;

        public AnimationClip(float duration, int numBones)
        {
            this.duration = duration;
            animations = new IKeyFrameAnimation[numBones];
        }

        public AnimationClip(float duration, IKeyFrameAnimation[] animations)
        {
            this.duration = duration;
            this.animations = animations;
        }

        public void SetAnimation(IKeyFrameAnimation animation, int boneId)
        {
            animations[boneId] = animation;
        }

        public float Duration
        {
            get { return duration; }
        }

        public IKeyFrameAnimation[] Animations
        {
            get { return animations; }
        }

        public void ValidateAndSort()
        {
            foreach (IKeyFrameAnimation animation in animations)
            {
                animation.Sort();
            }
        }

        public Matrix[] GetBoneTransforms(float time)
        {
            Matrix[] matrices = new Matrix[animations.Length];
            for (int i = 0; i < animations.Length; i++)
                matrices[i] = animations[i].GetBoneTransform(time);
            //.KeyFrames[0].Transform;
            return matrices;
        }

    }
}
