using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Dope.DDXX.Animation;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ContentProcessor
{
    [ContentTypeWriter]
    public class AnimationControllerWriter : ContentTypeWriter<AnimationController>
    {
        protected override void Write(ContentWriter output, AnimationController value)
        {
            output.WriteObject(value.AnimationClips);
            output.WriteObject(value.SkinInformation);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationControllerReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    public class SkinInformationWriter : ContentTypeWriter<SkinInformation>
    {
        protected override void Write(ContentWriter output, SkinInformation value)
        {
            output.WriteObject(value.BindPose);
            output.WriteObject(value.InverseBindPose);
            output.WriteObject(value.SkeletonHierarchy);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SkinInformationReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    public class IAnimationClipWriter : ContentTypeWriter<IAnimationClip>
    {
        protected override void Write(ContentWriter output, IAnimationClip value)
        {
            output.WriteObject(value.Duration);
            output.WriteObject(value.Animations);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationClipReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    public class AnimationClipWriter : ContentTypeWriter<AnimationClip>
    {
        protected override void Write(ContentWriter output, AnimationClip value)
        {
            output.WriteObject(value.Duration);
            output.WriteObject(value.Animations);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationClipReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    public class IKeyFrameAnimationWriter : ContentTypeWriter<IKeyFrameAnimation>
    {
        protected override void Write(ContentWriter output, IKeyFrameAnimation value)
        {
            output.WriteObject(value.KeyFrames);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(KeyFrameAnimationReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    public class KeyFrameAnimationWriter : ContentTypeWriter<KeyFrameAnimation>
    {
        protected override void Write(ContentWriter output, KeyFrameAnimation value)
        {
            output.WriteObject(value.KeyFrames);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(KeyFrameAnimationReader).AssemblyQualifiedName;
        }
    }

    [ContentTypeWriter]
    public class KeyFrameWriter : ContentTypeWriter<KeyFrame>
    {
        protected override void Write(ContentWriter output, KeyFrame value)
        {
            output.WriteObject(value.Time);
            output.WriteObject(value.Transform);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(KeyFrameReader).AssemblyQualifiedName;
        }
    }

}
