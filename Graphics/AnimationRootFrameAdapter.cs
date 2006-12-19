using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class AnimationRootFrameAdapter : IAnimationRootFrame
    {
        private AnimationRootFrame animationRootFrame;

        public AnimationRootFrameAdapter(AnimationRootFrame rootFrame)
        {
            animationRootFrame = rootFrame;
        }

        #region IAnimationRootFrame Members

        public AnimationController AnimationController
        {
            get { return animationRootFrame.AnimationController; }
        }

        public IFrame FrameHierarchy
        {
            get { return null; } // animationRootFrame.FrameHierarchy; }
        }

        #endregion
    }
}
