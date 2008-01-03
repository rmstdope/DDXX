using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    public class SkinInformation : ISkinInformation
    {
        private IList<Matrix> bindPose;
        private IList<Matrix> inverseBindPose;
        private IList<int> skeletonHierarchy;

        public SkinInformation(IList<Matrix> bindPose, IList<Matrix> inverseBindPose,
            IList<int> skeletonHierarchy)
        {
            this.bindPose = bindPose;
            this.inverseBindPose = inverseBindPose;
            this.skeletonHierarchy = skeletonHierarchy;
        }

        public int NumBones
        {
            get { return bindPose.Count; }
        }

        public IList<Matrix> BindPose
        {
            get { return bindPose; }
        }

        public IList<Matrix> InverseBindPose
        {
            get { return inverseBindPose; }
        }

        public IList<int> SkeletonHierarchy
        {
            get { return skeletonHierarchy; }
        }

        public int GetParentIndex(int bone)
        {
            return SkeletonHierarchy[bone];
        }

        public Matrix GetInvSkinPose(int bone)
        {
            return inverseBindPose[bone];
        }
    }
}
