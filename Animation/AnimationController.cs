using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Animation
{
    public class AnimationController : IAnimationController
    {
        private IDictionary<string, IAnimationClip> animationClips;
        private ISkinInformation skinInformation;
        private Matrix[] worldMatrices;
        private PlayMode playMode;
        private float speed;

        public AnimationController(IDictionary<string, IAnimationClip> animationClips, ISkinInformation skin)
        {
            if (animationClips == null || animationClips.Count == 0)
                throw new ArgumentException("Must have at least one AnimationClip.", "animationClips");
            this.skinInformation = skin;
            this.animationClips = animationClips;
            this.worldMatrices = new Matrix[skin.NumBones];
            this.playMode = PlayMode.Loop;
            this.speed = 1.0f;
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public PlayMode PlayMode
        {
            get { return playMode; }
            set { playMode = value; }
        }

        public IDictionary<string, IAnimationClip> AnimationClips
        {
            get { return animationClips; }
        }

        public ISkinInformation SkinInformation
        {
            get { return skinInformation; }
        }

        public Matrix[] WorldMatrices
        {
            get { return worldMatrices; }
        }

        public void Step(Matrix rootWorldMatrix)
        {
            IAnimationClip[] clips = new IAnimationClip[animationClips.Count];
            animationClips.Values.CopyTo(clips, 0);
            IAnimationClip animationClip = clips[0];
            Matrix[] boneTransforms = animationClip.GetBoneTransforms(AdjustedTime(animationClip));

            // Root bone.
            worldMatrices[0] =  boneTransforms[0] * rootWorldMatrix;

            // Child bones.
            for (int bone = 1; bone < skinInformation.NumBones; bone++)
            {
                int parentBone = skinInformation.GetParentIndex(bone);
                worldMatrices[bone] = boneTransforms[bone] * worldMatrices[parentBone];
            }

            for (int bone = 0; bone < skinInformation.NumBones; bone++)
            {
                worldMatrices[bone] = skinInformation.GetInvSkinPose(bone) * worldMatrices[bone];
            }
            //for (int i = 0; i < worldMatrices.Length; i++)
            //{
            //    worldMatrices[i] = Matrix.Identity;
            //}
            //worldMatrices[0] = worldMatrices[0] * rootWorldMatrix;
        }

        private float AdjustedTime(IAnimationClip animationClip)
        {
            float time = Time.CurrentTime * speed;
            if (playMode == PlayMode.Loop)
            {
                return time % animationClip.Duration;
            }
            if (playMode == PlayMode.PingPongLoop)
            {
                if (((int)(time / animationClip.Duration) & 1) == 1)
                    return animationClip.Duration - time % animationClip.Duration;
                else
                    return time % animationClip.Duration;
            }
            return time;
        }
    }
}
