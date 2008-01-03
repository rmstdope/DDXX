using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    public interface ISkinInformation
    {
        int NumBones { get; }
        IList<Matrix> BindPose { get; }
        IList<Matrix> InverseBindPose { get; }
        IList<int> SkeletonHierarchy { get; }
        int GetParentIndex(int bone);
        Matrix GetInvSkinPose(int bone);
    }
}
