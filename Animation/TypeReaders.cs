using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    public class AnimationControllerReader : ContentTypeReader<AnimationController>
    {
        protected override AnimationController 
            Read(ContentReader input, AnimationController existingInstance)
        {
            IDictionary<string, IAnimationClip> animationClips = input.ReadObject<IDictionary<string, IAnimationClip>>();
            SkinInformation skinInformation = input.ReadObject<SkinInformation>();

            return new AnimationController(animationClips, skinInformation);
        }
    }

    public class SkinInformationReader : ContentTypeReader<SkinInformation>
    {
        protected override SkinInformation
            Read(ContentReader input, SkinInformation existingInstance)
        {
            IList<Matrix> bindPose = input.ReadObject<IList<Matrix>>();
            IList<Matrix> inverseBindPose = input.ReadObject<IList<Matrix>>();
            IList<int> skeletonHierarchy = input.ReadObject<IList<int>>();

            return new SkinInformation(bindPose, inverseBindPose, skeletonHierarchy);
        }
    }

    public class AnimationClipReader : ContentTypeReader<IAnimationClip>
    {
        protected override IAnimationClip
            Read(ContentReader input, IAnimationClip existingInstance)
        {
            float duration = input.ReadObject<float>();
            IKeyFrameAnimation[] animations = input.ReadObject<IKeyFrameAnimation[]>();

            return new AnimationClip(duration, animations);
        }
    }

    public class KeyFrameReader : ContentTypeReader<KeyFrame>
    {
        protected override KeyFrame
            Read(ContentReader input, KeyFrame existingInstance)
        {
            float time = input.ReadObject<float>();
            Matrix transform = input.ReadObject<Matrix>();

            return new KeyFrame(time, transform);
        }
    }

    public class KeyFrameAnimationReader : ContentTypeReader<IKeyFrameAnimation>
    {
        protected override IKeyFrameAnimation
            Read(ContentReader input, IKeyFrameAnimation existingInstance)
        {
            IList<KeyFrame> keyFrames = input.ReadObject<IList<KeyFrame>>();

            return new KeyFrameAnimation(keyFrames);
        }
    }

    public class AnimationReader : ContentTypeReader<KeyFrameAnimation>
    {
        protected override KeyFrameAnimation
            Read(ContentReader input, KeyFrameAnimation existingInstance)
        {
            IList<KeyFrame> keyFrames = input.ReadObject<IList<KeyFrame>>();

            return new KeyFrameAnimation(keyFrames);
        }
    }

}
