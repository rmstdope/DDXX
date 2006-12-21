using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public interface IAnimationRootFrame
    {
        // Summary:
        //     Retrieves the animation controller.
        AnimationController AnimationController { get; }
        //
        // Summary:
        //     Retrieves the root node reference of the loaded frame1 hierarchy.
        IFrame FrameHierarchy { get; }
    }
}
